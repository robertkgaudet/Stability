<%@ WebHandler Language="C#" Class="CheckSignOutStatus" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Linq;
using Newtonsoft.Json;
using System.Web.Security;


public class CheckSignOutStatus : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        try
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string userId = Membership.GetUser().ProviderUserKey.ToString();

                using (var dc = new CrowdReliefDBDataContext())
                {
                    // Get the latest timesheet entry for the user where TimeOut is null
                    var latestTimesheet = (from t in dc.Timesheets
                                           where t.UserId == new Guid(userId) && t.TimeOut == null
                                           orderby t.TimeIn descending
                                           select t).Take(1).SingleOrDefault();

                    if (latestTimesheet != null)
                    {
                        DateTime timeInValue = latestTimesheet.TimeIn;
                        TimeSpan duration = DateTime.Now - timeInValue;

                        // Check if more than 1 minute has passed
                        bool shouldSignOut = duration.TotalMinutes > 1;

                        // Return JSON response
                        context.Response.Write(JsonConvert.SerializeObject(new { shouldSignOut }));
                    }
                    else
                    {
                        // No active timesheet found
                        context.Response.Write(JsonConvert.SerializeObject(new { shouldSignOut = false }));
                    }
                }
            }
            else
            {
                // User is not authenticated
                context.Response.Write(JsonConvert.SerializeObject(new { shouldSignOut = false }));
            }
        }
        catch (Exception ex)
        {
            // Handle errors and return a JSON response
            context.Response.Write(JsonConvert.SerializeObject(new { error = ex.Message }));
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}