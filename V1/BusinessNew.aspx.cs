using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_BusinessNew : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!User.Identity.IsAuthenticated)
		{
			Response.Redirect("/Register");
		}

		this.Master.PageTitle = "Stability - Add New Business";
		this.Master.PageDescription = "Add a new Stability Business";
		this.Master.FbDescription = "Add a new Stability Business";
		this.Master.FbImage = "V1/Images/HMichael.png";
		this.Master.FbImageType = "image/jpg";
		this.Master.FbSite_name = "Stability - Add New Business";
		this.Master.FbURL = Request.Url.AbsoluteUri;

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



			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var skills = from c in dc.Skills
						 orderby c.Name
						 select new { name = " - " + c.Name + " ", c.SkillId };

			chkBoxListSkills.DataSource = skills;
			chkBoxListSkills.DataBind();

			//For editing business skills
			//var userSkills = from us in dc.UserSkills
			//				 where us.UserId == userId
			//				 select us;

			////Preselect the orgs for this user
			//if (userSkills.Count() > 0)
			//{
			//	foreach (var userEvent in userSkills)
			//	{
			//		for (int i = 0; i < chkBoxListSkills.Items.Count; i++)
			//		{
			//			if (userEvent.SkillId.ToString() == chkBoxListSkills.Items[i].Value)
			//			{
			//				chkBoxListSkills.Items[i].Selected = true;
			//			}
			//		}
			//	}
			//}
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string businessName		= txtParentOrganization.Value;
		string description		= txtDescription.Value;
		string address			= txtAddress.Value;
		string city				= txtCity.Value;
		string state			= ddlState.SelectedValue;
		string zip				= txtZipCode.Value;
		string donationLink		= txtDonationLink.Value;
		string primaryPhone		= txtPrimaryPhonenumber.Value;
		string secondaryPhone	= txtSecondaryPhoneNumber.Value;
		string website			= txtWebsite.Value;
		string ein				= txtEIN.Value;
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
		
		Business business = new Business();
		Guid businessId = Guid.NewGuid();
		business.Address = address;
		business.City = city;
		business.CreatedBy = userId;
		business.CreatedOn = DateTime.Now;
		business.Description = description;
		business.FacebookURL = string.IsNullOrEmpty(facebook) ? null : facebook;
		business.FacebookGroupURL = string.IsNullOrEmpty(facebookGroup) ? null : facebookGroup;
		business.InstagramURL = string.IsNullOrEmpty(instagram) ? null : instagram;
		business.IsActive = true;
		business.OwnerId = userId;
		business.Name = businessName;
		business.BusinessId = businessId;
		business.PointOfContactEmail = POCEmail;
		business.PointOfContactName = POCName;
		business.PointOfContactPhoneNumber = POCPhone;
		business.PrimaryPhone = primaryPhone;
		business.PublicEmail = string.IsNullOrEmpty(publicEmail) ? null : publicEmail;
		business.PublicPhoneNumber = string.IsNullOrEmpty(publicPhone) ? null : publicPhone;
		business.PurposeMission = purposeMission;
		business.SecondaryPhone = string.IsNullOrEmpty(secondaryPhone) ? null : secondaryPhone;
		business.State = state;
		business.TwitterURL = string.IsNullOrEmpty(twitter) ? null : twitter;
		business.Website = string.IsNullOrEmpty(website) ? null : website;
		business.BlogURL = string.IsNullOrEmpty(blogURL) ? null : blogURL;
		business.DonationURL = string.IsNullOrEmpty(donationLink) ? null : donationLink;
		business.YearFounded = yearFounded;
		business.YouTubeURL = string.IsNullOrEmpty(youTube) ? null : youTube;
		business.Zip = zip;
		business.EIN = ein;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		dc.Businesses.InsertOnSubmit(business);
		dc.SubmitChanges();

		foreach (ListItem item in chkBoxListSkills.Items)
		{
			if (item.Selected)
			{
				BusinessSkill businessSkill = new BusinessSkill();
				businessSkill.SkillId = new Guid(item.Value);
				businessSkill.BusinessId = businessId;
				businessSkill.BusinessSkillId = Guid.NewGuid();
				dc.BusinessSkills.InsertOnSubmit(businessSkill);
				dc.SubmitChanges();
			}
		}

		Response.Redirect(Request.QueryString["eventName"] + "/Business");
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect(Request.QueryString["eventName"] + "/Business");
	}
}