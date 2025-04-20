using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;

namespace Stability
{
// Enable CORS for this controller
[EnableCors(origins: "http://localhost:19006", headers: "*", methods: "*", SupportsCredentials = true)]
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
            var query = from t in db.Timesheets
                        where t.UserId == uid && !t.IsDeleted && t.UserOrganizationEventId.HasValue && t.TaskTypeId.HasValue
                        join ue in db.UserOrganizationEvents on t.UserOrganizationEventId.Value equals ue.UserOrganizationEventId
                        join ev in db.OrganizationEvents on ue.OrganizationEventId equals ev.OrganizationEventId
                        join tt in db.TaskTypes on t.TaskTypeId.Value equals tt.TaskTypeId
                        select new 
                        {
                            TimesheetId = t.TimesheetId,
                            DeploymentName = ev.CampaignName,
                            DeploymentType = tt.Name,
                            TimeIn = t.TimeIn,
                            TimeOut = t.TimeOut,
                            Location = ev.StagingCity,
                            Description = t.Description,
                            WorkCompleted = t.WorkCompleted
                        };
                        
            // Execute the query first to get data from database
            var results = query.ToList();
            
            // Then format the data in memory
            var list = results.Select(r => new TimeEntryDto
            {
                id = r.TimesheetId,
                deploymentName = r.DeploymentName,
                deploymentType = r.DeploymentType,
                date = r.TimeIn.ToString("yyyy-MM-dd"),
                timeSlot = r.TimeOut.HasValue
                    ? r.TimeIn.ToString("HH:mm") + " - " + r.TimeOut.Value.ToString("HH:mm")
                    : r.TimeIn.ToString("HH:mm"),
                location = r.Location,
                timeIn = r.TimeIn.ToString("o"),
                timeOut = r.TimeOut.HasValue ? r.TimeOut.Value.ToString("o") : null,
                duration = r.TimeOut.HasValue ? (r.TimeOut.Value - r.TimeIn).TotalHours : 0,
                comments = r.WorkCompleted ?? r.Description
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
        
        [HttpGet, Route("task-types")]
        [Authorize]
        public IHttpActionResult GetTaskTypes()
        {
            // Return all task types from the database
            var taskTypes = db.TaskTypes.Select(tt => new { id = tt.TaskTypeId, name = tt.Name }).ToList();
            return Ok(taskTypes);
        }
    }
}