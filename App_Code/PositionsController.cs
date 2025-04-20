using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Diagnostics;

namespace Stability
{
// Enable CORS for this controller
[EnableCors(origins: "http://localhost:19006", headers: "*", methods: "*", SupportsCredentials = true)]
[RoutePrefix("api/deployments")]
    public class PositionsController : ApiController
    {
        private readonly CrowdReliefDBDataContext db = new CrowdReliefDBDataContext();

        public class PositionDto
        {
            public Guid id { get; set; }
            public string date { get; set; }
            public string role { get; set; }
            public string time { get; set; }
            public int spotsRemaining { get; set; }
        }

        [HttpGet, Route("{id:guid}/positions")]
        public IHttpActionResult GetPositions(Guid id)
        {
            System.Diagnostics.Debug.WriteLine($"GetPositions called for deployment ID: {id}");
            
            try
            {
                // Get all positions for this deployment that aren't deleted
                var allPositions = db.OrganizationEventPositions
                    .Where(p => p.OrganizationEventId == id && !p.IsDeleted)
                    .ToList();
                
                System.Diagnostics.Debug.WriteLine($"Found {allPositions.Count} total positions for deployment");
                
                // Create a collection to store all available positions
                var availablePositions = new System.Collections.Generic.List<PositionDto>();
                
                // For each position, check if there are remaining spots based on NumberNeeded
                foreach(var position in allPositions)
                {
                    // Count active claims for this position
                    int activeClaimsCount = db.UserOrganizationEventPositions
                        .Count(c => c.OrganizationEventPositionId == position.OrganizationEventPositionId && c.IsActive);
                    
                    // Calculate remaining spots
                    int spotsRemaining = position.NumberNeeded - activeClaimsCount;
                    
                    System.Diagnostics.Debug.WriteLine($"Position {position.OrganizationEventPositionId} ({position.Position.Name}): {activeClaimsCount}/{position.NumberNeeded} spots claimed, {spotsRemaining} spots remaining");
                    
                    // Only include positions that have remaining spots
                    if (spotsRemaining > 0)
                    {
                        availablePositions.Add(new PositionDto
                        {
                            id = position.OrganizationEventPositionId,
                            date = position.DeploymentDate.HasValue ? position.DeploymentDate.Value.ToString("yyyy-MM-dd") : null,
                            role = position.Position.Name,
                            time = (position.ArrivalTime.HasValue && position.DepartureTime.HasValue)
                                ? position.ArrivalTime.Value.ToString("hh\\:mm") + " - " + position.DepartureTime.Value.ToString("hh\\:mm")
                                : null,
                            spotsRemaining = spotsRemaining
                        });
                    }
                }
                    
                System.Diagnostics.Debug.WriteLine($"Returning {availablePositions.Count} available positions");
                return Ok(availablePositions);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetPositions: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}