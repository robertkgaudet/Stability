<%@ WebHandler Language="C#" Class="UpdateTimesheet" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Linq;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class UpdateTimesheet : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string results = string.Empty;
        try
        {
            string userId = context.Request.QueryString["userId"];
            string userOrganizationEventId = context.Request.QueryString["userOrganizationEventId"];
            string organizationEventId = context.Request.QueryString["organizationEventId"];
            string workDescription = context.Request.QueryString["workDescription"];
            string taskTypeId = context.Request.QueryString["taskTypeId"];
            bool isUserActive = Convert.ToBoolean(context.Request.QueryString["isUserActive"]);
            bool noCause = Convert.ToBoolean(context.Request.QueryString["noCause"]);

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            if (noCause)
            {
            
                userOrganizationEventId = Guid.NewGuid().ToString();
                //Update their se.cau
                UserOrganizationEvent userOrganizationEvent = new UserOrganizationEvent();
                userOrganizationEvent.UserOrganizationEventId = new Guid(userOrganizationEventId);
                userOrganizationEvent.UserId = new Guid(userId);
                userOrganizationEvent.OrganizationEventId = new Guid(organizationEventId);
                userOrganizationEvent.CreatedOn = DateTime.Now;
                dc.UserOrganizationEvents.InsertOnSubmit(userOrganizationEvent);
                dc.SubmitChanges();
            }
            else
            {
                //has a cause already
                if (!String.IsNullOrEmpty(organizationEventId))
                {
                    //See if current cause matches one sent in.
                    //User had a cause, if it does not match the organizationEventId sent in, then update the one sent in and set the new one.
                    var userOrganizationEvent = (from uoe in dc.UserOrganizationEvents
                                                 where uoe.UserId == new Guid(userId)
                                                 && uoe.OrganizationEventId == new Guid(organizationEventId)
                                                 orderby uoe.CreatedOn descending
                                                 select uoe).Take(1).SingleOrDefault();

                    if (userOrganizationEvent != null)
                    {
                        //User had this cause already
                        //If it's still active do nothing, if it's not then reactivate it.
                        if (userOrganizationEvent.DeactivatedOn != null)
                        {
                            userOrganizationEvent.DeactivatedOn = null;
                            dc.SubmitChanges();
                        }
                        else
                        {
                            //Do nothing
                        }
                    }
                    else
                    {
                        //User didn't have this cause yet.
                        userOrganizationEventId = Guid.NewGuid().ToString();
                        //Update their cause.
                        UserOrganizationEvent userOrganizationEventNew = new UserOrganizationEvent();
                        userOrganizationEventNew.UserOrganizationEventId = new Guid(userOrganizationEventId);
                        userOrganizationEventNew.UserId = new Guid(userId);
                        userOrganizationEventNew.OrganizationEventId = new Guid(organizationEventId);
                        userOrganizationEventNew.CreatedOn = DateTime.Now;
                        dc.UserOrganizationEvents.InsertOnSubmit(userOrganizationEventNew);
                        dc.SubmitChanges();
                    }
                }
                
            }

            Guid timesheetIdGuid = Guid.NewGuid();

            if (isUserActive)
            {
                Timesheet timesheet = new Timesheet();
                timesheet.TimesheetId = timesheetIdGuid;
                timesheet.TaskTypeId = new Guid(taskTypeId);
                timesheet.TimeIn = DateTime.Now;
                timesheet.UserId = new Guid(userId);
                timesheet.UserOrganizationEventId = new Guid(userOrganizationEventId);
                timesheet.Description = workDescription;
                timesheet.IsDeleted = false;
                timesheet.Error = false;
                dc.Timesheets.InsertOnSubmit(timesheet);
                dc.SubmitChanges();
                results = "Tracking your time.";
            }
            else
            {
                //Update their most recent time out with now.
                var timesheet = (from t in dc.Timesheets
                                    where t.UserId == new Guid(userId)
                                    orderby t.TimeIn descending
                                    select t).Take(1).SingleOrDefault();

                if (timesheet != null)
                {
                    timesheet.TimeOut = DateTime.Now;
                    timesheet.WorkCompleted = workDescription;
                    dc.SubmitChanges();
                    results = "Tracking time ended.";
                }
            }
        }
        catch (Exception ex)
        {
            results += results + ex.Data + "<br> " + ex.HelpLink + "<br> " + ex.HResult + "<br> " + ex.InnerException + "<br> " + ex.Message + "<br> " + ex.Source + "<br>" + ex.StackTrace + "<br>" + ex.TargetSite;
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(results);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}