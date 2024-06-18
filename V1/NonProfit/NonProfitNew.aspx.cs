using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Administration_NonProfitNew : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			ListItemCollection statesList = new ListItemCollection();
			foreach (string state in States.Names())
			{
				ListItem li = new ListItem(state, state);
				statesList.Add(li);
			}

			ddlState.DataSource = statesList;
			ddlState.DataBind();
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string organizationName = txtParentOrganization.Value;
		string description		= txtDescription.Value;
		string address			= txtAddress.Value;
		string city				= txtCity.Value;
		string state			= ddlState.SelectedValue;
		string zip				= txtZipCode.Value;
		string donationLink		= txtDonationLink.Value;
		string primaryPhone		= txtPrimaryPhonenumber.Value;
		string secondaryPhone	= txtSecondaryPhoneNumber.Value;
		string website			= txtWebsite.Value;
		bool status501c3		= chk501c3Status.Checked;
		string ein				= txtEIN.Value;
		bool isVoad				= chkVoad.Checked;
		string POCName			= txtPOCFullname.Value;
		string POCEmail			= txtPOCEmailAddress.Value;
		string POCPhone			= txtPOCPhoneNumber.Value;
		string publicPhone		= txtPublicPhoneNumber.Value;
		string publicEmail		= txtPublicEmailAddress.Value;
		string purposeMission	= txtPurposeMission.Value;
		string facebook			= txtFacebook.Value;
		string facebookGroup	= txtFacebookGroup.Value;
		string instagram		= txtInstagram.Value;
		string youTube			= txtYouTube.Value;
		string twitter			= txtTwitter.Value;
		string blogURL			= txtblogURL.Value;
		string yearFounded		= txtYearFounded.Value;
		string urlFriendlyName	= txtURLFriendlyName.Value;
		Guid organizationId		= Guid.NewGuid();
		
		Organization organization = new Organization();

		organization.Address = address;
		organization.City = city;
		organization.CreatedBy = userId;
		organization.CreatedOn = DateTime.Now;
		organization.Description = description;
		organization.FacebookURL = string.IsNullOrEmpty(facebook) ? null : facebook;
		organization.URLFriendlyName = urlFriendlyName;
		organization.FacebookGroupURL = string.IsNullOrEmpty(facebookGroup) ? null : facebookGroup;
		organization.InstagramURL = string.IsNullOrEmpty(instagram) ? null : instagram;
		organization.IsActive = true;
		organization.OwnerId = userId;
		organization.IsVoadMember = isVoad;
		organization.Name = organizationName;
		organization.OrganizationId = organizationId;
		organization.PointOfContactEmail = POCEmail;
		organization.PointOfContactName = POCName;
		organization.PointOfContactPhoneNumber = POCPhone;
		organization.PrimaryPhone = primaryPhone;
		organization.PublicEmail = string.IsNullOrEmpty(publicEmail) ? null : publicEmail;
		organization.PublicPhoneNumber = string.IsNullOrEmpty(publicPhone) ? null : publicPhone;
		organization.PurposeMission = purposeMission;
		organization.SecondaryPhone = string.IsNullOrEmpty(secondaryPhone) ? null : secondaryPhone;
		organization.State = state;
		organization.TwitterURL = string.IsNullOrEmpty(twitter) ? null : twitter;
		organization.Website = string.IsNullOrEmpty(website) ? null : website;
		organization.BlogURL = string.IsNullOrEmpty(blogURL) ? null : blogURL;
		organization.DonationURL = string.IsNullOrEmpty(donationLink) ? null : donationLink;
		organization.YearFounded = yearFounded;
		organization.YouTubeURL = string.IsNullOrEmpty(youTube) ? null : youTube;
		organization.Zip = zip;
		organization._501c3Status = status501c3;
		organization.EIN = ein;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		dc.Organizations.InsertOnSubmit(organization);
		dc.SubmitChanges();

		UserOrganization userOrganization = new UserOrganization();
		userOrganization.UserOrganizationId = Guid.NewGuid();
		userOrganization.OrganizationId = organizationId;
		userOrganization.UserId = userId;
		userOrganization.IsPrimary = true;
		dc.UserOrganizations.InsertOnSubmit(userOrganization);
		dc.SubmitChanges();

		Roles.AddUserToRole(User.Identity.Name, "NonProfitAdministrator");

		Response.Redirect("/V1/NonProfitAdministration/InviteTeam.aspx?userActionModal=false&organizationId=" + organizationId);

	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Cajun-Navy-Page-Register.aspx");
	}
}