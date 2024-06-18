<%@ WebHandler Language="C#" Class="GetStreamPost" %>

using System;
using System.Web;
using System.Linq;
using System.Web.Services;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Configuration;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GetStreamPost : IHttpHandler, IReadOnlySessionState
{
	public string recoveryStageId			= ConfigurationManager.AppSettings["recoveryStageId"].ToString();
	public string rebuildProgressSliderId	= ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
	public string overallProgressSliderId	= ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();

	public void ProcessRequest (HttpContext context)
	{
		string results                      = string.Empty;
		int pageNumber                      = int.Parse(context.Request.QueryString["pageNumber"]);
		int streamPostPageSize              = int.Parse(ConfigurationManager.AppSettings["streamPostPageSize"].ToString());
		string eventId						=  (string)context.Request.QueryString["eventId"];

		try
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var posts = (from rp in dc.RebuildPosts
						 join p in dc.Profiles on rp.UserId equals p.UserId
						 join r in dc.Rebuilds on rp.RebuildId equals r.RebuildId
						 join ev in dc.Events on r.EventId equals ev.EventId
						 orderby rp.CreatedOn descending
						 select new { p.ProfileId, rp.Post, rp.CreatedOn,
							 fullname = p.Firstname + " " + p.Lastname,
							 disasterName = ev.Name,
							 r.RebuildId,
							 ev.EventId,
							 r.SurvivorId,
							 r.Difficulty,
							 disasterIcon = ev.Icon.Replace("COLOR", "btn-" + ev.Color + " btn-outline"),
							 ProfilePhoto = (p.Photo == null ? "Avatar.png" : p.Photo) }).Skip(streamPostPageSize * pageNumber).Take(streamPostPageSize);
				
			if(!String.IsNullOrEmpty(eventId))
			{
				posts = posts.Where(post => post.EventId == new Guid(eventId));
			}
			

			foreach(var post in posts)
			{

				BaseOrganizationWebForm baseWebForm = new BaseOrganizationWebForm();
				string tickLabel = string.Empty;
				string progressPercent = baseWebForm.GetPercent(false, Guid.Empty, post.SurvivorId, new Guid(rebuildProgressSliderId), out tickLabel).ToString();
				string overallPercent = baseWebForm.GetPercent(false, Guid.Empty, post.SurvivorId, new Guid(overallProgressSliderId), out tickLabel).ToString();
				string volunteersNeeded = baseWebForm.CalculateVolunteersNeeded(post.SurvivorId, Convert.ToInt32(post.Difficulty), false, Guid.Empty) + " Volunteers Needed";
				string progressBars = string.Empty;
				
				string overallProgressHTML = "Overall: " + overallPercent + "% <div class=\"progress m-t-xs full progress-small\">" + Environment.NewLine +
											"<div style=\"width:" + overallPercent + "%\" aria-valuemax=\"100\" aria-valuemin=\"0\" aria-valuenow=\"" + overallPercent + "\" role=\"progressbar\" class=\"progress-bar progress-bar-info\">" + Environment.NewLine +
												"<span class=\"sr-only\">" + overallPercent + "% Complete(success)</span>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
										"</div>" + Environment.NewLine;

				string progressHTML = "Rebuild: " + progressPercent + "% <div class=\"progress m-t-xs full progress-small\">" + Environment.NewLine +
											"<div style=\"width:" + progressPercent + "%\" aria-valuemax=\"100\" aria-valuemin=\"0\" aria-valuenow=\"" + progressPercent + "\" role=\"progressbar\" class=\"progress-bar progress-bar-success\">" + Environment.NewLine +
												"<span class=\"sr-only\">" + progressPercent + "% Complete(success)</span>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
										"</div>" + Environment.NewLine;

				progressBars = "<div class=\"m\"><div class=\"row\"><div class=\"col-sm-4\"><h4>" +  baseWebForm.GetLatestProgressTick(post.SurvivorId, new Guid(recoveryStageId)) + " Stage</h4></div><div class=\"col-sm-4\">" + overallProgressHTML + "</div><div class=\"col-sm-4\">" + progressHTML + "</div></div></div>";

				results = results +  "<div class=\"hpanel post\">" + Environment.NewLine +
										"<div class=\"panel-body\">" +  Environment.NewLine +
											"<div class=\"message\">" +  Environment.NewLine +
												"<div class=\"blog-article-box\">" +  Environment.NewLine +
												"<h1 class=\"pull-left m-r-lg\">" + post.disasterIcon + "</h1>" +  Environment.NewLine +
												"<a href=\"/V1/Profile/Rebuild.aspx?rebuildId=" + post.RebuildId + "\" class=\"StreamLink\"><b>" +  post.fullname + "</b></a> posted an update for <a href=\"/V1/Event.aspx?eventId=" + post.EventId + "\" class=\"StreamLink\">" + post.disasterName + "</a>" +  Environment.NewLine +
												"</div>" + Environment.NewLine +
												"<span class=\"message-date\"> " + baseWebForm.GetElapsedTime(post.CreatedOn) + "</span>"  + Environment.NewLine +
												"<span class=\"message-content\">" +  Environment.NewLine +
												post.Post +  Environment.NewLine +
												"</span>" +  Environment.NewLine + progressBars +
											"</div>" + Environment.NewLine +
											"<div class=\"panel-footer\">"  + Environment.NewLine +
											"<span>" + volunteersNeeded + " - </span>" + Environment.NewLine +
											"<a href=\"Profile/Volunteer.aspx?eventid=" + post.EventId + "\" class=\"StreamLink\">Volunteer</a>" + Environment.NewLine +
											"</div>" + Environment.NewLine + 
										"</div>" +  Environment.NewLine +
									"</div>" + Environment.NewLine;
			}
		}
		catch(Exception ex)
		{
			results = "<div class=\"error\"><h2>ERROR in GetStreamPost.aspx</h2>error:" + ex.Message + "</div>";
		}

		context.Response.ContentType = "text/plain";
		context.Response.Write(results);
	}

	public bool IsReusable {
		get {
			return false;
		}
	}
}