using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_TeamRoles : BaseWebForm
{
	public string _logo;
	public string _teamName;
	public string _teamSquareLogo;
	public string _teamDescription;
	public string _pageName;
	public string _organizationId;
	public string _nonProfitDropDown;
	public string _coverImage;
	public string organizationId = string.Empty;
	public string jsonEvents = string.Empty;
	public string availableDates = string.Empty;
	public string teamCounts = string.Empty;
	public string programId = string.Empty;


	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamFooter.PageName = "teamRolesPage";
		ucTeamHeader.PageName = "";

		#region HEADER PROPERTIES
		////////////////////////
		//BEGIN HEADER PROPERTIES
		////////////////////////

		organizationId	= Request.QueryString["organizationId"];
		programId		= Request.QueryString["programId"];
		
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "businesscoverimage.png";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName }).SingleOrDefault();

		string squareLogo = string.Empty;
		if (organization != null)
		{
			ucTeamHeader.CoverImage = _coverImage;

			if (!String.IsNullOrEmpty(organization.LogoSquare))
			{
				squareLogo = "/Impactoid/Images/Logos/" + organization.LogoSquare;
			}
			else
			{
				squareLogo = "/V1/Images/Logo-Placeholder.png";
			}

			Master.PageTitle = organization.Name + " Programs on Stability";
			Master.PageDescription = organization.Description;
			Master.FbDescription = organization.Description;
			Master.FbImage = _coverImage;
			Master.FbSite_name = organization.Name + " Programs on Stability";
			ucTeamHeader.URLFriendlyPageName = organization.URLFriendlyName;
		}

		ucTeamFooter.TeamName = organization.Name;
		ucTeamFooter.OrganizationId = organizationId;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.TeamLogo = squareLogo;
		Master.FbImageType = "image/jpg";
		Master.FbURL = Request.Url.AbsoluteUri;

		bool isOwner = false;
		#endregion


		
        hypInviteTeam.NavigateUrl = "/V1/NonProfitAdministration/InviteTeam.aspx?organizationId=" + organizationId;
        hypDonationDashboard.NavigateUrl = "/V1/NonProfit/DonationDashboard.aspx?organizationId=" + organizationId;
        hypInvitedMembers.NavigateUrl = "/V1/NonProfit/InvitedMembers.aspx?organizationId=" + organizationId;
        hypSquareLogoUpload.NavigateUrl = "/V1/NonProfit/SquareLogoUpload.aspx?organizationId=" + organizationId;
        hypLogoUpload.NavigateUrl = "/V1/NonProfit/LogoUpload.aspx?organizationId=" + organizationId;
        hypCoverImageUpload.NavigateUrl = "/V1/NonProfitAdministration/CoverImage1600x600.aspx?organizationId=" + organizationId;
        hypManagePhotos.NavigateUrl = "/V1/NonProfitAdministration/ManagePhotos.aspx?organizationId=" + organizationId;
        hypMail.NavigateUrl = "/V1/NonProfit/People.aspx?organizationId=" + organizationId + "&type=email";
        hypSms.NavigateUrl = "/V1/NonProfit/People.aspx?organizationId=" + organizationId + "&type=sms";
        hypTickets.NavigateUrl = "/V1/NonProfitAdministration/Tickets.aspx?organizationId=" + organizationId;
        hypReports.NavigateUrl = "/V1/NonProfitAdministration/Reports.aspx?organizationId=" + organizationId;
        hypSettings.NavigateUrl = "/V1/NonProfitAdministration/Settings.aspx?organizationId=" + organizationId;
        hypUpdateTeamInfo.NavigateUrl = "/V1/NonProfit/NonProfitNew.aspx?userActionModal=false&organizationId=" + organizationId;

    }

	protected void btnChangePageStatus_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		bool updateActiveStatus = true;

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

		if (organization != null)
		{

			btnDeactivatePage.Text = " <div class='wrimagecard-topimage_header' style='background-color:  rgba(51, 105, 232, 0.1)'> <center><i class='fa fa-ban' style='color:#3369e8'> </i></center> </div><div class='wrimagecard-topimage_title'> <h4> De-activate This Team   <div class='pull-right badge' id='WrGridSystem'></div></h4> </div>   ";

			if (organization.IsActive == true)
			{
				updateActiveStatus = false;
				btnDeactivatePage.Text = " <div class='wrimagecard-topimage_header' style='background-color:  rgb(232 51 51 / 10%)'> <center><i class='fa fa-ban' style='color:#e83333'> </i></center> </div><div class='wrimagecard-topimage_title'> <h4>Re-activate This Team  <div class='pull-right badge' id='WrGridSystem'></div></h4> </div>   ";

			}

			organization.IsActive = updateActiveStatus;
			dc.SubmitChanges();
		}
	}
}