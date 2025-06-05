using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_NonProfit : BaseOrganizationWebForm
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string donateLink = string.Empty;
	public string impactoidLink = string.Empty;
	public string volunteerLink = string.Empty; 
	public string organizationId = string.Empty;
	public string activityPageLink = string.Empty;
	public string teamMembersLink = string.Empty;
	public string nonProfitDropDown = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationId = Request.QueryString["organizationId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

		if (organization != null)
		{
			if(!User.Identity.IsAuthenticated)
			{
				if ((bool)!organization.IsActive)
				{
					Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=79305f85-3816-46a8-911f-0d7e3e227c32");
				}
			}

			hypLogo.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId;
			if (!String.IsNullOrEmpty(organization.Logo))
			{	imgLogo.Visible = true;
				imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + organization.Logo;
				imgLogo.AlternateText = organization.Name + " Logo";
			}
			else
			{
				//Use placeholder image.imgLogo.Visible = true;
				imgLogo.ImageUrl = "/V1/Images/Logo-Placeholder.png";
				imgLogo.AlternateText = organization.Name + " Logo";
			}

			if ((bool)!organization.IsActive)
			{
				divAlertPageMessage.Visible = true;
			}

			if (User.IsInRole("Administrator"))
			{
				if (!String.IsNullOrEmpty(Request.QueryString["ownerId"]))
				{
					//Set the new ownerId
					organization.OwnerId = new Guid(Request.QueryString["ownerId"]);
					dc.SubmitChanges();
					litAlertMessage.Text = "A new non-profit page owner has been set.";
					divAlertMessage.Visible = true;
				}
			}

			Master.PageTitle = organization.Name + " on Stability";
			Master.PageDescription = organization.Description;
			Master.FbDescription = organization.Description;
			Master.FbImage = organization.CoverImage;
			Master.FbImageType = "image/jpg";
			Master.FbSite_name = organization.Name + " on Stability";
			Master.FbURL = Request.Url.AbsoluteUri;

			litOrganizationName.Text = organization.Name;
			lblOrgName.Text = organization.Name;
			litMission.Text = organization.PurposeMission;
			litDescription.Text = organization.Description;
			litYearFounded.Text = organization.YearFounded;
			hypAddress.Text = organization.Address + "<br/>" + organization.City + ", " + organization.State + " " + organization.Zip;
			hypAddress.NavigateUrl = "http://maps.google.com/maps?q=" + organization.Address.Replace(" ", "+") + "," + organization.City.Replace(" ", "+") + "," + organization.State.Replace(" ", "+") + "," + organization.Zip;
			lblVoadMember.Text = organization.IsVoadMember.ToString();
			lbl501c3.Text = organization._501c3Status.ToString();


			lbVolunteer.Visible = true;
			if (User.Identity.IsAuthenticated)
			{
				//If the user is logged in and not in a nonprofit already then send to choose a nonprofit.
				var userOrganization = from uo in dc.UserOrganizations
										where uo.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString()) && uo.Status== (int)RequestStatus.Approved && uo.Status == (int)RequestStatus.Pending
                                        && uo.OrganizationId == new Guid(organizationId)
										select uo;

				if (userOrganization.Count() == 0)
				{
					//Tell the user they can choose this nonprofit to volunteer with.
					volunteerLink = "/V1/Profile/EditNonProfits.aspx?organizationId=" + organizationId;
				}
				else
				{
					//User is already volunteering for this nonprofit, show that message and disable the volunteer button.
					lbVolunteer.Visible = false;
					btnActiveVolunteer.Text = "You are on this team.";
					btnActiveVolunteer.Visible = true;
					btnActiveVolunteer.Enabled = false;
				}

			}
			else
			{
				volunteerLink = "/Register/" + organizationId;
			}

			lblPointOfContactPerson.Text = organization.PointOfContactName;
			if (!String.IsNullOrEmpty(organization.PointOfContactPhoneNumber))
			{
				hypPointOfContactPhone.Text = Regex.Replace(organization.PointOfContactPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypPointOfContactPhone.NavigateUrl = "tel:" + organization.PointOfContactPhoneNumber;
				hypPointOfContactPhone.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.PointOfContactEmail))
			{
				hypPointOfContactEmail.Text = organization.PointOfContactEmail;
				hypPointOfContactEmail.NavigateUrl = "mailto:" + organization.PointOfContactEmail;
				hypPointOfContactEmail.Font.Underline = true;
			}


			if (!String.IsNullOrEmpty(organization.FacebookURL))
			{
				hypFacebookPage.Text = organization.Name + " Facebook Page";
				hypFacebookPage.NavigateUrl = organization.FacebookURL;
				hypFacebookPage.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.FacebookGroupURL))
			{
				hypFacebookGroup.Text = organization.Name + " Facebook Group";
				hypFacebookGroup.NavigateUrl = organization.FacebookGroupURL;
				hypFacebookGroup.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.TwitterURL))
			{
				hypTwitter.Text = "Visit " + organization.TwitterURL;
				hypTwitter.NavigateUrl = "https://www.Twitter.com/" + organization.TwitterURL;
				hypTwitter.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.InstagramURL))
			{
				hypInstagram.Text = "Instagram";
				hypInstagram.NavigateUrl = "https://www.Instagram.com/" + organization.InstagramURL;
				hypInstagram.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.YouTubeURL))
			{
				hypYouTube.Text = organization.Name + " YouTube Channel";
				hypYouTube.NavigateUrl = organization.YouTubeURL;
				hypYouTube.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.EIN))
			{
				lblEIN.Text = organization.EIN;
				dtEIN.Visible = true;
				ddEIN.Visible = true;
			}

			if (!String.IsNullOrEmpty(organization.PrimaryPhone))
			{
				dtPrimaryPhone.Visible = true;
				ddPrimaryPhone.Visible = true;
				hypPrimaryPhone.Text = Regex.Replace(organization.PrimaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypPrimaryPhone.NavigateUrl = "tel:" + organization.PrimaryPhone;
				hypPrimaryPhone.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.SecondaryPhone))
			{
				dtSecondaryPhone.Visible = true;
				ddSecondaryPhone.Visible = true;
				hypSecondaryPhone.Text = Regex.Replace(organization.SecondaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypSecondaryPhone.NavigateUrl = "tel:" + organization.SecondaryPhone;
				hypSecondaryPhone.Font.Underline = true;
			}


			if (!String.IsNullOrEmpty(organization.PublicPhoneNumber))
			{
				ddPublicPhone.Visible = true;
				hyoPublicPhoneNumber.Text = Regex.Replace(organization.PublicPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hyoPublicPhoneNumber.NavigateUrl = "tel:" + organization.PublicPhoneNumber;
				hyoPublicPhoneNumber.Font.Underline = true;
			}
			if (!String.IsNullOrEmpty(organization.PublicEmail))
			{
				ddPublicEmail.Visible = true;
				hypPublicEmailAddress.Text = organization.PublicEmail;
				hypPublicEmailAddress.NavigateUrl = "mailto:" + organization.PublicEmail;
				hypPublicEmailAddress.Font.Underline = true;
			}
			if (!String.IsNullOrEmpty(organization.Website))
			{
				ddWebsite.Visible = true;
				hypWebsite.Text = organization.Website;
				hypWebsite.NavigateUrl = organization.Website;
				hypWebsite.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.BlogURL))
			{
				ddBlog.Visible = true;
				hypBlog.Text = organization.BlogURL;
				hypBlog.NavigateUrl = organization.BlogURL;
				hypBlog.Font.Underline = true;
			}

			if (!String.IsNullOrEmpty(organization.DonationURL))
			{
				lbDonate.Visible = true;
				donateLink = organization.DonationURL;
			}
			bool isWebsiteActive = organization.IsWebsiteActive == null ? false : (bool)organization.IsWebsiteActive;
			if (isWebsiteActive)
			{
				lbImpactoidWebsite.Visible = true;
				divSubscribeToWebsite.Visible = false;
				string url = !String.IsNullOrEmpty(organization.URLFriendlyName) ? "/Impactoid/CommunityPage.aspx?organizationName=" + organization.URLFriendlyName : "/Impactoid/CommunityPage.aspx?organizationId=" + organization.OrganizationId;
				lbImpactoidWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + organization.OrganizationId;// url;
			}
			else
			{
				lbImpactoidWebsite.Visible = false;
				divSubscribeToWebsite.Visible = true;
				hypDemoWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + organizationId;
			}

			if (User.IsInRole("Administrator"))
			{
				lbImpactoidWebsite.Visible = true;
				divSubscribeToWebsite.Visible = true;
				string url = !String.IsNullOrEmpty(organization.URLFriendlyName) ? "/Impactoid/CommunityPage.aspx?organizationName=" + organization.URLFriendlyName : "/Impactoid/CommunityPage.aspx?organizationId=" + organization.OrganizationId;
				lbImpactoidWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + organization.OrganizationId;// url;
			}

			activityPageLink = "/V1/NonProfit/Default.aspx?organizationId=" + organization.OrganizationId;
			teamMembersLink = "/V1/NonProfitAdministration/VolunteerList.aspx?organizationId=" + organization.OrganizationId;
		}

		if (String.IsNullOrEmpty(Request.QueryString["organizationId"]))
		{
			Response.Write("No organization Id provided.");
			Response.End();
        }

		LoadNonProfits();

		var peopleList = from uo in dc.UserOrganizations
						 join p in dc.Profiles on uo.UserId equals p.UserId
						 join net in dc.aspnet_Memberships on p.UserId equals net.UserId
						 join u in dc.aspnet_Users on p.UserId equals u.UserId
						 where uo.OrganizationId == new Guid(organizationId) && uo.Status== (int)RequestStatus.Approved && uo.Status == (int)RequestStatus.Pending
                         && p.PassedVetting == true
						 && net.IsApproved == true
						 && net.LastLoginDate > DateTime.Now.AddDays(-30)
						 orderby net.LastLoginDate descending
						 select new { p.Firstname, p.Lastname, p.UserId, p.Title, p.ZelloName, LastLoginDate = u.LastActivityDate };

		rpNonProfitPeople.DataSource = peopleList;
		rpNonProfitPeople.DataBind();

		bool isOwner = false;
		if(User.Identity.IsAuthenticated == true)
		{ 
			var userOrganizationOwner = (from uo in dc.UserOrganizations
									join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
									where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString()) && uo.Status== (int)RequestStatus.Approved && uo.Status == (int)RequestStatus.Pending
                                    && uo.OrganizationId == new Guid(organizationId)
									select o).Take(1).SingleOrDefault();

			if (userOrganizationOwner != null)
			{ 
				if((userOrganizationOwner.OwnerId != userId))
				{
					isOwner = true;
				}
			}
		}

		if (User.IsInRole("Administrator") || isOwner)
		{
			if(organization.IsActive != true)
			{
				btnDeactivatePage.Text = "Re-activate This Team";
			}

			btnViewTeamMembers.Visible = true;
			btnInviteTeamMembers.Visible = true;
			hypViewAllVolunteers.Visible = true;
			hypViewAllVolunteers.NavigateUrl = "/V1/NonProfitAdministration/VolunteerList.aspx?organizationId=" + organizationId;
			hypCause.Visible = true;
			hypCause.NavigateUrl = "/V1/NonProfitAdministration/RespondToEvent.aspx?organizationId=" + organizationId;
			divUnpublishedInformation.Visible = true;
			divUploadLogoCover.Visible = true;
			dtEIN.Visible = true;
			ddEIN.Visible = true;
			btnUploadLogo.Visible = true;
			btnUploadCoverImage.Visible = true;
			btnManagePhotos.Visible = true;
			btnEditMyGroup.Visible = true;

			//Show all active and inactive
			var campaigns = from oe in dc.OrganizationEvents
							join ev in dc.Events on oe.EventId equals ev.EventId
							where oe.OrganizationId == new Guid(organizationId)
							orderby ev.BeginDate descending
							select new { oe.OrganizationEventId, oe.IsActive, oe.URLFriendlyCampaignName, ev.URLFriendlyName, oe.MissionPurpose, campaignName = oe.CampaignName, disasterName = ev.Name };

			rptActiveCampaigns.DataSource = campaigns;
			rptActiveCampaigns.DataBind();
		}
		else
		{
			var campaigns = from oe in dc.OrganizationEvents
							join ev in dc.Events on oe.EventId equals ev.EventId
							where oe.OrganizationId == new Guid(organizationId)
							&& oe.IsActive == true
							orderby ev.BeginDate descending
							select new { oe.OrganizationEventId, oe.IsActive, oe.URLFriendlyCampaignName, ev.URLFriendlyName, oe.MissionPurpose, campaignName = oe.CampaignName, disasterName = ev.Name };

			rptActiveCampaigns.DataSource = campaigns;
			rptActiveCampaigns.DataBind();
		}

	}
	public void LoadNonProfits()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var nonProfits = from o in dc.Organizations
							where o.IsActive == true
							orderby o.Name
							select new { o };

		foreach (var nonProfit in nonProfits)
		{
			nonProfitDropDown += "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;
		}
	}

	protected void rptActiveCampaigns_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			HyperLink hypCauseName = (HyperLink)e.Item.FindControl("hypCauseName");
			HyperLink hypDisasterName = (HyperLink)e.Item.FindControl("hypDisasterName");
			HyperLink hypEdit = (HyperLink)e.Item.FindControl("hypEdit");
			Label lblCauseMission = (Label)e.Item.FindControl("lblCauseMission");
			Label lblInActiveFlag = (Label)e.Item.FindControl("lblInActiveFlag"); 

			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "campaignName");
			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId"); 
			string disasterName = (string)DataBinder.Eval(dataItem.DataItem, "disasterName");
			string URLFriendlyName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyName");
			string missionPurpose = (string)DataBinder.Eval(dataItem.DataItem, "missionPurpose");
			string URLFriendlyCampaignName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyCampaignName");
			bool isActive = (bool)DataBinder.Eval(dataItem.DataItem, "IsActive");

			hypCauseName.Text = campaignName;
			hypCauseName.NavigateUrl = "~/Cause/" + URLFriendlyCampaignName;
			hypDisasterName.Text = disasterName;
			hypDisasterName.NavigateUrl = "~/Disaster/" + URLFriendlyName;
			lblCauseMission.Text = "<b>Mission:</b> " + missionPurpose;

			if (User.IsInRole("Administrator"))
			{
				hypEdit.Visible = true;
				hypEdit.NavigateUrl = "/V1/NonProfitAdministration/EditNonProfitCampaign.aspx?OrganizationEventId=" + organizationEventId;
				if (!isActive)
				{ 
					lblInActiveFlag.Visible = true;
				}
			}
		}
	}

	protected void rpNonProfitPeople_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String firstname = (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String lastname = (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
			String zelloName = (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
			String title = (String)DataBinder.Eval(dataItem.DataItem, "Title");
			DateTime lastOnlineActiveDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "LastLoginDate");

			Literal lblInfo = (Literal)e.Item.FindControl("lblInfo");

			DateTime lastLoginDate = DateTime.Now;
			double timeSpanHoursInt = 0;
			double timeSpanMinutesInt = 0;
			double timeSpanDaysInt = 0;
			string signedInInfo = string.Empty;

			string activedMessage = string.Empty;
			string deActivedMessage = string.Empty;

			string activeColorClass = string.Empty;
			string totalTimeToday = string.Empty;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			//Is the user signed in?
			var timesheet = (from t in dc.Timesheets
							 join tt in dc.TaskTypes on t.TaskTypeId equals tt.TaskTypeId into timeJoin
							 from time in timeJoin.DefaultIfEmpty()
							 where t.UserId == userId && t.TimeIn != null
							 orderby t.TimeIn descending
							 select new { t.TimeIn, t.TimeOut, t.Description, taskname = time.Name }).Take(1).SingleOrDefault();

			if (timesheet != null)
			{
				//If user has no timeout and more than 8 hours has passed then...
				TimeSpan? spanToday = (DateTime.Now - timesheet.TimeIn);
				timeSpanDaysInt = spanToday.Value.TotalDays;
				timeSpanHoursInt = spanToday.Value.TotalHours;
				timeSpanMinutesInt = spanToday.Value.Minutes;

				activedMessage = "<span class='" + activeColorClass + "'>Active at: " + timesheet.TimeIn.ToLongTimeString() + " " + timesheet.TimeIn.ToLongDateString() + "</span>";

				if (timesheet.TimeOut == null)
				{
					//User is either signed in or they forgot to sign out.
					if (timeSpanHoursInt > 8)
					{
						signedInInfo = "<span class='text-danger'>Forgot to sign out.</span><br>";
						//PROBLEM - User signed in more than 8 hours ago and did not sign out, show how many days/hours.
						activeColorClass = "text-danger";
						deActivedMessage = "<br>De-Activated at: <i>User has been Active for more than 8 hours.</i>";
						totalTimeToday = "<span class='text-danger'>User did not De-Activate. They have been Active for more than 8 hours.</span>";
					}
					else if (timeSpanHoursInt <= 8)
					{
						signedInInfo = "<span class='text-success'>Currently Tracking Time</span><br>";
						//User recently signed in.//GOOD ZONE USER IS SIGNED IN AND HAS BEEN SO FOR LESS THAN 8 HOURS
						//User signed in for today and has been active for less than 8 hours.
						deActivedMessage = "<br>De-Activated at: <i><small>User Activated less than 8 hours ago.</small></i>";
						activeColorClass = "text-success";
						totalTimeToday = "<span class='text-success'>" + (spanToday.Value.Hours > 0 ? spanToday.Value.Hours + " hours " + timeSpanMinutesInt + " minutes " : timeSpanMinutesInt + " minutes ") + "</span>";
					}
				}
				else
				{
					//User has a sign out that matches their sign in.
					signedInInfo = "<span>Not Tracking Time</span><br>";
					spanToday = (timesheet.TimeOut - timesheet.TimeIn);
					activeColorClass = string.Empty;
					deActivedMessage = "<br>De-Activated at: " + timesheet.TimeOut.Value.ToLongTimeString() + " " + timesheet.TimeOut.Value.ToLongDateString();
					totalTimeToday = "<span class='text-success'>" + (spanToday.Value.Hours > 0 ? spanToday.Value.Hours + " hours " + spanToday.Value.Minutes + " minutes " : spanToday.Value.Minutes + " minutes ") + "</span>";
				}

				signedInInfo = signedInInfo + activedMessage + deActivedMessage;
			}

			zelloName = String.IsNullOrEmpty(zelloName) ? "none" : zelloName;
			title = String.IsNullOrEmpty(title) ? "none" : title;
			string active = (GetElapsedTime(lastOnlineActiveDate).Substring(0, 1) == "-" ? "<span class='text-success'>Online Now</span> <i class=\"fa fa-wifi text-success\"></i>" : "Last Online: " + GetElapsedTime(lastOnlineActiveDate) + " <i class=\"fa fa-wifi text-muted\"></i>");
			lblInfo.Text = "<dl class=\"dl-vertical\"><dt><b><a href=\"/V1/Profile/Profile.aspx?userId=" + userId.ToString() + "\"  style=\"text-decoration:underline;\">" + firstname + " " + lastname + "</a></b> <small>" + active + "</small></dt><dd><small>" + title + "</small></dd><dd><small>" + signedInInfo + "</small></dd><dd><small>Last Active Total: " + totalTimeToday + "</small></dd></dl>";










			//HyperLink hypMakeOwner = (HyperLink)e.Item.FindControl("hypMakeOwner");
			//hypMakeOwner.NavigateUrl = "NonProfit.aspx?organizationId=" + Request.QueryString["organizationId"] +"&ownerId=" + userId.ToString();
			//hypMakeOwner.Text = "Set '" + firstname + "' As Owner";
			//if (User.IsInRole("Administrator"))
			//{
			//    hypMakeOwner.Visible = true;
			//}
		}
	}
}