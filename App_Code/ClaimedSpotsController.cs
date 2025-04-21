using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using System.Diagnostics;

namespace Stability
{
// Enable CORS for this controller
[EnableCors(origins: "http://localhost:19006", headers: "*", methods: "*", SupportsCredentials = true)]
[RoutePrefix("api/claimedspots")]
    public class ClaimedSpotsController : ApiController
    {
        private readonly CrowdReliefDBDataContext db = new CrowdReliefDBDataContext();

        public class ClaimDto
        {
            public Guid id { get; set; }
            public string deploymentName { get; set; }
            public string date { get; set; }
            public string role { get; set; }
            public string time { get; set; }
            public string location { get; set; }
        }

        [HttpGet, Route("")]
        [Authorize]
        public IHttpActionResult Get()
        {
            var user = Membership.GetUser(User.Identity.Name);
            var uid = (Guid)user.ProviderUserKey;
            
            // First get raw data without any string formatting
            var rawData = db.UserOrganizationEventPositions
                .Where(c => c.UserId == uid && c.IsActive)
                .Select(c => new 
                {
                    c.UserOrganizationEventPositionId,
                    c.OrganizationEventPosition.OrganizationEvent.CampaignName,
                    c.OrganizationEventPosition.DeploymentDate,
                    c.OrganizationEventPosition.Position.Name,
                    c.OrganizationEventPosition.ArrivalTime,
                    c.OrganizationEventPosition.DepartureTime,
                    c.OrganizationEventPosition.OrganizationEvent.StagingCity
                })
                .ToList();

            // Then apply string formatting in memory
            var list = rawData.Select(c => new ClaimDto
            {
                id = c.UserOrganizationEventPositionId,
                deploymentName = c.CampaignName,
                date = c.DeploymentDate.HasValue ? c.DeploymentDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                role = c.Name,
                time = (c.ArrivalTime.HasValue && c.DepartureTime.HasValue)
                    ? FormatTimeSpan(c.ArrivalTime.Value) + " - " + FormatTimeSpan(c.DepartureTime.Value)
                    : null,
                location = c.StagingCity
            }).ToList();
            
            return Ok(list);
        }

        // Helper method to format TimeSpan objects
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            DateTime dateTime = DateTime.Today.Add(timeSpan);
            return dateTime.ToString("hh:mm");
        }

        public class ClaimInput
        {
            public Guid deploymentId { get; set; }
            public string date { get; set; }
            public string role { get; set; }
            public string time { get; set; }
            public string location { get; set; }
            public Guid id { get; set; }
            public string idString { get; set; }
        }

