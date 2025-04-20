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
            var list = db.UserOrganizationEventPositions
                .Where(c => c.UserId == uid && c.IsActive)
                .Select(c => new ClaimDto
                {
                    id = c.UserOrganizationEventPositionId,
                    deploymentName = c.OrganizationEventPosition.OrganizationEvent.CampaignName,
                    date = c.OrganizationEventPosition.DeploymentDate.Value.ToString("yyyy-MM-dd"),
                    role = c.OrganizationEventPosition.Position.Name,
                    time = (c.OrganizationEventPosition.ArrivalTime.HasValue && c.OrganizationEventPosition.DepartureTime.HasValue)
                        ? c.OrganizationEventPosition.ArrivalTime.Value.ToString("hh\\:mm") + " - " + c.OrganizationEventPosition.DepartureTime.Value.ToString("hh\\:mm")
                        : null,
                    location = c.OrganizationEventPosition.OrganizationEvent.StagingCity
                })
                .ToList();
            return Ok(list);
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
            var pos = db.OrganizationEventPositions
                .FirstOrDefault(p => p.OrganizationEventId == model.deploymentId
                    && p.DeploymentDate.HasValue && p.DeploymentDate.Value.ToString("yyyy-MM-dd") == model.date
                    && p.Position.Name == model.role);
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