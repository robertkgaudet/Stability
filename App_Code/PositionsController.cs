using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Stability
{
// Enable CORS for this controller
[EnableCors(origins: "*", headers: "*", methods: "*")]
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
        }

        [HttpGet, Route("{id:guid}/positions")]
        public IHttpActionResult GetPositions(Guid id)
        {
            var list = db.OrganizationEventPositions
                .Where(p => p.OrganizationEventId == id && !p.IsDeleted)
                .AsEnumerable()
                .Select(p => new PositionDto
                {
                    id = p.OrganizationEventPositionId,
                    date = p.DeploymentDate.HasValue ? p.DeploymentDate.Value.ToString("yyyy-MM-dd") : null,
                    role = p.Position.Name,
                    time = (p.ArrivalTime.HasValue && p.DepartureTime.HasValue)
                        ? p.ArrivalTime.Value.ToString("hh\\:mm") + " - " + p.DepartureTime.Value.ToString("hh\\:mm")
                        : null
                })
                .ToList();
            return Ok(list);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}