        [HttpPost, Route("")]
        [Authorize]
        public IHttpActionResult Post(ClaimInput model)
        {
            if (model == null) return BadRequest("Invalid input");
            var user = Membership.GetUser(User.Identity.Name);
            var uid = (Guid)user.ProviderUserKey;
            
            // Log the received input
            System.Diagnostics.Debug.WriteLine($"Claim request received: DeploymentId={model.deploymentId}, Role={model.role}, Date={model.date}, ID={model.id}");
            
            try
            {
                // Check if we need to use position ID if provided
                if (model.id != Guid.Empty || !string.IsNullOrEmpty(model.idString))
                {
                    // Use the position ID directly if provided
                    System.Diagnostics.Debug.WriteLine($"Looking up position by ID: {model.id} or idString: {model.idString}");
                    
                    Guid positionId;
                    // If the ID is a string in the JSON, try to parse it
                    if (model.id == Guid.Empty && !string.IsNullOrEmpty(model.idString))
                    {
                        System.Diagnostics.Debug.WriteLine($"Trying to parse string ID: {model.idString}");
                        if (!Guid.TryParse(model.idString, out positionId))
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to parse position ID from string: {model.idString}");
                            return BadRequest("Invalid position ID format");
                        }
                    }
                    else
                    {
                        positionId = model.id;
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Looking up position with ID: {positionId}");
                    var pos = db.OrganizationEventPositions.FirstOrDefault(p => 
                        p.OrganizationEventPositionId == positionId);
                        
                    if (pos == null) 
                    {
                        System.Diagnostics.Debug.WriteLine($"Position not found with ID: {positionId}");
                        return BadRequest("Position not found");
                    }
                    
                    // Count existing active claims for this position
                    int activeClaimsCount = db.UserOrganizationEventPositions
                        .Count(c => c.OrganizationEventPositionId == pos.OrganizationEventPositionId && c.IsActive);
                        
                    System.Diagnostics.Debug.WriteLine($"Position found. NumberNeeded={pos.NumberNeeded}, ActiveClaims={activeClaimsCount}");
                    
                    // Check if there are remaining spots based on NumberNeeded
                    if (activeClaimsCount >= pos.NumberNeeded)
                    {
                        System.Diagnostics.Debug.WriteLine($"Position {pos.OrganizationEventPositionId} is full: {activeClaimsCount}/{pos.NumberNeeded} spots claimed");
                        // All spots are filled
                        return BadRequest("All spots for this position have already been claimed");
                    }
                    
                    var claim = new UserOrganizationEventPosition
                    {
                        UserOrganizationEventPositionId = Guid.NewGuid(),
                        UserId = uid,
                        OrganizationEventPositionId = pos.OrganizationEventPositionId,
                        CreatedOn = DateTime.UtcNow,
                        IsActive = true
                    };
                    db.UserOrganizationEventPositions.InsertOnSubmit(claim);
                    db.SubmitChanges();
                    
                    System.Diagnostics.Debug.WriteLine($"Successfully claimed position {pos.OrganizationEventPositionId} for user {uid}");
                    
                    var dto = new ClaimDto
                    {
                        id = claim.UserOrganizationEventPositionId,
                        deploymentName = pos.OrganizationEvent.CampaignName,
                        date = pos.DeploymentDate.HasValue ? pos.DeploymentDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                        role = pos.Position.Name,
                        time = model.time,
                        location = pos.OrganizationEvent.StagingCity
                    };
                    return Created(string.Empty, dto);
                }
                else
                {
                    // Try to find by other attributes when ID is not provided
                    System.Diagnostics.Debug.WriteLine($"Looking up position by attributes: DeploymentId={model.deploymentId}, Role={model.role}, Date={model.date}");
                    
                    var query = db.OrganizationEventPositions
                        .Where(p => p.OrganizationEventId == model.deploymentId);
                        
                    if (!string.IsNullOrEmpty(model.role))
                    {
                        query = query.Where(p => p.Position.Name == model.role);
                    }
                    
                    // Get all matching positions and filter by date in memory 
                    // (since DateTime formatting in LINQ to SQL can be problematic)
                    var positions = query.ToList();
                    System.Diagnostics.Debug.WriteLine($"Found {positions.Count} positions matching deployment ID and role");
                    
                    // Apply date filter if present
                    if (!string.IsNullOrEmpty(model.date))
                    {
                        positions = positions.Where(p => 
                            p.DeploymentDate.HasValue && 
                            p.DeploymentDate.Value.ToString("yyyy-MM-dd") == model.date).ToList();
                        System.Diagnostics.Debug.WriteLine($"After date filter: {positions.Count} positions remain");
                    }
                    
                    var pos = positions.FirstOrDefault();
                        
                    if (pos == null)
                    {
                        System.Diagnostics.Debug.WriteLine("No positions found matching the criteria");
                        return BadRequest("Position not found");
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Found position {pos.OrganizationEventPositionId}");
                    
                    // Count existing active claims for this position
                    int activeClaimsCount = db.UserOrganizationEventPositions
                        .Count(c => c.OrganizationEventPositionId == pos.OrganizationEventPositionId && c.IsActive);
                        
                    System.Diagnostics.Debug.WriteLine($"Position info: NumberNeeded={pos.NumberNeeded}, ActiveClaims={activeClaimsCount}");
                    
                    // Check if there are remaining spots based on NumberNeeded
                    if (activeClaimsCount >= pos.NumberNeeded)
                    {
                        // All spots are filled
                        System.Diagnostics.Debug.WriteLine($"Position {pos.OrganizationEventPositionId} is full: {activeClaimsCount}/{pos.NumberNeeded} spots claimed");
                        return BadRequest("All spots for this position have already been claimed");
                    }
                    
                    var claim = new UserOrganizationEventPosition
                    {
                        UserOrganizationEventPositionId = Guid.NewGuid(),
                        UserId = uid,
                        OrganizationEventPositionId = pos.OrganizationEventPositionId,
                        CreatedOn = DateTime.UtcNow,
                        IsActive = true
                    };
                    db.UserOrganizationEventPositions.InsertOnSubmit(claim);
                    db.SubmitChanges();
                    
                    System.Diagnostics.Debug.WriteLine($"Successfully claimed position {pos.OrganizationEventPositionId} for user {uid}");
                    
                    var dto = new ClaimDto
                    {
                        id = claim.UserOrganizationEventPositionId,
                        deploymentName = pos.OrganizationEvent.CampaignName,
                        date = pos.DeploymentDate.HasValue ? pos.DeploymentDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                        role = pos.Position.Name,
                        time = model.time,
                        location = pos.OrganizationEvent.StagingCity
                    };
                    return Created(string.Empty, dto);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in claim process: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route("{id:guid}")]
        [Authorize]
        public IHttpActionResult Delete(Guid id)
        {
            var user = Membership.GetUser(User.Identity.Name);
            var uid = (Guid)user.ProviderUserKey;
            var claim = db.UserOrganizationEventPositions.FirstOrDefault(c => c.UserOrganizationEventPositionId == id && c.UserId == uid);
            if (claim == null) return NotFound();
            db.UserOrganizationEventPositions.DeleteOnSubmit(claim);
            db.SubmitChanges();
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}