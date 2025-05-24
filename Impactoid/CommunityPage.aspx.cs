using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Braintree;
using System.IdentityModel.Metadata;

public partial class Impactoid_CommunityPage : System.Web.UI.Page
{
	public string organizationId = string.Empty;
	public string organizationName = string.Empty;
	public string publicEmailAddress = string.Empty;
	public string publicPhoneNumber = string.Empty;
	public string facebookURL = string.Empty;
	public string facebookGroupURL = string.Empty;
	public string twitterURL = string.Empty;
	public string instagramURL = string.Empty;
	public string youtubeURL = string.Empty;
	public string pageTitle = string.Empty;
	public string pageDescription = string.Empty;
	public string causePhotoFolder = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		if (String.IsNullOrEmpty(Request.QueryString["organizationId"]) && String.IsNullOrEmpty(Request.QueryString["organizationName"]))
		{
			Response.Write("No organization Id provided.");
			Response.End();
		}

		organizationId = Request.QueryString["organizationId"];
		organizationName = Request.QueryString["organizationName"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		Organization organization;
		if (String.IsNullOrEmpty(organizationId))
		{
			organization = (from o in dc.Organizations
								where o.URLFriendlyName == organizationName
								select o).SingleOrDefault();
		}
		else
		{
			organization = (from o in dc.Organizations
								where o.OrganizationId == new Guid(organizationId)
								select o).SingleOrDefault();
		}

		if (organization != null)
		{
			this.Master.PageTitle = organization.Name;
			this.Master.FbSite_name = organization.Name;
			this.Master.PageDescription = organization.Description;
			this.Master.FbDescription = organization.Description;

			imgCover.ImageUrl = "aid/images/slider/slide3.jpg";
			if (!String.IsNullOrEmpty(organization.CoverImage))
			{
				imgCover.ImageUrl = causePhotoFolder + organization.CoverImage;
			}

			organizationId = organization.OrganizationId.ToString();
			publicEmailAddress = organization.PublicEmail;
			publicPhoneNumber = organization.PublicPhoneNumber;
			facebookURL = organization.FacebookURL;
			facebookGroupURL = organization.FacebookGroupURL;
			instagramURL = organization.InstagramURL;
			twitterURL = organization.TwitterURL;
			youtubeURL = organization.YouTubeURL;
			pageTitle = organization.Name;
			pageDescription = organization.Description;
			litDescription.Text = organization.Description;
			litOrgName.Text = organization.Name;
			litMission.Text = organization.PurposeMission;
			litHistory.Text = organization.History;
			litAboutUs.Text = organization.AFewWordsAboutUs;
			litProgramOverview.Text = organization.ProgramsOverview;
		}

		var programs = from p in dc.Programs
					join op in dc.OrganizationPrograms on p.ProgramId equals op.ProgramId
					where op.OrganizationId == new Guid(organizationId)
					orderby p.Order
					select p;

		rptPrograms.DataSource = programs;
		rptPrograms.DataBind();

        var causes = (from oe in dc.OrganizationEvents
                     join ev in dc.Events on oe.EventId equals ev.EventId
                     join dca in dc.DonationCampaigns on oe.OrganizationEventId equals dca.OrganizationEventId into dcaGroup
                     from dca in dcaGroup.DefaultIfEmpty() 
                     where oe.OrganizationId == new Guid(organizationId)
                     && oe.IsActive == true
                     orderby ev.BeginDate descending
                     select new
                     {
                         oe.OrganizationEventId,
                         DisasterDescription = ev.Description,
                         disasterDate = ev.BeginDate,
                         DonationURL = dca != null
                             ? "/V1/NonProfit/DonationDetails.aspx?organizationId=" + organizationId + "&donationCampaignId=" + dca.DonationCampaignId
                             : null,
                         oe.VolunteerURL,
                         oe.IsActive,
                         oe.URLFriendlyCampaignName,
                         ev.URLFriendlyName,
                         oe.MissionPurpose,
                         campaignName = oe.CampaignName,
                         disasterName = ev.Name,
                         DonationCampaignId = dca != null ? dca.DonationCampaignId : (Guid?)null 
                     }).Take(4);


        if (causes.Count() == 0)
        {
            testimonial.Visible = false;
        }

        rptGalleryMenu.DataSource = causes;
        rptGalleryMenu.DataBind();

        rptCauseSlider.DataSource = causes;
        rptCauseSlider.DataBind();

        rptCauses.DataSource = causes.Take(4);
        rptCauses.DataBind();

        rptDisasters.DataSource = causes.Distinct();
		rptDisasters.DataBind();

		var photos = from oep in dc.OrganizationEventPhotos
					 join oe in dc.OrganizationEvents on oep.OrganizationEventId equals oe.OrganizationEventId
					 join p in dc.Photos on oep.PhotoId equals p.PhotoId
					 where oe.OrganizationId.Equals(organizationId) && oep.IsCover == false
					 select new {oe.CampaignName, p.Filename};

		if (photos.Count() > 0)
		{
			rptGalleryGrid.DataSource = photos;
			rptGalleryGrid.DataBind();
		}


		var organizations = from o in dc.Organizations
							where o.IsActive == true && o.IsWebsiteActive == true
							select new { o.Name, o.OrganizationId };

		rptTicker.DataSource = organizations;
		rptTicker.DataBind();

		var team = from p in dc.Profiles
				   join uo in dc.UserOrganizations on p.UserId equals uo.UserId
				   where uo.OrganizationId == new Guid(organizationId)
				   &&
				   p.ShowOnWebsite == true && uo.IsEnabled == true
                   select new { fullname = p.Firstname + " " + p.Lastname, p.Title, p.UserId };


		if(team.Count() > 0)
		{
			rptTeam.DataSource = team;
			rptTeam.DataBind();
		}
	}

