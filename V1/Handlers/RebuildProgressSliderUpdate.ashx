<%@ WebHandler Language="C#" Class="RebuildProgressSliderUpdate" %>

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
public class RebuildProgressSliderUpdate : IHttpHandler, IReadOnlySessionState
{
	public void ProcessRequest (HttpContext context)
	{
		string results      = string.Empty;
		string tickValue    = (string)context.Request.QueryString["tickValue"];
		string sliderId     = (string)context.Request.QueryString["sliderId"];
		string rebuildId     = (string)context.Request.QueryString["rebuildId"];
		string survivorId   = (string)context.Request.QueryString["survivorId"];
		string userId       = (string)context.Request.QueryString["userId"];

		try
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			Guid sliderTickId = (from g in dc.SliderTicks
								 where g.Tick == int.Parse(tickValue)
								 &&
								 g.SliderId == Guid.Parse(sliderId)
								 &&
								 g.IsActive == true
								 select g.SliderTickId).SingleOrDefault();

			UserSliderTick userSliderTick = new UserSliderTick();
			
			userSliderTick.CreatedOn = DateTime.Now;
			userSliderTick.SliderTickId = sliderTickId;
			userSliderTick.UserId = new Guid(userId);
			userSliderTick.SurvivorId = new Guid(survivorId);
			userSliderTick.UserSliderTickId = Guid.NewGuid();
			userSliderTick.SliderId = new Guid(sliderId);
			userSliderTick.RebuildId = new Guid(rebuildId);
			dc.UserSliderTicks.InsertOnSubmit(userSliderTick);
			dc.SubmitChanges();
			
			try
			{
				//make sure nothing breaks.
				var rebuildUser = (from r in dc.Rebuilds
								   join p in dc.Profiles on r.SurvivorId equals p.UserId
								   where r.CreatedBy == new Guid(userId)// && or.IsPrimaryOrganization == true
								   select new { r.RebuildId, p.Firstname, p.Lastname }).Take(1).SingleOrDefault();

				var slider = (from s in dc.Sliders
							  where s.SliderId == new Guid(sliderId)
							  select new { s.Type }).SingleOrDefault();

				if(rebuildUser != null)
				{
					UpdatePost(rebuildUser.Firstname + "'s <b>" + slider.Type + "</b> progress just updated for their home which is being rebuilt.", new Guid(userId), new Guid(rebuildId));
				}
			}
			catch(Exception ex)
			{}
			results = "success";
		}
		catch(Exception ex)
		{
			results = "error:" + ex.Message;
		}

		context.Response.ContentType = "text/plain";
		context.Response.Write(results);
	}

	protected void UpdatePost(string post, Guid userId, Guid rebuildId)
	{
		if (!string.IsNullOrEmpty(post))
		{
			RebuildPost rebuildPost = new RebuildPost();
			rebuildPost.CreatedOn = DateTime.Now;
			rebuildPost.IsVisible = true;
			rebuildPost.Post = post;
			rebuildPost.RebuildPostId = Guid.NewGuid();
			rebuildPost.UserId = userId;
			rebuildPost.RebuildId = rebuildId;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			dc.RebuildPosts.InsertOnSubmit(rebuildPost);
			dc.SubmitChanges();
		}
	}

	public bool IsReusable {
		get {
			return false;
		}
	}
}