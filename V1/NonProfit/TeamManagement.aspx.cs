using Stripe;
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

        organizationId = Request.QueryString["organizationId"];
        programId = Request.QueryString["programId"];

        string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
        _coverImage = causePhotoFolder + "businesscoverimage.png";

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(organizationId)
                            select new { o.Name, o.LogoSquare, o.Description, o.DonationURL, o.Logo, o.CoverImage, o.URLFriendlyName,o.IsActive}).SingleOrDefault();

        if(organization.IsActive==true)
        {
            btnDeactivatePage.Text = "De-activate This Team ";
        }
        else
        {
            btnDeactivatePage.Text = "Re-activate This Team  ";

        }
        string squareLogo = "/V1/Images/Logo-Placeholder.png";
        if (organization != null)
        {
            bool userOrganizationOwner = dc.Organizations
          .Any(o => o.OrganizationId == new Guid(organizationId) && o.OwnerId == userId);
            if(userOrganizationOwner==true || User.IsInRole("Administrator"))
            {
                hypDonationDashboards.Visible = true;
                hypSettingss.Visible = true;
                hypUpdateTeamInfos.Visible = true;
                btnDeactivatePages.Visible = true;
                hiddenRequest.Visible = true;
            }
            ucTeamHeader.CoverImage = _coverImage;

            if (!string.IsNullOrEmpty(organization.LogoSquare))
            {
                string virtualPath_square = "/Impactoid/Images/Logos/" + organization.LogoSquare;
                string physicalPath_square = Server.MapPath(virtualPath_square);

                if (System.IO.File.Exists(physicalPath_square))
                {
                    squareLogo = virtualPath_square;
                }
            }

            Master.PageTitle = organization.Name + " Programs on Stability";
            Master.PageDescription = organization.Description;
            Master.FbDescription = organization.Description;
            Master.FbImage = _coverImage;
            Master.FbSite_name = organization.Name + " Programs on Stability";
            ucTeamHeader.URLFriendlyPageName = organization.URLFriendlyName;

			if (!string.IsNullOrEmpty(organization.CoverImage))
			{
				//Let's the user change the cover image.
				_coverImage = causePhotoFolder + organization.CoverImage;
			}

        }

		ucTeamHeader.CoverImage = _coverImage;
		ucTeamHeader._teamTitle = organization.Name;
        ucTeamHeader.TeamDescription = organization.Description;
        ucTeamFooter.OrganizationId = organizationId;
        ucTeamHeader.OrganizationId = organizationId;
        ucTeamHeader.TeamLogo = squareLogo;
        Master.FbImageType = "image/jpg";
        Master.FbURL = Request.Url.AbsoluteUri;

        bool isOwner = false;
        #endregion
        hypInviteTeam.NavigateUrl = "/V1/NonProfitAdministration/InviteTeam.aspx?organizationId=" + organizationId;
        hypInviteTeam.Attributes["data-toggle"] = "tooltip";
        hypInviteTeam.Attributes["title"] = "Go To InviteTeam";

        hypDonationDashboard.NavigateUrl = "/V1/NonProfit/DonationDashboard.aspx?organizationId=" + organizationId;
        hypDonationDashboard.Attributes["data-toggle"] = "tooltip";
        hypDonationDashboard.Attributes["title"] = "Go To DonationDashboard";

        hypInvitedMembers.NavigateUrl = "/V1/NonProfit/InvitedMembers.aspx?organizationId=" + organizationId;
        hypInvitedMembers.Attributes["data-toggle"] = "tooltip";
        hypInvitedMembers.Attributes["title"] = "Go To InvitedMembers";


        hypRequest.NavigateUrl = "/V1/NonProfit/ReceivedRequests.aspx?organizationId=" + organizationId;
        hypRequest.Attributes["data-toggle"] = "tooltip";
        hypRequest.Attributes["title"] = "View this team's Request";


        hypSquareLogoUpload.NavigateUrl = "/V1/NonProfit/SquareLogoUpload.aspx?organizationId=" + organizationId;
        hypSquareLogoUpload.Attributes["data-toggle"] = "tooltip";
        hypSquareLogoUpload.Attributes["title"] = "Go To SquareLogoUpload";

        hypLogoUpload.NavigateUrl = "/V1/NonProfit/LogoUpload.aspx?organizationId=" + organizationId;
        hypLogoUpload.Attributes["data-toggle"] = "tooltip";
        hypLogoUpload.Attributes["title"] = "Go To LogoUpload";

        hypCoverImageUpload.NavigateUrl = "/V1/NonProfitAdministration/CoverImage1600x600.aspx?organizationId=" + organizationId;
        hypCoverImageUpload.Attributes["data-toggle"] = "tooltip";
        hypCoverImageUpload.Attributes["title"] = "Go To CoverImage1600x600";

        hypManagePhotos.NavigateUrl = "/V1/NonProfitAdministration/ManagePhotos.aspx?organizationId=" + organizationId;
        hypManagePhotos.Attributes["data-toggle"] = "tooltip";
        hypManagePhotos.Attributes["title"] = "Go To ManagePhotos";

        hypMail.NavigateUrl = "/V1/NonProfit/People.aspx?organizationId=" + organizationId + "&type=email";
        hypMail.Attributes["data-toggle"] = "tooltip";
        hypMail.Attributes["title"] = "Go To People (Email)";

        hypSms.NavigateUrl = "/V1/NonProfit/People.aspx?organizationId=" + organizationId + "&type=sms";
        hypSms.Attributes["data-toggle"] = "tooltip";
        hypSms.Attributes["title"] = "Go To People (SMS)";

        hypTickets.NavigateUrl = "/V1/NonProfitAdministration/Tickets.aspx?organizationId=" + organizationId;
        hypTickets.Attributes["data-toggle"] = "tooltip";
        hypTickets.Attributes["title"] = "Go To Tickets";

        hypReports.NavigateUrl = "/V1/NonProfitAdministration/Reports.aspx?organizationId=" + organizationId;
        hypReports.Attributes["data-toggle"] = "tooltip";
        hypReports.Attributes["title"] = "Go To Reports";

        hypSettings.NavigateUrl = "/V1/NonProfitAdministration/Settings.aspx?organizationId=" + organizationId;
        hypSettings.Attributes["data-toggle"] = "tooltip";
        hypSettings.Attributes["title"] = "Go To Settings";

        hypUpdateTeamInfo.NavigateUrl = "/V1/NonProfit/NonProfitNew.aspx?userActionModal=false&organizationId=" + organizationId;
        hypUpdateTeamInfo.Attributes["data-toggle"] = "tooltip";
        hypUpdateTeamInfo.Attributes["title"] = "Go To NonProfitNew";

    }

    protected void btnChangePageStatus_Click(object sender, EventArgs e)
    {
        organizationId = Request.QueryString["organizationId"];
     
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        bool updateActiveStatus = true;

        var organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(organizationId)
                            select o).SingleOrDefault();

        if (organization != null)
        {

               btnDeactivatePage.Text = "De-activate This Team  ";

            if (organization.IsActive == true)
            {
                updateActiveStatus = false;
                btnDeactivatePage.Text = " Re-activate This Team ";

            }
           
            organization.IsActive = updateActiveStatus;
            dc.SubmitChanges();
        }
        Response.Redirect(Request.RawUrl);
    }
   
   
}