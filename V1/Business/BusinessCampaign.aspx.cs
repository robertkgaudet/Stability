using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Business_BusinessCampaign : BaseOrganizationWebForm
{
	public string businessPhotoFolder = System.Configuration.ConfigurationManager.AppSettings["businessPhotoFolder"].ToString();
	public string donateLink = string.Empty;
	public string volunteerLink = string.Empty;
	public string icon = string.Empty;
	public string editCampaignLink = string.Empty;
	public string uploadLogoLink = string.Empty;
	string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();

	protected void Page_Load(object sender, EventArgs e)
	{
        if (String.IsNullOrEmpty(Request.QueryString["businessEventFriendlyURLName"]))
		{
			Response.Write("No business Id provided.");
			Response.End();
        }

		string businessEventFriendlyURLName = Request.QueryString["businessEventFriendlyURLName"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var businessEvent = (from oe in dc.BusinessEvents
								join ev in dc.Events on oe.EventId equals ev.EventId
								join o in dc.Businesses on oe.BusinessId equals o.BusinessId
								where oe.URLFriendlyCampaignName == businessEventFriendlyURLName
								select new { oe, ev, o }).SingleOrDefault();

		var businessProfilePhoto = (from bp in dc.BusinessPhotos
									join p in dc.Photos on bp.PhotoId equals p.PhotoId
									where bp.BusinessEventId == businessEvent.oe.BusinessEventId
									orderby p.CreatedOn descending
									select new { p.FilenameCropped }).Take(1).SingleOrDefault();
				
		if(businessProfilePhoto != null)
		{
			imgBusinessProfile.Src = businessPhotoFolder + businessProfilePhoto.FilenameCropped;
		}

		if (businessEvent.ev.Icon != null)
		{
			icon = businessEvent.ev.Icon.Replace("COLOR", "btn-" + businessEvent.ev.Color + " btn-outline");
		}

		litCampaignName.Text = businessEvent.oe.CampaignName;
		litEventName.Text = businessEvent.ev.Name;
		hypEventName.Text = businessEvent.ev.Name;
		hypEventName.NavigateUrl = "/" + businessEvent.ev.URLFriendlyName + "/Business";
		hypEventName.Font.Underline = true;

		var peopleList =	from uo in dc.BusinessUsers
							join p in dc.Profiles on uo.UserId equals p.UserId
							where uo.BusinessId == businessEvent.oe.BusinessId
							orderby p.Title descending
							select p;


				 rpNonProfitPeople.DataSource = peopleList;
		rpNonProfitPeople.DataBind();
		
		Master.PageTitle			= businessEvent.oe.CampaignName + " by " + businessEvent.o.Name + " - Stability";
		Master.PageDescription		= businessEvent.oe.MissionPurpose;
		Master.FbDescription		= businessEvent.oe.MissionPurpose;
		Master.FbImage				= "/V1/Images/" + businessEvent.ev.ImageFileName;
		Master.FbImageType			= "image/jpg";
		Master.FbSite_name			= businessEvent.oe.CampaignName + " by " + businessEvent.o.Name + " - Stability";
		Master.FbURL				= Request.Url.AbsoluteUri;

		lblVolunteerInstructions.Text		= businessEvent.oe.VolunteerInstructions;
		litCampaignMission.Text				= businessEvent.oe.MissionPurpose;
		hypBusinessName.Text			= businessEvent.o.Name;
		hypBusinessName.NavigateUrl		= businessEvent.o.Website;
		hypBusinessName.Font.Underline	= true;
		lblParentOrgName.Text				= businessEvent.o.Name;
		hypParentAddress.Text				= businessEvent.o.Address + "<br/>" + businessEvent.o.City + ", " + businessEvent.o.State + " " + businessEvent.o.Zip;
		hypParentAddress.NavigateUrl		= "http://maps.google.com/maps?q=" + businessEvent.o.Address.Replace(" ","+") + "," + businessEvent.o.City.Replace(" ","+") + "," + businessEvent.o.State.Replace(" ","+") + "," + businessEvent.o.Zip;
			
		lblPointOfContactPerson.Text = businessEvent.o.PointOfContactName;
		if(!String.IsNullOrEmpty(businessEvent.o.PointOfContactPhoneNumber))
		{
			hypPointOfContactPhone.Text				= Regex.Replace(businessEvent.o.PointOfContactPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
			hypPointOfContactPhone.NavigateUrl			= "tel:" + businessEvent.o.PointOfContactPhoneNumber;
			hypPointOfContactPhone.Font.Underline		= true;
		}

		if (userId == businessEvent.o.OwnerId)
		{
			divEditCampaign.Visible = true;
			editCampaignLink = "/V1/BusinessAdministration/EditBusinessCampaign.aspx?BusinessEventId=" + businessEvent.oe.BusinessEventId;
			uploadLogoLink = "/V1/BusinessAdministration/UploadBusinessCampaignLogo.aspx?BusinessEventId=" + businessEvent.oe.BusinessEventId;
		}

			if (!String.IsNullOrEmpty(businessEvent.oe.BlogURL))
		{
			hypBlog.NavigateUrl = businessEvent.oe.BlogURL;
			hypBlog.Text = businessEvent.oe.BlogURL;
			hypBlog.Font.Underline = true;
		}
		if (!String.IsNullOrEmpty(businessEvent.oe.ZelloChannel))
		{
			dtZelloChannel.Visible = true;
			ddZelloChannel.Visible = true;
			lblZelloChannel.Text = businessEvent.oe.ZelloChannel;
		}

		if (!String.IsNullOrEmpty(businessEvent.o.PointOfContactEmail))
		{
			hypPointOfContactEmail.Text				= businessEvent.o.PointOfContactEmail;
			hypPointOfContactEmail.NavigateUrl		= "mailto:" + businessEvent.o.PointOfContactEmail;
			hypPointOfContactEmail.Font.Underline	= true;
		}
			
			
		if(!String.IsNullOrEmpty(businessEvent.o.FacebookURL))
		{
			hypFacebookPage.Text			= businessEvent.o.Name + " Facebook Page";
			hypFacebookPage.NavigateUrl		= businessEvent.o.FacebookURL;
			hypFacebookPage.Font.Underline	= true;
		}

		if(!String.IsNullOrEmpty(businessEvent.o.FacebookGroupURL))
		{
			hypFacebookGroup.Text			= businessEvent.o.Name + " Facebook Group";
			hypFacebookGroup.NavigateUrl	= businessEvent.o.FacebookGroupURL;
			hypFacebookGroup.Font.Underline	= true;
		}
			
		if(!String.IsNullOrEmpty(businessEvent.o.TwitterURL))
		{
			hypTwitter.Text					= "Visit " + businessEvent.o.TwitterURL;
			hypTwitter.NavigateUrl			= "https://www.Twitter.com/" + businessEvent.o.TwitterURL;
			hypTwitter.Font.Underline		= true;
		}
			
		if(!String.IsNullOrEmpty(businessEvent.o.YouTubeURL))
		{
			hypYouTube.Text					= businessEvent.o.Name + " YouTube Channel";
			hypYouTube.NavigateUrl			= businessEvent.o.YouTubeURL;
			hypYouTube.Font.Underline		= true;
		}
			
		if(!String.IsNullOrEmpty(businessEvent.o.PrimaryPhone))
		{
			dtPrimaryPhone.Visible				= true;
			ddPrimaryPhone.Visible				= true;
			hypPrimaryPhone.Text				= Regex.Replace(businessEvent.o.PrimaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
			hypPrimaryPhone.NavigateUrl			= "tel:" + businessEvent.o.PrimaryPhone;
			hypPrimaryPhone.Font.Underline		= true;
		}


		if(!String.IsNullOrEmpty(businessEvent.o.PublicPhoneNumber))
		{
			ddParentPhone.Visible				= true;
			hypParentPhone.Text			= Regex.Replace(businessEvent.o.PublicPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
			hypParentPhone.NavigateUrl	= "tel:" + businessEvent.o.PublicPhoneNumber;
			hypParentPhone.Font.Underline = true;
		}

		if(!String.IsNullOrEmpty(businessEvent.o.PublicEmail))
		{ 
			
			ddParentEmail.Visible			= true;
			hypParentEmail.Text				= businessEvent.o.PublicEmail;
			hypParentEmail.NavigateUrl		= "mailto:" + businessEvent.o.PublicEmail;
			hypParentEmail.Font.Underline	= true;
		}

		if(!String.IsNullOrEmpty(businessEvent.o.Website))
		{
			dtWebsite.Visible = true;
			ddWebsite.Visible			= true;
			hypWebsite.Text				= businessEvent.o.Website;
			hypWebsite.NavigateUrl		= businessEvent.o.Website;
			hypWebsite.Font.Underline	= true;
		}

		if ((bool)businessEvent.oe.AcceptsVolunteers)
		{
			lbVolunteer.Visible = true;
			volunteerLink = "/CampaignVolunteer/" + businessEvent.oe.URLFriendlyCampaignName;
		}

		if (!String.IsNullOrEmpty(businessEvent.o.DonationURL))
		{
			lbDonate.Visible = true;
			donateLink = businessEvent.o.DonationURL;
		}
	}
    
    protected void rpNonProfitPeople_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
        //List disasters
        //Show their photo
        //Show their skills
        
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String firstname	= (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String lastname		= (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
			String zelloName		= (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
			String title	= (String)DataBinder.Eval(dataItem.DataItem, "Title");

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var profilePhoto = (from p in dc.ProfilePhotos
							   join ph in dc.Photos on p.PhotoId equals ph.PhotoId
							   where p.UserId == userId && p.IsCurrrent == true
							   select new {ph.FilenameCropped }).Take(1).SingleOrDefault();
            
            Literal lblInfo = (Literal)e.Item.FindControl("lblInfo");
			Image imgProfilePhoto = (Image)e.Item.FindControl("imgProfilePhoto");

            imgProfilePhoto.ImageUrl = "/V1/Images/icons8-customer-64.png";
			if(profilePhoto != null)
			{
				imgProfilePhoto.ImageUrl = profilePhotoFolder + profilePhoto.FilenameCropped;
			}

			zelloName = String.IsNullOrEmpty(zelloName) ? "none" : zelloName;
			title = String.IsNullOrEmpty(title) ? "none" : title;
				
			lblInfo.Text = "<p><dl class=\"dl-horizontal\" class=\"m-l-sm\"><dd><b><a href=\"/V1/Profile/Profile.aspx?userId=" + userId.ToString() + "\"  style=\"text-decoration:underline;\">" + firstname + " "+  lastname + "</a></b><br>Zello: " + zelloName + "<br>Title: " + title + "</dd></dl></p>";
		}
	}
}