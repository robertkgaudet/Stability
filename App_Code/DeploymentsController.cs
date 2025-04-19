using System;
using System.Linq;
using System.Web.Http;

namespace Stability
{
    [RoutePrefix("api/deployments")]
    public class DeploymentsController : ApiController
    {
        private readonly CrowdReliefDBDataContext db = new CrowdReliefDBDataContext();

        public class DeploymentDto
        {
            public Guid id { get; set; }
            public string title { get; set; }
            public string location { get; set; }
            public string organization { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string duration { get; set; }
            public int teamMembers { get; set; }
            public int slots { get; set; }
            public string logoUrl { get; set; }
        }

        [HttpGet, Route("")]
        public IHttpActionResult GetDeployments()
        {
            var items = db.OrganizationEvents
                .Where(e => e.AcceptsVolunteers == true && (e.IsActive ?? false))
                .AsEnumerable() // switch to LINQ-to-Objects for DateTime operations
                .Select(e => new DeploymentDto
                {
                    id = e.OrganizationEventId,
                    title = e.CampaignName,
                    location = e.StagingCity,
                    organization = e.Organization.Name,
                    startDate = e.BeginDate,
                    endDate = e.EndDate,
                    duration = e.BeginDate.HasValue && e.EndDate.HasValue
                        ? ((e.EndDate.Value - e.BeginDate.Value).Days + " days")
                        : null,
                    teamMembers = e.UserOrganizationEvents.Count,
                    slots = e.OrganizationEventPositions.Count,
                    logoUrl = string.IsNullOrEmpty(e.LogoFileName)
                        ? null
                        : Url.Content("~/Uploads/" + e.LogoFileName)
                })
                .ToList();
            return Ok(items);
        }

        [HttpGet, Route("{id:guid}")]
        public IHttpActionResult GetDeployment(Guid id)
        {
            var e = db.OrganizationEvents.FirstOrDefault(ev => ev.OrganizationEventId == id);
            if (e == null) return NotFound();
            var dto = new DeploymentDto
            {
                id = e.OrganizationEventId,
                title = e.CampaignName,
                location = e.StagingCity,
                organization = e.Organization.Name,
                startDate = e.BeginDate,
                endDate = e.EndDate,
                duration = e.BeginDate.HasValue && e.EndDate.HasValue
                    ? ((e.EndDate.Value - e.BeginDate.Value).Days + " days")
                    : null,
                teamMembers = e.UserOrganizationEvents.Count,
                slots = e.OrganizationEventPositions.Count,
                logoUrl = string.IsNullOrEmpty(e.LogoFileName)
                    ? null
                    : Url.Content("~/Uploads/" + e.LogoFileName)
            };
            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}