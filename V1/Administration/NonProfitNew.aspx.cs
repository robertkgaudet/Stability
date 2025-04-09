using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class V1_Administration_NonProfitNew : BaseOrganizationWebForm
{
	string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationId = Request.QueryString["organizationId"];
		if (!IsPostBack)
		{
			ListItemCollection statesList = new ListItemCollection();
			foreach (string state in States.Names())
			{
				ListItem li = new ListItem(state, state);
				statesList.Add(li);
			}

			ddlState.DataSource = statesList;
			ddlState.DataBind();

			if(!String.IsNullOrEmpty(organizationId))
			{
				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
				//Select the state for the org.
				var organization = (from o in dc.Organizations
								   where o.OrganizationId == new Guid(organizationId)
								   select o).SingleOrDefault();

				txtParentOrganization.Value = organization.Name;
				txtDescription.Value = organization.Description;
				txtAddress.Value = organization.Address;
				txtCity.Value = organization.City;
				txtZipCode.Value = organization.Zip;
				txtDonationLink.Value = organization.DonationURL;
				txtPrimaryPhonenumber.Value = organization.PrimaryPhone;
				txtSecondaryPhoneNumber.Value = organization.SecondaryPhone;
				txtWebsite.Value = organization.Website;
				chk501c3Status.Checked = organization._501c3Status == null ? false : (bool)organization._501c3Status;
				txtEIN.Value = organization.EIN;
				chkVoad.Checked = organization.IsVoadMember == null ? false : (bool)organization.IsVoadMember;
				txtPOCFullname.Value = organization.PointOfContactName;
				txtPOCEmailAddress.Value = organization.PointOfContactEmail;
				txtPOCPhoneNumber.Value = organization.PointOfContactPhoneNumber;
				txtPublicPhoneNumber.Value = organization.PublicPhoneNumber;
				txtPublicEmailAddress.Value = organization.PublicEmail;
				txtPurposeMission.Value = organization.PurposeMission;
				txtFacebook.Value = organization.FacebookURL;
				txtFacebookGroup.Value = organization.FacebookGroupURL;
				txtInstagram.Value = organization.InstagramURL;
				txtYouTube.Value = organization.YouTubeURL;
				txtTwitter.Value = organization.TwitterURL;
				txtblogURL.Value = organization.BlogURL;
				txtTikTok.Value = organization.TikTokURL;
				txtYearFounded.Value = organization.YearFounded;
				txtURLFriendlyName.Value = organization.URLFriendlyName;

				ListItem item = ddlState.Items.FindByValue(organization.State);
				if (item != null)
				{
					ddlState.SelectedValue = item.Value;
				}
			}
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		organizationId = Request.QueryString["organizationId"];
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
		string tikTokURL		= txtTikTok.Value;
		string yearFounded		= txtYearFounded.Value;
		string urlFriendlyName	= txtURLFriendlyName.Value;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (!String.IsNullOrEmpty(organizationId))
		{
			//Select the state for the org.
			Organization organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

			if(!String.IsNullOrEmpty(address))
				organization.Address = address;

			if (!String.IsNullOrEmpty(city))
				organization.City = city;

			if (!String.IsNullOrEmpty(zip))
				organization.Zip = zip;

			if (!String.IsNullOrEmpty(state))
				organization.State = state;

			organization.CreatedBy = userId;
			organization.CreatedOn = DateTime.Now;
			organization.Description = description;
			organization.FacebookURL = string.IsNullOrEmpty(facebook) ? null : facebook;
			organization.URLFriendlyName = urlFriendlyName;
			organization.FacebookGroupURL = string.IsNullOrEmpty(facebookGroup) ? null : facebookGroup;
			organization.InstagramURL = string.IsNullOrEmpty(instagram) ? null : instagram;
			organization.TikTokURL = string.IsNullOrEmpty(tikTokURL) ? null : tikTokURL;
			organization.IsActive = true;
			organization.IsVoadMember = isVoad;
			organization.Name = organizationName;
			organization.PointOfContactEmail = POCEmail;
			organization.PointOfContactName = POCName;
			organization.PointOfContactPhoneNumber = POCPhone;

			if (!String.IsNullOrEmpty(primaryPhone))
				organization.PrimaryPhone = primaryPhone;

			organization.PublicEmail = string.IsNullOrEmpty(publicEmail) ? null : publicEmail;
			organization.PublicPhoneNumber = string.IsNullOrEmpty(publicPhone) ? null : publicPhone;
			organization.PurposeMission = purposeMission;
			organization.SecondaryPhone = string.IsNullOrEmpty(secondaryPhone) ? null : secondaryPhone;
			organization.TwitterURL = string.IsNullOrEmpty(twitter) ? null : twitter;
			organization.Website = string.IsNullOrEmpty(website) ? null : website;
			organization.BlogURL = string.IsNullOrEmpty(blogURL) ? null : blogURL;
			organization.DonationURL = string.IsNullOrEmpty(donationLink) ? null : donationLink;
			organization.YearFounded = yearFounded;
			organization.YouTubeURL = string.IsNullOrEmpty(youTube) ? null : youTube;
			organization._501c3Status = status501c3;
			organization.EIN = ein;
			dc.SubmitChanges();

			Response.Redirect("/V1/NonProfit/Default.aspx??userActionModal=false&organizationId=" + organization.OrganizationId);
		}
		else
		{
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
			organization.TikTokURL = string.IsNullOrEmpty(tikTokURL) ? null : tikTokURL;
			organization.IsActive = true;
			organization.OwnerId = userId;
			organization.IsVoadMember = isVoad;
			organization.Name = organizationName;
			organization.OrganizationId = Guid.NewGuid();
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
			dc.Organizations.InsertOnSubmit(organization);
			dc.SubmitChanges();

			UserOrganization userOrganization = new UserOrganization();
			userOrganization.UserOrganizationId = Guid.NewGuid();
			userOrganization.OrganizationId = organization.OrganizationId;
			userOrganization.UserId = userId;
			userOrganization.IsPrimary = true;
			dc.UserOrganizations.InsertOnSubmit(userOrganization);
			dc.SubmitChanges();

			Response.Redirect("/V1/NonProfitAdministration/InviteTeam.aspx?userActionModal=false&organizationId=" + organization.OrganizationId);
		}

	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Administration/NonProfitList.aspx?userActionModal=false&");
	}
}