	protected void rptTeam_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litTitle = (Literal)e.Item.FindControl("litTitle");
			Literal litName = (Literal)e.Item.FindControl("litName");
			Image imgTeamMember = (Image)e.Item.FindControl("imgTeamMember");

			string fullname = (string)DataBinder.Eval(dataItem.DataItem, "fullname");
			string title = (string)DataBinder.Eval(dataItem.DataItem, "Title");
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			string photoFilename = string.Empty;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			//Get the photo name
			var profileImage = (from ph in dc.ProfilePhotos
								join p in dc.Photos on ph.PhotoId equals p.PhotoId
								where ph.UserId == userId && ph.IsCurrrent == true
								orderby p.CreatedOn descending
								select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			if (profileImage != null)
			{
				//Get the users profile image
				photoFilename = profilePhotoFolder + profileImage.FilenameCropped;
			}
			else
			{
				photoFilename = "/V1/Images/icons8-customer-64.png";
			}

			imgTeamMember.ImageUrl = photoFilename;

			litTitle.Text = title;
			litName.Text = fullname;	
		}
	}
	protected void rptGalleryGrid_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litGalleryGridCategory = (Literal)e.Item.FindControl("litGalleryGridCategory");
			Image imgGalleryImage = (Image)e.Item.FindControl("imgGalleryImage");
			HyperLink hypGalleryModal = (HyperLink)e.Item.FindControl("hypGalleryModal"); 

			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "campaignName");
			string filename = (string)DataBinder.Eval(dataItem.DataItem, "filename");
			litGalleryGridCategory.Text = "<div class=\"single-gallery " + campaignName.Replace(" ", "") + "\">";
			imgGalleryImage.ImageUrl = causePhotoFolder + filename;

			hypGalleryModal.NavigateUrl = causePhotoFolder + filename;
			hypGalleryModal.Text = "<span><i class=\"fa fa-eye\"></i></span>"; 
		}
	}
	protected void rptGalleryMenu_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			HyperLink hypGalleryMenu = (HyperLink)e.Item.FindControl("hypGalleryMenu");
			Literal litIdControl = (Literal)e.Item.FindControl("litIdControl");
			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "campaignName");
			litIdControl.Text = "<li data-filter=\"." + campaignName.Replace(" ", "") + "\" runat=\"server\" id=\"liControl\">";
			hypGalleryMenu.Text = campaignName;
		}
	}
	protected void rptTicker_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			HyperLink hypOrganizationName = (HyperLink)e.Item.FindControl("hypOrganizationName");
			string organizationName = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId");
			hypOrganizationName.Text = organizationName;
			hypOrganizationName.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + organizationId.ToString();
		}
	}

	protected void rptPrograms_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litProgramName = (Literal)e.Item.FindControl("litProgramName");
			Literal litProgramDescription = (Literal)e.Item.FindControl("litProgramDescription");

			Literal litRemote = (Literal)e.Item.FindControl("litRemote");
			Literal litTraining = (Literal)e.Item.FindControl("litTraining");
			Literal litDeployment = (Literal)e.Item.FindControl("litDeployment");

			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			string missionPurpose = (string)DataBinder.Eval(dataItem.DataItem, "Description");
			bool isRemoteOnly = (bool)DataBinder.Eval(dataItem.DataItem, "IsRemoteOnly");
			bool isDeploymentRequired = (bool)DataBinder.Eval(dataItem.DataItem, "IsDeploymentRequired");
			bool isTrainingRequired = (bool)DataBinder.Eval(dataItem.DataItem, "IsTrainingRequired");
			litProgramDescription.Text = missionPurpose;
			litProgramName.Text = campaignName;

			litRemote.Text = isRemoteOnly ? "Remote Allowed" : "No Remote Allowed";
			litTraining.Text = isTrainingRequired ? "Training Required" : "No Pre-training Needed";
			litDeployment.Text = isDeploymentRequired ? "Deployment Required" : "Optional Deployment";
		}
	}

	protected void rptCauses_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litCauseName = (Literal)e.Item.FindControl("litCauseName");
			Literal litCauseDescription = (Literal)e.Item.FindControl("litCauseDescription");
			Image imgSlider = (Image)e.Item.FindControl("imgSlider");
			HyperLink hypDonate = (HyperLink)e.Item.FindControl("hypDonate");
			HyperLink hypVolunteer = (HyperLink)e.Item.FindControl("hypVolunteer");

			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");
			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "CampaignName");
			string missionPurpose = (string)DataBinder.Eval(dataItem.DataItem, "MissionPurpose");
			litCauseDescription.Text = missionPurpose;
			litCauseName.Text = campaignName;
			string donationURL = (string)DataBinder.Eval(dataItem.DataItem, "DonationURL");
			string volunteerURL = (string)DataBinder.Eval(dataItem.DataItem, "VolunteerURL");

			hypDonate.Visible = false;
			if (!String.IsNullOrEmpty(donationURL))
			{
				hypDonate.NavigateUrl = donationURL;
				hypDonate.Visible = true;
			}
			hypVolunteer.Visible = false;
			if (!String.IsNullOrEmpty(donationURL))
			{
				hypVolunteer.NavigateUrl = volunteerURL;
				hypVolunteer.Visible = true;
			}

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var organizationEventPhoto = (from oep in dc.OrganizationEventPhotos
										  join p in dc.Photos on oep.PhotoId equals p.PhotoId
										  where oep.OrganizationEventId == organizationEventId
										  orderby p.CreatedOn descending
										  select new { p.Filename }).Take(1).SingleOrDefault();

			if (organizationEventPhoto != null)
			{
				imgSlider.ImageUrl = causePhotoFolder + organizationEventPhoto.Filename;
			}

		}
	}

	protected void rptDisasters_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litDisasterName = (Literal)e.Item.FindControl("litDisasterName");
			Literal litDisasterDate = (Literal)e.Item.FindControl("litDisasterDate");
			Literal litDisasterDescription = (Literal)e.Item.FindControl("litDisasterDescription");

			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");

			string disasterName = (string)DataBinder.Eval(dataItem.DataItem, "DisasterName");
			string disasterDescription = (string)DataBinder.Eval(dataItem.DataItem, "DisasterDescription");
			DateTime disasterDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "DisasterDate");

			litDisasterName.Text = disasterName;
			litDisasterDate.Text = disasterDate.ToLongDateString();
			litDisasterDescription.Text = disasterDescription;
		}
	}

	protected void rptCauseSlider_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litCauseName = (Literal)e.Item.FindControl("litCauseName");
			Literal litCauseDescription = (Literal)e.Item.FindControl("litCauseDescription");
			Image imgSlider = (Image)e.Item.FindControl("imgSlider");
			HyperLink hypDonate = (HyperLink)e.Item.FindControl("hypDonate");
			HyperLink hypVolunteer = (HyperLink)e.Item.FindControl("hypVolunteer");

			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");
			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "CampaignName");
			string missionPurpose = (string)DataBinder.Eval(dataItem.DataItem, "MissionPurpose");
			string donationURL = (string)DataBinder.Eval(dataItem.DataItem, "DonationURL");
			string volunteerURL = (string)DataBinder.Eval(dataItem.DataItem, "VolunteerURL");
			litCauseDescription.Text = missionPurpose;
			litCauseName.Text = campaignName;

			hypDonate.Visible = false;
			if (!String.IsNullOrEmpty(donationURL))
			{
				hypDonate.NavigateUrl = donationURL;
				hypDonate.Visible = true;
			}
			hypVolunteer.Visible = false;
			if (!String.IsNullOrEmpty(donationURL))
			{
				hypVolunteer.NavigateUrl = volunteerURL;
				hypVolunteer.Visible = true;
			}

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var organizationEventPhoto = (from oep in dc.OrganizationEventPhotos
										  join p in dc.Photos on oep.PhotoId equals p.PhotoId
										 where oep.OrganizationEventId == organizationEventId
										 orderby p.CreatedOn descending
										 select new { p.Filename }).Take(1).SingleOrDefault();

			if (organizationEventPhoto != null)
			{
				imgSlider.ImageUrl = causePhotoFolder + organizationEventPhoto.Filename;
			}

		}
	}
}