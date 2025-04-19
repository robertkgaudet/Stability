using System;
using System.Linq;
using System.Web.Http;
using System.Web.Security;

namespace Stability
{
    [RoutePrefix("api/timesheets")]
    public class TimeSheetsController : ApiController
    {
        private readonly CrowdReliefDBDataContext db = new CrowdReliefDBDataContext();

        public class TimeEntryDto
        {
            public Guid id { get; set; }
            public string deploymentName { get; set; }
            public string deploymentType { get; set; }
            public string date { get; set; }
            public string timeSlot { get; set; }
            public string location { get; set; }
            public string timeIn { get; set; }
            public string timeOut { get; set; }
            public double duration { get; set; }
            public string comments { get; set; }
        }

        [HttpGet, Route("")]
        [Authorize]
        public IHttpActionResult Get()
        {
            var user = Membership.GetUser(User.Identity.Name);
            var uid = (Guid)user.ProviderUserKey;
            // Join Timesheet -> UserOrganizationEvent -> OrganizationEvent, and TaskType
            var list = (from t in db.Timesheets
                        where t.UserId == uid && !t.IsDeleted && t.UserOrganizationEventId.HasValue && t.TaskTypeId.HasValue
                        join ue in db.UserOrganizationEvents on t.UserOrganizationEventId.Value equals ue.UserOrganizationEventId
                        join ev in db.OrganizationEvents on ue.OrganizationEventId equals ev.OrganizationEventId
                        join tt in db.TaskTypes on t.TaskTypeId.Value equals tt.TaskTypeId
                        select new TimeEntryDto
                        {
                            id = t.TimesheetId,
                            deploymentName = ev.CampaignName,
                            deploymentType = tt.Name,
                            date = t.TimeIn.ToString("yyyy-MM-dd"),
                            timeSlot = t.TimeOut.HasValue
                                ? t.TimeIn.ToString("HH:mm") + " - " + t.TimeOut.Value.ToString("HH:mm")
                                : t.TimeIn.ToString("HH:mm"),
                            location = ev.StagingCity,
                            timeIn = t.TimeIn.ToString("o"),
                            timeOut = t.TimeOut.HasValue ? t.TimeOut.Value.ToString("o") : null,
                            duration = t.TimeOut.HasValue ? (t.TimeOut.Value - t.TimeIn).TotalHours : 0,
                            comments = t.WorkCompleted ?? t.Description
                        }).ToList();
            return Ok(list);
        }

        public class TimeEntryInput
        {
            public string deploymentName { get; set; }
            public string deploymentType { get; set; }
            public string date { get; set; }
            public string timeSlot { get; set; }
            public string location { get; set; }
            public string timeIn { get; set; }
            public string timeOut { get; set; }
            public double duration { get; set; }
            public string comments { get; set; }
        }

        [HttpPost, Route("")]
        [Authorize]
        public IHttpActionResult Post(TimeEntryInput model)
        {
            if (model == null) return BadRequest("Invalid input");
            var user = Membership.GetUser(User.Identity.Name);
            var uid = (Guid)user.ProviderUserKey;
            // Find TaskType
            var tt = db.TaskTypes.FirstOrDefault(x => x.Name == model.deploymentType);
            if (tt == null) return BadRequest("Invalid deploymentType");
            var entry = new Timesheet
            {
                TimesheetId = Guid.NewGuid(),
                UserId = uid,
                TaskTypeId = tt.TaskTypeId,
                TimeIn = DateTime.Parse(model.timeIn),
                TimeOut = DateTime.Parse(model.timeOut),
                Description = model.comments,
                WorkCompleted = model.comments,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };
            db.Timesheets.InsertOnSubmit(entry);
            db.SubmitChanges();
            var dto = new TimeEntryDto
            {
                id = entry.TimesheetId,
                deploymentName = model.deploymentName,
                deploymentType = model.deploymentType,
                date = model.date,
                timeSlot = model.timeSlot,
                location = model.location,
                timeIn = entry.TimeIn.ToString("o"),
                timeOut = entry.TimeOut.Value.ToString("o"),
                duration = model.duration,
                comments = model.comments
            };
            return Created(string.Empty, dto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}