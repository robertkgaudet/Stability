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
            public int totalHours { get; set; }
        }

        [HttpGet, Route("")]
        [Authorize]
        public IHttpActionResult Get()
        {
            var user = Membership.GetUser(User.Identity.Name);
            var uid = (Guid)user.ProviderUserKey;
            
            // Using the same query logic as in LoadTimeSheet() method
            var timesheet = from t in db.Timesheets
                           join tt in db.TaskTypes on t.TaskTypeId equals tt.TaskTypeId into timeJoin
                           from time in timeJoin.DefaultIfEmpty()
                           where t.UserId == uid && !t.IsDeleted
                           orderby t.TimeIn descending
                           select new 
                           {
                               t.TimesheetId,
                               t.TimeIn, 
                               t.TimeOut, 
                               t.Description, 
                               taskname = time.Name, 
                               t.UserId,
                               t.WorkCompleted
                           };
            
            // Execute the query to get data from database
            var results = timesheet.ToList();
            
            // Format the data in memory
            var list = results.Select(r => {
                // Calculate hours similar to dlTimesheet_ItemDataBound method
                int timeSpanHoursInt = 0;
                int timeSpanMinutesInt = 0;
                
                if (r.TimeOut.HasValue)
                {
                    TimeSpan? span = (r.TimeOut.Value - r.TimeIn);
                    timeSpanHoursInt = span.Value.Hours;
                    timeSpanMinutesInt = span.Value.Minutes;
                    
                    if(timeSpanMinutesInt > 30)
                    {
                        timeSpanHoursInt = timeSpanHoursInt + 1;
                    }
                }
                
                return new TimeEntryDto
                {
                    id = r.TimesheetId,
                    deploymentType = r.taskname,
                    date = r.TimeIn.ToString("yyyy-MM-dd"),
                    timeSlot = r.TimeOut.HasValue
                        ? r.TimeIn.ToString("HH:mm") + " - " + r.TimeOut.Value.ToString("HH:mm")
                        : r.TimeIn.ToString("HH:mm"),
                    timeIn = r.TimeIn.ToString("o"),
                    timeOut = r.TimeOut.HasValue ? r.TimeOut.Value.ToString("o") : null,
                    duration = r.TimeOut.HasValue ? (r.TimeOut.Value - r.TimeIn).TotalHours : 0,
                    comments = r.WorkCompleted ?? r.Description,
                    totalHours = timeSpanHoursInt
                };
            }).ToList();
            
            return Ok(list);
        }

        [HttpGet, Route("all")]
        [Authorize]
        public IHttpActionResult GetAll()
        {
            // This mimics the functionality when Request["all"] is not empty in LoadTimeSheet()
            
            // Using the same query logic as in LoadTimeSheet() method but for all users
            var timesheet = from t in db.Timesheets
                           join tt in db.TaskTypes on t.TaskTypeId equals tt.TaskTypeId into timeJoin
                           from time in timeJoin.DefaultIfEmpty()
                           where !t.IsDeleted
                           orderby t.TimeIn descending
                           select new 
                           {
                               t.TimesheetId,
                               t.TimeIn, 
                               t.TimeOut, 
                               t.Description, 
                               taskname = time.Name, 
                               t.UserId,
                               t.WorkCompleted
                           };
            
            // Execute the query to get data from database
            var results = timesheet.ToList();
            
            // Format the data in memory
            var list = results.Select(r => {
                // Calculate hours similar to dlTimesheet_ItemDataBound method
                int timeSpanHoursInt = 0;
                int timeSpanMinutesInt = 0;
                
                if (r.TimeOut.HasValue)
                {
                    TimeSpan? span = (r.TimeOut.Value - r.TimeIn);
                    timeSpanHoursInt = span.Value.Hours;
                    timeSpanMinutesInt = span.Value.Minutes;
                    
                    if(timeSpanMinutesInt > 30)
                    {
                        timeSpanHoursInt = timeSpanHoursInt + 1;
                    }
                }
                
                return new TimeEntryDto
                {
                    id = r.TimesheetId,
                    deploymentType = r.taskname,
                    date = r.TimeIn.ToString("yyyy-MM-dd"),
                    timeSlot = r.TimeOut.HasValue
                        ? r.TimeIn.ToString("HH:mm") + " - " + r.TimeOut.Value.ToString("HH:mm")
                        : r.TimeIn.ToString("HH:mm"),
                    timeIn = r.TimeIn.ToString("o"),
                    timeOut = r.TimeOut.HasValue ? r.TimeOut.Value.ToString("o") : null,
                    duration = r.TimeOut.HasValue ? (r.TimeOut.Value - r.TimeIn).TotalHours : 0,
                    comments = r.WorkCompleted ?? r.Description,
                    totalHours = timeSpanHoursInt
                };
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
            
            DateTime timeIn = DateTime.Parse(model.timeIn);
            DateTime timeOut = DateTime.Parse(model.timeOut);
            
            var entry = new Timesheet
            {
                TimesheetId = Guid.NewGuid(),
                UserId = uid,
                TaskTypeId = tt.TaskTypeId,
                TimeIn = timeIn,
                TimeOut = timeOut,
                Description = model.comments,
                WorkCompleted = null,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };
            
            db.Timesheets.InsertOnSubmit(entry);
            db.SubmitChanges();
            
            // Calculate hours in the same way as Get/GetAll methods
            int timeSpanHoursInt = 0;
            int timeSpanMinutesInt = 0;
            
            TimeSpan span = (timeOut - timeIn);
            timeSpanHoursInt = span.Hours;
            timeSpanMinutesInt = span.Minutes;
            
            if(timeSpanMinutesInt > 30)
            {
                timeSpanHoursInt = timeSpanHoursInt + 1;
            }
            
            var dto = new TimeEntryDto
            {
                id = entry.TimesheetId,
                deploymentType = tt.Name,
                date = timeIn.ToString("yyyy-MM-dd"),
                timeSlot = timeIn.ToString("HH:mm") + " - " + timeOut.ToString("HH:mm"),
                location = model.location,
                timeIn = timeIn.ToString("o"),
                timeOut = timeOut.ToString("o"),
                duration = span.TotalHours,
                comments = model.comments,
                totalHours = timeSpanHoursInt,
                deploymentName = model.deploymentName
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