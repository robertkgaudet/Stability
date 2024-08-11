<%@ WebHandler Language="C#" Class="UpdateDefaultDisaster" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Linq;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class UpdateDefaultDisaster : IHttpHandler, IReadOnlySessionState
{
	public void ProcessRequest(HttpContext context)
	{
		string results = string.Empty;
		try
		{
			string userStringId = context.Request.QueryString["userId"];
			string eventId      = context.Request.QueryString["eventId"];
			
		    CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		    Guid userId = new Guid(userStringId);

		    var profile = (from p in dc.Profiles
					       where p.UserId == userId
					       select p).SingleOrDefault();

		    profile.DefaultEventId = new Guid(eventId);
		    dc.SubmitChanges();

		    var disaster = (from ev in dc.Events
						    where ev.EventId == new Guid(eventId)
						    select ev).SingleOrDefault();

		    //Makes sure the user is assocated with the response if not already.
		    var userCheck = from p in dc.UserEvents
						    where p.UserId == userId
						    && p.EventId == disaster.EventId
						    select p;

		    if (userCheck != null)
		    {
				UserEvent userEvent = new UserEvent();
				userEvent.EventId = disaster.EventId;
				userEvent.UserId = userId;
				userEvent.UserEventId = Guid.NewGuid();
				dc.UserEvents.InsertOnSubmit(userEvent);
				dc.SubmitChanges();
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