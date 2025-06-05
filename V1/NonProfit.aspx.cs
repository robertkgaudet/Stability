using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit : BaseOrganizationWebForm
{
	public string eventName = HttpContext.Current.Request.QueryString["eventName"];
	public Guid eventId = Guid.Empty;
	public string registerNonProfit = "/V1/NonProfitNew.aspx";
	public string editCampaign = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		string nonProfitPageTitle = System.Configuration.ConfigurationManager.AppSettings["NonProfitPageTitle"];

		if(!IsPostBack)
		{
			var disaster = (from ev in dc.Events
							where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
							select ev).SingleOrDefault();

			if(disaster != null)
			{
				string pageDescription = "Non-profit organizations are invited to create a response to " + disaster.Name + ". This will allow you to collaborate with other non-profits, find volunteers, assist with home rebuilds, manage resources and much more. <br> Responding to this disaster requires two steps. <ol><li>Add your organization to the Stability system.</li> <li>Then create a non-profit response to " + disaster.Name + ".</li></ol>";
				string pageTitle = disaster.Name + nonProfitPageTitle;

				this.Master.PageTitle = pageTitle;
				this.Master.PageDescription = pageDescription;
				this.Master.FbDescription = pageDescription;
				this.Master.FbSite_name = pageTitle;

				uc1EventHeader.PageTitle		= nonProfitPageTitle;
				uc1EventHeader.EventName		= disaster.Name;
				uc1EventHeader.PageDescription	= pageDescription;
				eventId = disaster.EventId;
			}


			btnRegisterNonProfit.Text = "Register My NonProfit";
			litAddNonProfitEvent.Text = "Nonprofits must register with Stability, to be eligible to create a response to this disaster.";
			//Does somene have a non-profit setup in Stability?
			var nonProfit = from np in dc.Organizations
							where np.OwnerId == userId
							select np;

			if (nonProfit.Count() > 0)
			{
				//Create a campaign
				btnRegisterNonProfit.Text = "Create Disaster Campaign";
				btnRegisterNonProfit.Visible = true;

				litRegisterNonProfit.Visible = false;

				divAddNonProfit.Visible = true;

				litNonProfitName.Text = "<h3>" + nonProfit.Take(1).SingleOrDefault().Name + "</h3>";
				litAddNonProfitEvent.Text = " Your non-profit, <b>" + nonProfit.Take(1).SingleOrDefault().Name + "</b>, is eligible to create a Campaign for responding to " + disaster.Name + ".";
				litAddNonProfitEvent.Visible = true;

				registerNonProfit = "/V1/NonProfitAdministration/RespondToEvent.aspx?organizationId=" + nonProfit.Take(1).SingleOrDefault().OrganizationId + "&eventId=" + eventId;
			}

			var nonProfitCampaign = from npe in dc.OrganizationEvents
									where npe.EventId == eventId
									select npe;

			if (nonProfitCampaign.Count() > 0)
			{
				//List the non-profits that are responding to this event.
				LoadNonProfits(eventId);

				//Does this user have a campaign?
				var nonProfitCampaignUser = from npe in dc.OrganizationEvents
											where npe.EventId == eventId && npe.Createdby == userId
											select npe;

				if(nonProfitCampaignUser.Count() > 0)
				{ 
					//They already have a campaign response.
					btnRegisterNonProfit.Visible = false;
					litRegisterNonProfit.Visible = false;
					litAddNonProfitEvent.Visible = false;
					divAddNonProfit.Visible = false;
				}
				else
				{
					btnRegisterNonProfit.Visible = true;
					litRegisterNonProfit.Visible = true;
					litAddNonProfitEvent.Visible = true;
				}
			}


				
		}
	}

	protected void LoadNonProfits(Guid eventId)
	{
		string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var nonProfitCampaigns = from oe in dc.OrganizationEvents
									join o in dc.Organizations on oe.OrganizationId equals o.OrganizationId
									where oe.EventId == eventId
									orderby oe.CreatedOn descending
									select new { oe, o};

		string nonProfitCampaignString = string.Empty;
		foreach (var nonProfitCampaign in nonProfitCampaigns)
		{

			string adminButtons = string.Empty;
			string volunteerButton = string.Empty;
			string donationButton = string.Empty;
			string volunteerList = string.Empty;

			var volunteers = from uo in dc.UserOrganizations
							 join p in dc.Profiles on uo.UserId equals p.UserId
							 where uo.OrganizationId == nonProfitCampaign.o.OrganizationId && (uo.Status== (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                             orderby p.Title descending
							 select new { p.Firstname, p.Lastname, p.UserId, p.Title, p.ZelloName };

			if (volunteers.Count() > 0)
			{
				foreach (var volunteer in volunteers.Take(10))
				{
					var profilePhoto = (from p in dc.ProfilePhotos
										join ph in dc.Photos on p.PhotoId equals ph.PhotoId
										where p.UserId == volunteer.UserId && p.IsCurrrent == true
										select new { ph.FilenameCropped }).Take(1).SingleOrDefault();

					if (profilePhoto != null && !String.IsNullOrEmpty(profilePhoto.FilenameCropped))
					{
						volunteerList += "<a href =\"/V1/Profile/Profile.aspx?userId=" + volunteer.UserId + "\"><img alt = \"logo\" class=\"img-circle m-t-xs\" width=\"40px\" src=\"" + profilePhotoFolder + profilePhoto.FilenameCropped + "\"></a>" + Environment.NewLine;
					}
				}
			}


			if (!String.IsNullOrEmpty(nonProfitCampaign.oe.DonationURL))
			{
				donationButton =
									"<a href=\"" + nonProfitCampaign.oe.DonationURL + "\" class=\"btn btn-xs btn-success m-t-sm\" target=\"_blank\">" + Environment.NewLine +
										"Donate" + Environment.NewLine +
									"</a>" + Environment.NewLine;
			}

			if ((bool)nonProfitCampaign.oe.AcceptsVolunteers)
			{
				volunteerButton = "<a href=\"/V1/Profile/Volunteer.aspx?eventId=" + nonProfitCampaign.oe.EventId + "\" class=\"btn btn-xs btn-success m-l-xs m-t-sm\">" + Environment.NewLine +
											"Volunteer" + Environment.NewLine +
										"</a>" + Environment.NewLine;
			}

			if (userId == nonProfitCampaign.o.OwnerId)
			{
				adminButtons = "<div class=\"project-action pull-right\">" + Environment.NewLine +
									"<div class=\"btn-group\">" + Environment.NewLine +
										"<button class=\"btn btn-xs btn-warning editCampaign\"> Edit</button>" + Environment.NewLine +
									"</div>" + Environment.NewLine +
								"</div>" + Environment.NewLine;

				editCampaign = "/V1/NonProfitAdministration/EditNonProfitCampaign.aspx?OrganizationEventId=" + nonProfitCampaign.oe.OrganizationEventId;
			}

			//Count the rebuilds this organization is managing.

			var rebuildCount = (from or in dc.OrganizationRebuilds
								join r in dc.Rebuilds on or.RebuildId equals r.RebuildId
								where or.OrganizationId == nonProfitCampaign.o.OrganizationId 
                                && r.EventId == eventId
								select or).Count();

			var volunteerCount = (from ue in dc.UserEvents
						  join ur in dc.aspnet_UsersInRoles on ue.UserId equals ur.UserId
						  join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
						  join uo in dc.UserOrganizations on ue.UserId equals uo.UserId
						  where r.LoweredRoleName == "helper" && (uo.Status== (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                        && ue.EventId == eventId
						&& uo.OrganizationId == nonProfitCampaign.o.OrganizationId
						select ue).Count();


			//var volunteerCount = (from p in dc.Profiles
			//					 join ur in dc.aspnet_UsersInRoles on p.UserId equals ur.UserId
			//					 join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
			//					 join uo in dc.UserOrganizations on p.UserId equals uo.UserId
			//					 join ue in dc.UserEvents on p.UserId equals ue.UserId
			//					 join oev in dc.OrganizationEvents on ue.EventId equals oev.EventId
			//					 where r.LoweredRoleName == "volunteer" || r.LoweredRoleName == "helper"
			//					 && oev.EventId == eventId
			//					 && uo.OrganizationId == nonProfitCampaign.o.OrganizationId
			//					 select p).Count();

			var survivorCount = (from p in dc.Profiles
								  join ur in dc.aspnet_UsersInRoles on p.UserId equals ur.UserId
								  join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
								  join uo in dc.UserOrganizations on p.UserId equals uo.UserId
								  join ue in dc.UserEvents on p.UserId equals ue.UserId
								  where r.LoweredRoleName == "survivor" && (uo.Status== (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                                  && ue.EventId == eventId
								  && uo.OrganizationId == nonProfitCampaign.o.OrganizationId
								  select p).Count();

			//Count the community size.

			nonProfitCampaignString +=
					 "<div class=\"grid-item m-t-lg\"><div class=\"hpanel hgreen\">" + Environment.NewLine +
					 "<div class=\"panel-body\">" + Environment.NewLine +
						 "<span class=\"label label-success pull-right\">NEW</span>" + Environment.NewLine +
						 "<div class=\"row\">" + Environment.NewLine +
							 "<div class=\"col-sm-8\">" + Environment.NewLine +
								"<a href=\"/V1/NonProfit/Default.aspx?organizationId=" + nonProfitCampaign.o.OrganizationId + "\">" + nonProfitCampaign.o.Name + "</a>" + Environment.NewLine +
								 "<h4><a href = \"/Cause/" + nonProfitCampaign.oe.URLFriendlyCampaignName + "\" >" + nonProfitCampaign.oe.CampaignName + "</a></h4>" + Environment.NewLine +
								 "<p>" + Environment.NewLine +
									 nonProfitCampaign.oe.MissionPurpose + Environment.NewLine +
								 "</p>" + Environment.NewLine +
								 "<div class=\"row\">" + Environment.NewLine +
									 "<div class=\"col-sm-3\">" + Environment.NewLine +
										 "<div class=\"project-label\">SURVIVORS</div>" + Environment.NewLine +
										 "<small>" + survivorCount.ToString() + "</small>" + Environment.NewLine +
									 "</div>" + Environment.NewLine +
									 "<div class=\"col-sm-3\">" + Environment.NewLine +
										 "<div class=\"project-label\">VOLUNTEERS</div>" + Environment.NewLine +
										 "<small>" + volunteerCount.ToString() + "</small>" + Environment.NewLine +
									 "</div>" + Environment.NewLine +
									 "<div class=\"col-sm-3\">" + Environment.NewLine +
										 "<div class=\"project-label\">HOMES</div>" + Environment.NewLine +
										 "<small>" + rebuildCount.ToString() + "</small>" + Environment.NewLine +
									 "</div>" + Environment.NewLine +
									 "<div class=\"col-sm-3\">" + Environment.NewLine +
										 "<div class=\"project-label\">PROGRESS</div>" + Environment.NewLine +
										 "<div class=\"progress m-t-xs full progress-small\">" + Environment.NewLine +
											 "<div style = \"width: 12%\" aria-valuemax=\"100\" aria-valuemin=\"0\" aria-valuenow=\"12\" role=\"progressbar\" class=\" progress-bar progress-bar-success\">" + Environment.NewLine +
											 "</div>" + Environment.NewLine +
										 "</div>" + Environment.NewLine +
									 "</div>" + Environment.NewLine +
								 "</div>" + Environment.NewLine +
							 "</div>" + Environment.NewLine +
							 "<div class=\"col-sm-4 project-info\">" + Environment.NewLine +
								 "<div class=\"project-value\">" + Environment.NewLine +
									 donationButton +
									 volunteerButton +
								 "</div>" + Environment.NewLine +
								 "<div class=\"project-people\">" + Environment.NewLine +
									 volunteerList +
								 "</div>" + Environment.NewLine +
							 "</div>" + Environment.NewLine +
						 "</div>" + Environment.NewLine +
					 "</div>" + Environment.NewLine +
					 "<div class=\"panel-footer\">" + Environment.NewLine +
						 adminButtons +
					 "</div>" + Environment.NewLine +
				 "</div></div>" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;

			litEvents.Text = nonProfitCampaignString;

		}
	}
}