using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Web.Security;

public partial class Administration_Rebuilds : BaseOrganizationWebForm
{
	public string rebuildProgressPercent = string.Empty;
	public string overallProgressPercent = string.Empty;
	public string totalVolunteersNeeded = string.Empty;
	Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

	public string rebuildTickLabel = string.Empty;
	public string overallTickLabel = string.Empty;
	
	public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
	public string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
	public string recoveryStageId = ConfigurationManager.AppSettings["recoveryStageId"].ToString();

	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var rebuilds = from r in dc.Rebuilds
					   join a in dc.Addresses on r.RebuildAddressId equals a.AddressId
					   join p in dc.Profiles on r.SurvivorId equals p.UserId
					   where r.IsHidden == false
					   orderby r.CreatedOn descending
					   select new { r.RebuildId, r.RebuildStartDate, a.Address1, a.City, p.Firstname, p.Lastname, p.PhoneNumber, p.Age, p.UserId, r.Difficulty, OnHoldClass = (r.IsOnHold == false ? "enableRebuildNoGoRed" : "enableRebuildActiveGreen"), OnHold = (r.IsOnHold == false ? "Hold" : "Clear") };

		dlRebuildTable.DataSource = rebuilds.Distinct();
		dlRebuildTable.DataBind();

		rebuildProgressPercent = GetPercent(false, Guid.Empty, Guid.Empty, new Guid(rebuildProgressSliderId), out rebuildTickLabel).ToString();
		overallProgressPercent = GetPercent(false, Guid.Empty, Guid.Empty, new Guid(overallProgressSliderId), out overallTickLabel).ToString();

		litVolunteers.Text = CalculateVolunteersNeeded(userId, 0, true, Guid.Empty);
	}

	protected void dlRebuildTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litProgress = (Literal)e.Item.FindControl("litProgress"); 
			Literal litOverallProgress = (Literal)e.Item.FindControl("litOverallProgress");
			Literal litTickLabel = (Literal)e.Item.FindControl("litTickLabel"); 
			Literal litVolunteersNeeded = (Literal)e.Item.FindControl("litVolunteersNeeded"); 
			HyperLink hypPhone = (HyperLink)e.Item.FindControl("hypPhone");
			Literal litRebuildStage = (Literal)e.Item.FindControl("litRebuildStage"); 

			Guid survivorId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			int? difficulty = (int?)DataBinder.Eval(dataItem.DataItem, "Difficulty");
			String phonenumber = (String)DataBinder.Eval(dataItem.DataItem, "PhoneNumber");
			string tickLabel = string.Empty;

			string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
			string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
			string progressPercent = GetPercent(false, Guid.Empty, survivorId, new Guid(rebuildProgressSliderId), out tickLabel).ToString();
			string overallPercent = GetPercent(false, Guid.Empty, survivorId, new Guid(overallProgressSliderId), out tickLabel).ToString();
			
			litRebuildStage.Text = GetLatestProgressTick(survivorId, new Guid(recoveryStageId));

			//litTickLabel.Text = tickLabel;
			if(difficulty != null)
			{
				litVolunteersNeeded.Text = CalculateVolunteersNeeded(userId, int.Parse(difficulty.ToString()), false, Guid.Empty);
			}
			
			if(!String.IsNullOrEmpty(phonenumber))
			{
				hypPhone.NavigateUrl	= "tel:" + phonenumber;
				hypPhone.Text			= Regex.Replace(phonenumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypPhone.Font.Underline = true;
			}
			
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

			litProgress.Text = progressHTML;
		}
	}
}