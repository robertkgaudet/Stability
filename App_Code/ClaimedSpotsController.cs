using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;

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
        }

        [HttpPost, Route("")]
        [Authorize]
        public IHttpActionResult Post(ClaimInput model)
        {
            if (model == null) return BadRequest("Invalid input");
            var user = Membership.GetUser(User.Identity.Name);
            var uid = (Guid)user.ProviderUserKey;
            
            // Fetch raw data first without string formatting
            var pos = db.OrganizationEventPositions
                .Where(p => p.OrganizationEventId == model.deploymentId && p.DeploymentDate.HasValue && p.Position.Name == model.role)
                .ToList()
                .FirstOrDefault(p => p.DeploymentDate.Value.ToString("yyyy-MM-dd") == model.date);
                
            if (pos == null) return BadRequest("Position not found");
            
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
            
            var dto = new ClaimDto
            {
                id = claim.UserOrganizationEventPositionId,
                deploymentName = pos.OrganizationEvent.CampaignName,
                date = pos.DeploymentDate.Value.ToString("yyyy-MM-dd"),
                role = pos.Position.Name,
                time = model.time,
                location = pos.OrganizationEvent.StagingCity
            };
            return Created(string.Empty, dto);
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