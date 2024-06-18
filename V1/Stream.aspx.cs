using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

public partial class V1_Stream : BaseOrganizationWebForm
{
	public string recoveryStageId = ConfigurationManager.AppSettings["recoveryStageId"].ToString();
	public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
	public string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
	public string eventId = HttpContext.Current.Request.QueryString["eventId"];

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadPosts();

		bool testMode = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["brainTreeTestMode"].ToString());
		string testNonce = System.Configuration.ConfigurationManager.AppSettings["testNonce"].ToString();

		if (!this.IsPostBack)
		{

		}
		else
		{
		}
	}

	public void LoadPosts()
	{
		int streamPostPageSize = int.Parse(ConfigurationManager.AppSettings["streamPostPageSize"].ToString()) + 10;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var posts = (from rp in dc.RebuildPosts
					 join p in dc.Profiles on rp.UserId equals p.UserId
					 join r in dc.Rebuilds on rp.RebuildId equals r.RebuildId
					 join ev in dc.Events on r.EventId equals ev.EventId
					 orderby rp.CreatedOn descending
					 select new
					 {
						 rp.RebuildPostId,
						 p.ProfileId,
						 rp.Post,
						 rp.CreatedOn,
						 fullname = "<b>" + p.Firstname + " " + p.Lastname + "</b>",
						 disasterName = ev.Name,
						 r.RebuildId,
						 ev.EventId,
						 r.SurvivorId,
						 r.Difficulty,
						 disasterIcon = ev.Icon.Replace("COLOR", "btn-" + ev.Color + " btn-outline"),
						 ProfilePhoto = (p.Photo == null ? "Avatar.png" : p.Photo)
					 }).Skip(0).Take(streamPostPageSize);

		if (!String.IsNullOrEmpty(eventId))
		{
			posts = posts.Where(post => post.EventId == new Guid(eventId));
		}

		rptPosts.DataSource = posts;
		rptPosts.DataBind();
	}

	protected void rptPosts_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litOverallProgress = (Literal)e.Item.FindControl("litOverallProgress");
			Literal litRebuildProgress = (Literal)e.Item.FindControl("litRebuildProgress");
			Literal litRebuildStage = (Literal)e.Item.FindControl("litRebuildStage");
			HyperLink hypVolunteer = (HyperLink)e.Item.FindControl("hypVolunteer");
			Label lblMessageDate = (Label)e.Item.FindControl("lblMessageDate");
			Label lblVolunteersNeeded = (Label)e.Item.FindControl("lblVolunteersNeeded");

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			Guid survivorId = (Guid)DataBinder.Eval(dataItem.DataItem, "SurvivorId");
			Guid eventId = (Guid)DataBinder.Eval(dataItem.DataItem, "EventId");
			DateTime createdOn = (DateTime)DataBinder.Eval(dataItem.DataItem, "CreatedOn");
			int difficulty = 0;
			if (DataBinder.Eval(dataItem.DataItem, "Difficulty") != null)
			{
				difficulty = (int)DataBinder.Eval(dataItem.DataItem, "Difficulty");
			}

			string tickLabel = string.Empty;
			string progressPercent = GetPercent(false, Guid.Empty, survivorId, new Guid(rebuildProgressSliderId), out tickLabel).ToString();
			string overallPercent = GetPercent(false, Guid.Empty, survivorId, new Guid(overallProgressSliderId), out tickLabel).ToString();

			lblVolunteersNeeded.Text = CalculateVolunteersNeeded(survivorId, difficulty, false, Guid.Empty) + " Volunteers Needed - ";

			hypVolunteer.Text = "Volunteer";
			hypVolunteer.NavigateUrl = "~/V1/Profile/Volunteer.aspx?eventid=" + eventId;
			lblMessageDate.Text = GetElapsedTime(createdOn);

			string overallProgressHTML = "Overall: " + overallPercent + "% <div class=\"progress m-t-xs full progress-small\">" +
										"<div style=\"width:" + overallPercent + "%\" aria-valuemax=\"100\" aria-valuemin=\"0\" aria-valuenow=\"" + overallPercent + "\" role=\"progressbar\" class=\"progress-bar progress-bar-info\">" +
											"<span class=\"sr-only\">" + overallPercent + "% Complete(success)</span>" +
										"</div>" +
									"</div>";

			litOverallProgress.Text = overallProgressHTML;

			string progressHTML = "Rebuild: " + progressPercent + "% <div class=\"progress m-t-xs full progress-small\">" +
										"<div style=\"width:" + progressPercent + "%\" aria-valuemax=\"100\" aria-valuemin=\"0\" aria-valuenow=\"" + progressPercent + "\" role=\"progressbar\" class=\"progress-bar progress-bar-success\">" +
											"<span class=\"sr-only\">" + progressPercent + "% Complete(success)</span>" +
										"</div>" +
									"</div>";

			litRebuildProgress.Text = progressHTML;
			litRebuildStage.Text = GetLatestProgressTick(survivorId, new Guid(recoveryStageId)) + " Stage";
		}
	}

	protected void btn_Click(object sender, EventArgs e)
    {
//		this.Label1.Text = "b";
		//Button btn = (Button)sender;
		//RepeaterItem dataItem = (RepeaterItem)btn.Parent;
		//this.Label1.Text = dataItem.ItemType.ToString();
		//this.Label1.Text = ((Guid)DataBinder.Eval(dataItem.DataItem, "RebuildPostId")).ToString();
	}
}