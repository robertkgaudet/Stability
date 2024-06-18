using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_Default : BaseWebForm
{
	public string organizationId = string.Empty;
	public string volunteerLink = string.Empty;
	public string donateLink = string.Empty;
	public string impactoidLink = string.Empty;
	public string activityPageLink = string.Empty;
	public string nonProfitDropDown = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationId = Request.QueryString["organizationId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

		if (!User.Identity.IsAuthenticated)
		{
			if ((bool)!organization.IsActive)
			{
				Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=79305f85-3816-46a8-911f-0d7e3e227c32");
			}
		}

		Master.PageTitle = organization.Name + " Team Page on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = organization.CoverImage;
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " Team Page on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;

		ucTeamNavigation.PageName = "teamPage";
		ucTeamNavigation.TeamName = organization.Name;

		string logo = string.Empty;
		if (!String.IsNullOrEmpty(organization.Logo))
		{
			logo = "/Impactoid/Images/Logos/" + organization.Logo;
		}
		else
		{
			//Use placeholder image.imgLogo.Visible = true;
			logo = "/V1/Images/Logo-Placeholder.png";
		}

		ucTeamNavigation.TeamName = organization.Name;
		ucTeamHeader.Logo = logo;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.PageName = "Home";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;

		litTeamName.Text = organization.Name;
		lblOrgName.Text = organization.Name;
		litMission.Text = organization.PurposeMission;
		litDescription.Text = organization.Description;
		litYearFounded.Text = organization.YearFounded;
		hypAddress.Text = organization.Address + "<br/>" + organization.City + ", " + organization.State + " " + organization.Zip;
		hypAddress.NavigateUrl = "http://maps.google.com/maps?q=" + organization.Address.Replace(" ", "+") + "," + organization.City.Replace(" ", "+") + "," + organization.State.Replace(" ", "+") + "," + organization.Zip;
		lblVoadMember.Text = organization.IsVoadMember.ToString();
		lbl501c3.Text = organization._501c3Status.ToString();

		bool isOwner = false;
		lbVolunteer.Visible = true;
		if (User.Identity.IsAuthenticated)
		{
			var userOrganizationOwner = (from uo in dc.UserOrganizations
										 join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
										 where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
										 && uo.OrganizationId == new Guid(organizationId)
										 select o).Take(1).SingleOrDefault();

			if (userOrganizationOwner != null)
			{
				if ((userOrganizationOwner.OwnerId != userId))
				{
					isOwner = true;
				}
			}

			//If the user is logged in and not in a nonprofit already then send to choose a nonprofit.
			var userOrganization = from uo in dc.UserOrganizations
								   where uo.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
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

		if (User.IsInRole("Administrator") || isOwner)
		{
			if (organization.IsActive != true)
			{
				btnDeactivatePage.Text = "Re-activate This Team";
			}

			btnInviteTeamMembers.Visible = true;
			divUnpublishedInformation.Visible = true;
			divUploadLogoCover.Visible = true;
			dtEIN.Visible = true;
			ddEIN.Visible = true;
			btnUploadLogo.Visible = true;
			btnUploadCoverImage.Visible = true;
			btnManagePhotos.Visible = true;
			btnEditMyGroup.Visible = true;
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
	}
	protected void btnChangePageStatus_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		bool updateActiveStatus = true;

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

		btnDeactivatePage.Text = "De-activate This Team";
		divAlertPageMessage.Visible = false;
		if (organization.IsActive == true)
		{
			updateActiveStatus = false;
			btnDeactivatePage.Text = "Re-activate This Team";
			divAlertPageMessage.Visible = true;
		}

		organization.IsActive = updateActiveStatus;
		dc.SubmitChanges();
	}
}