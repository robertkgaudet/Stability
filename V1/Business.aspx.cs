using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Business : BaseOrganizationWebForm
{
	public string businessPhotoFolder = System.Configuration.ConfigurationManager.AppSettings["businessPhotoFolder"].ToString();
	public string eventName = HttpContext.Current.Request.QueryString["eventName"];
	public Guid eventId = Guid.Empty;
	public string registerBusiness = "/V1/BusinessNew.aspx?eventName=" + HttpContext.Current.Request.QueryString["eventName"];
	public string editCampaign = string.Empty;
		
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		string businessPageTitle = System.Configuration.ConfigurationManager.AppSettings["BusinessPageTitle"];

		if (!IsPostBack)
		{
			var disaster = (from ev in dc.Events
							where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
							select ev).SingleOrDefault();

			if (disaster != null)
			{
				string pageDescription = "Businesses are invited to create a response to " + disaster.Name + ". This will allow you to collaborate with  non-profits, find volunteers, assist with home recovery, manage resources and much more. <br> Responding to this disaster requires two steps. <ol><li>Add your organization to the Stability system.</li> <li>Then create a business response to " + disaster.Name + ".</li></ol>";
				string pageTitle = disaster.Name + businessPageTitle;

				this.Master.PageTitle = "Stability - " + pageTitle;
				this.Master.PageDescription = "Stability - " + pageDescription;
				this.Master.FbDescription = "Stability - " + pageDescription;
				this.Master.FbImage = "/V1/Images/" + disaster.ImageFileName;
				this.Master.FbImageType = "image/jpg";
				this.Master.FbSite_name = "Stability - " + pageTitle;
				this.Master.FbURL = Request.Url.AbsoluteUri;

				uc1EventHeader.PageTitle = businessPageTitle;
				uc1EventHeader.EventName = disaster.Name;
				uc1EventHeader.PageDescription = pageDescription;
				eventId = disaster.EventId;
			}

			btnRegisterBusiness.Text = "Register My Business";
			litAddBusinessEvent.Text = "Businesss must register with Stability, to be eligible to create a response to this disaster.";
			//Does somene have a non-profit setup in Stability?
			var business = from np in dc.Businesses
							where np.OwnerId == userId
							select np;

			hypNewBusinessCampaign.Visible = false;
			hypNewBusiness.Visible = false;
			if (business.Count() > 0)
			{
				hypNewBusinessCampaign.Visible = true;
				hypNewBusinessCampaign.Font.Underline = true;
				hypNewBusiness.NavigateUrl = "/V1/BusinessNew.aspx?eventName=" + HttpContext.Current.Request.QueryString["eventName"];
				if (business.Count() > 1)
				{
					hypNewBusinessCampaign.NavigateUrl = "/V1/BusinessAdministration/RespondToEvent.aspx?eventId=" + eventId;
				}
				else
				{
					hypNewBusinessCampaign.NavigateUrl = "/V1/BusinessAdministration/RespondToEvent.aspx?businessId=" + business.SingleOrDefault().BusinessId + "&eventId=" + eventId;
				}
				//Create a campaign
				btnRegisterBusiness.Text = "Create Disaster Campaign";
				btnRegisterBusiness.Visible = true;
				hypNewBusiness.Visible = true;
				hypNewBusiness.Font.Underline = true;	

				litRegisterBusiness.Visible = false;

				divAddBusiness.Visible = false;

				litBusinessName.Text = "<h3>" + business.Take(1).SingleOrDefault().Name + "</h3>";
				litAddBusinessEvent.Text = " Your business, <b>" + business.Take(1).SingleOrDefault().Name + "</b>, is eligible to create a Campaign for responding to " + disaster.Name + ".";
				litAddBusinessEvent.Visible = true;

				registerBusiness = "/V1/BusinessAdministration/RespondToEvent.aspx?businessId=" + business.Take(1).SingleOrDefault().BusinessId + "&eventId=" + eventId;
			}

			var BusinessCampaign = from npe in dc.BusinessEvents
									join b in dc.Businesses on npe.BusinessId equals b.BusinessId
									where npe.EventId == eventId || b.GlobalCampaigns == true
									select npe;

			if (BusinessCampaign.Count() > 0)
			{
				//List the business that are responding to this event.
				LoadBusinesss(eventId);

				//Does this user have a campaign?
				var BusinessCampaignUser = from be in dc.BusinessEvents
											where be.EventId == eventId && be.Createdby == userId
											select be;

				if (BusinessCampaignUser.Count() > 0)
				{
					//They already have a campaign response.
					btnRegisterBusiness.Visible = false;
					litRegisterBusiness.Visible = false;
					litAddBusinessEvent.Visible = false;
					divAddBusiness.Visible = false;
				}
				else
				{
					btnRegisterBusiness.Visible = true;
					litRegisterBusiness.Visible = true;
					litAddBusinessEvent.Visible = true;
				}
			}
		}
	}

	protected void LoadBusinesss(Guid eventId)
	{
		string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var businessCampaigns = from be in dc.BusinessEvents
								 join b in dc.Businesses on be.BusinessId equals b.BusinessId
								 where be.EventId == eventId || b.GlobalCampaigns == true
								 orderby be.CreatedOn descending
								 select new { be, b };

		string businessCampaignString = string.Empty;
		foreach (var businessCampaign in businessCampaigns)
		{

			string adminButtons = string.Empty;
			string volunteerButton = string.Empty;
			string donationButton = string.Empty;
			string volunteerList = string.Empty;
			string businessLogo = string.Empty;

			var volunteers = from uo in dc.BusinessUsers
							 join p in dc.Profiles on uo.UserId equals p.UserId
							 where uo.BusinessId == businessCampaign.b.BusinessId
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


			if (!String.IsNullOrEmpty(businessCampaign.be.DonationURL))
			{
				donationButton =
									"<a href=\"" + businessCampaign.be.DonationURL + "\" class=\"btn btn-xs btn-success m-t-sm\" target=\"_blank\">" + Environment.NewLine +
										"Donate" + Environment.NewLine +
									"</a>" + Environment.NewLine;
			}

			if ((bool)businessCampaign.be.AcceptsVolunteers)
			{
				volunteerButton = "<a href=\"/V1/Profile/Volunteer.aspx?eventId=" + businessCampaign.be.EventId + "\" class=\"btn btn-xs btn-success m-l-xs m-t-sm\">" + Environment.NewLine +
											"Volunteer" + Environment.NewLine +
										"</a>" + Environment.NewLine;
			}

			if (userId == businessCampaign.b.OwnerId)
			{
				//adminButtons = "<div class=\"project-action pull-right\">" + Environment.NewLine +
								//	"<div class=\"btn-group\">" + Environment.NewLine +
								//		"<button class=\"btn btn-xs btn-warning editCampaign\"> Edit</button>" + Environment.NewLine +
								//	"</div>" + Environment.NewLine +
								//"</div>" + Environment.NewLine;

				//editCampaign = "/V1/BusinessAdministration/EditBusinessCampaign.aspx?BusinessEventId=" + businessCampaign.be.BusinessEventId;
			}

			////Count the rebuilds this business is managing.

			//var rebuildCount = (from or in dc.OrganizationRebuilds
			//					join r in dc.Rebuilds on or.RebuildId equals r.RebuildId
			//					where or.OrganizationId == businessCampaign.b.BusinessId
			//					&& r.EventId == eventId
			//					select or).Count();

			var volunteerCount = (from ue in dc.UserEvents
								  join ur in dc.aspnet_UsersInRoles on ue.UserId equals ur.UserId
								  join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
								  join bu in dc.BusinessUsers on ue.UserId equals bu.UserId
								  where r.LoweredRoleName == "helper"
								&& ue.EventId == eventId
								&& bu.BusinessId == businessCampaign.b.BusinessId
								  select ue).Count();


			//var volunteerCount = (from p in dc.Profiles
			//					 join ur in dc.aspnet_UsersInRoles on p.UserId equals ur.UserId
			//					 join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
			//					 join uo in dc.UserOrganizations on p.UserId equals uo.UserId
			//					 join ue in dc.UserEvents on p.UserId equals ue.UserId
			//					 join oev in dc.OrganizationEvents on ue.EventId equals oev.EventId
			//					 where r.LoweredRoleName == "volunteer" || r.LoweredRoleName == "helper"
			//					 && oev.EventId == eventId
			//					 && uo.OrganizationId == BusinessCampaign.o.OrganizationId
			//					 select p).Count();

			var survivorCount = (from p in dc.Profiles
								 join ur in dc.aspnet_UsersInRoles on p.UserId equals ur.UserId
								 join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
								 join bu in dc.BusinessUsers on p.UserId equals bu.UserId
								 join ue in dc.UserEvents on p.UserId equals ue.UserId
								 where r.LoweredRoleName == "survivor"
								 && ue.EventId == eventId
								 && bu.BusinessId == businessCampaign.b.BusinessId
								 select p).Count();

			var businessProfilePhoto = (from bp in dc.BusinessPhotos
										join p in dc.Photos on bp.PhotoId equals p.PhotoId
										where bp.BusinessEventId == businessCampaign.be.BusinessEventId
										orderby p.CreatedOn descending
										select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			if (businessProfilePhoto != null)
			{
				businessLogo = "<img width=\"75\" class=\"m-b img-rounded pull-left m-r-md\" src=\"" + businessPhotoFolder + businessProfilePhoto.FilenameCropped + "\" runat=\"server\" id=\"imgBusinessProfile\" />";
			}


			//Count the community size.

			businessCampaignString +=
					 "<div class=\"grid-item m-t-lg\"><div class=\"hpanel hgreen\">" + Environment.NewLine +
					 "<div class=\"panel-body\">" + Environment.NewLine +
						 "<span class=\"label label-success pull-right\">NEW</span>" + Environment.NewLine +
						 "<div class=\"row\">" + Environment.NewLine +
							 "<div class=\"col-sm-8\">" + Environment.NewLine +
								businessLogo + Environment.NewLine +
								"<a href=\"/V1/Business/Business.aspx?businessId=" + businessCampaign.b.BusinessId + "\">" + businessCampaign.b.Name + "</a>" + Environment.NewLine +
								 "<h4><u><a href = \"/BusinessResponse/" + businessCampaign.be.URLFriendlyCampaignName + "\" >" + businessCampaign.be.CampaignName + "</a></u></h4>" + Environment.NewLine +
								 "<p>" + Environment.NewLine +
									 businessCampaign.be.MissionPurpose + Environment.NewLine +
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
									 //"<div class=\"col-sm-3\">" + Environment.NewLine +
										// "<div class=\"project-label\">HOMES</div>" + Environment.NewLine +
										// "<small>" + rebuildCount.ToString() + "</small>" + Environment.NewLine +
									 //"</div>" + Environment.NewLine +
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

			litEvents.Text = businessCampaignString;

		}
	}
}