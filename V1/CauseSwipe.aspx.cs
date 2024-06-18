using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_CauseSwipe : System.Web.UI.Page
{
	public string causePhotoFolder = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var causes = from oe in dc.OrganizationEvents
					join ev in dc.Events on oe.EventId equals ev.EventId
					join o in dc.Organizations on oe.OrganizationId equals o.OrganizationId
					where oe.IsActive == true
					select new { oe.CampaignName, oe.DonationURL, oe.MissionPurpose, oe.OrganizationEventId, oe.OrganizationId, o.Name, oe.URLFriendlyCampaignName, oe.VolunteerInstructions };

		rptCauseSwipe.DataSource = causes;
		rptCauseSwipe.DataBind();
	}
	public static string ShortenStringKeepWholeWords(string input, int maxLength, string ellipsis = "...")
	{
		if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
			return input;

		int lastSpaceBeforeMax = input.LastIndexOf(' ', maxLength);

		if (lastSpaceBeforeMax > 0)
		{
			return input.Substring(0, lastSpaceBeforeMax) + ellipsis;
		}

		return input;  // If no space was found, returning the original string.
	}


	protected void rptCauseSwipe_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litCauseTitle = (Literal)e.Item.FindControl("litCauseTitle"); 
			Literal litVolunteerInstructions = (Literal)e.Item.FindControl("litVolunteerInstructions");
			Literal litDescription = (Literal)e.Item.FindControl("litDescription");
			var divCause = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("divCause");

			String causeName = (string)DataBinder.Eval(dataItem.DataItem, "CampaignName");
			String missionPurpose = (string)DataBinder.Eval(dataItem.DataItem, "MissionPurpose"); 
			String volunteerInstructions = (string)DataBinder.Eval(dataItem.DataItem, "VolunteerInstructions"); 
			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");


			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var organizationEventPhoto = (from oep in dc.OrganizationEventPhotos
										  join p in dc.Photos on oep.PhotoId equals p.PhotoId
										  where oep.OrganizationEventId == organizationEventId
										  orderby p.CreatedOn descending
										  select new { p.Filename }).Take(1).SingleOrDefault();

			if (organizationEventPhoto != null)
			{
				divCause.Attributes.Add("style", "display:block; background-image:url(\"" + causePhotoFolder + organizationEventPhoto.Filename + "\")");
			}

			litCauseTitle.Text = causeName;
			//litVolunteerInstructions.Text = volunteerInstructions;
			int maxLength = 400;
			litDescription.Text = ShortenStringKeepWholeWords(missionPurpose, maxLength);


			//CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		}
	}
}