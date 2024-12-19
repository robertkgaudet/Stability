using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class V1_Administration_Resources_EditLocation : BaseOrganizationWebForm
{
	public string _userId;

	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.PageTitle			= "Stability - Add New Location";
		this.Master.PageDescription		= "";
		this.Master.FbDescription		= "";
		this.Master.FbImage				= "Images/HurricaneMichael.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbSite_name			= "Stability - Add New Location";
		this.Master.FbURL				= Request.Url.AbsoluteUri;
		_userId = userId.ToString();
		string addressId = Request.QueryString["addressId"];
		hypAddNewLocation.NavigateUrl = "/V1/Administration/Location/AddLocation.aspx";

		if (!IsPostBack)
		{
			if(!String.IsNullOrEmpty(addressId))
			{
				chkIsOnMap.Checked = true;
				chkActive.Checked = true;

				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

				var locationInformation = (from a in dc.Addresses
										  where a.AddressId == new Guid(addressId)
										  select a).SingleOrDefault();

				lblLocationAddress.InnerText = locationInformation.FormattedAddress;
			  
				var locationProfile = (from lp in dc.LocationProfiles
										where lp.AddressId == new Guid(addressId)
										select lp).SingleOrDefault();

				if(locationProfile != null)
				{
					txtCapacity.Value = locationProfile.Capacity.ToString();

					if(locationProfile.DateClosed != null)
						txtDateClosed.Value = locationProfile.DateClosed.Value.ToShortDateString();


					if (locationProfile.DateOpened != null)
						txtDateOpened.Value = locationProfile.DateOpened.Value.ToShortDateString();

					txtEmailAddress.Value = locationProfile.EmailAddress;
					txtGeneratorSize.Value = locationProfile.GeneratorSize;
					txtFacebookURL.Value = locationProfile.FacebookURL;
					txtInstagramUsername.Value = locationProfile.InstagramUsername;
					txtLocationDescription.Value = locationProfile.Description;
					txtLocationName.Value = locationProfile.Name;
					txtPhoneNumber.Value = locationProfile.PhoneNumber;
					txtPOCEmailAddress.Value = locationProfile.PointOfContactEmail;
					txtPOCPhoneNumber.Value = locationProfile.PointOfContactPhoneNumber;
					txtPointOfContactName.Value = locationProfile.PointOfContactName;
					txtTwitterUsername.Value = locationProfile.TwitterUsername;
					txtWebsiteURL.Value = locationProfile.WebsiteURL;
					txtDonationURL.Value = locationProfile.DonationURL;
					txtYouTubeURL.Value = locationProfile.YouTubeURL;
					chkHasGenerator.Checked = Convert.ToBoolean(locationProfile.HasGenerator);
					chkActive.Checked = locationProfile.IsActive;
					chkAllowsPets.Checked = locationProfile.AllowsPets;
					chkIsOnMap.Checked = locationProfile.IsOnMap;
					chkNeedsVolunteers.Checked = Convert.ToBoolean(locationProfile.SeekingVolunteers);
					chkMedicalHelpProvided.Checked = locationProfile.ProvidesMedicalHelp;
				}
			}
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		string addressId = Request.QueryString["addressId"];

		var duplicateLocationProfile = (from lp in dc.LocationProfiles
										where lp.AddressId == new Guid(addressId)
										select lp).SingleOrDefault();
		
		if(duplicateLocationProfile == null)
		{
			//Insert
			LocationProfile locationProfile = new LocationProfile();
			Guid locationProfileId = Guid.NewGuid();
			
			string editLocationRelationships = "/V1/Administration/Location/EditLocationRelationships.aspx?addressId=" + addressId + "&locationProfileId=" + locationProfileId;
			
			hypViewLocation.NavigateUrl = "/V1/Location.aspx?locationProfileId=" + locationProfileId;

			locationProfile.LocationProfileId = locationProfileId;
			locationProfile.AddressId = new Guid(addressId);
			locationProfile.AllowsPets = chkAllowsPets.Checked;
			locationProfile.IsActive = chkActive.Checked;
			locationProfile.SeekingVolunteers = chkNeedsVolunteers.Checked;
			locationProfile.IsOnMap = chkIsOnMap.Checked;
			locationProfile.HasGenerator = chkHasGenerator.Checked;
			locationProfile.ProvidesMedicalHelp = chkMedicalHelpProvided.Checked;
			locationProfile.Capacity = !String.IsNullOrEmpty(txtCapacity.Value) ? int.Parse(txtCapacity.Value) : 0;
			locationProfile.CreatedBy = userId;
			locationProfile.CreatedOn = DateTime.Now;

			if (!String.IsNullOrEmpty(txtGeneratorSize.Value))
				locationProfile.GeneratorSize = txtGeneratorSize.Value;

			if (!String.IsNullOrEmpty(txtDateClosed.Value))
				locationProfile.DateClosed = Convert.ToDateTime(txtDateClosed.Value);

			if (!String.IsNullOrEmpty(txtDateOpened.Value))
				locationProfile.DateClosed = Convert.ToDateTime(txtDateOpened.Value);

			if (!String.IsNullOrEmpty(txtLocationDescription.Value))
				locationProfile.Description = txtLocationDescription.Value;

			if (!String.IsNullOrEmpty(txtEmailAddress.Value))
				locationProfile.EmailAddress = txtEmailAddress.Value;

			if (!String.IsNullOrEmpty(txtFacebookURL.Value))
				locationProfile.FacebookURL = txtFacebookURL.Value;

			if (!String.IsNullOrEmpty(txtInstagramUsername.Value))
				locationProfile.InstagramUsername = txtInstagramUsername.Value;

			if (!String.IsNullOrEmpty(txtLocationName.Value))
				locationProfile.Name = txtLocationName.Value;

			if (!String.IsNullOrEmpty(txtPhoneNumber.Value))
				locationProfile.PhoneNumber = txtPhoneNumber.Value;

			if (!String.IsNullOrEmpty(txtPOCEmailAddress.Value))
				locationProfile.PointOfContactEmail = txtPOCEmailAddress.Value;

			if (!String.IsNullOrEmpty(txtPointOfContactName.Value))
				locationProfile.PointOfContactName = txtPointOfContactName.Value;

			if (!String.IsNullOrEmpty(txtPOCPhoneNumber.Value))
				locationProfile.PointOfContactPhoneNumber = txtPOCPhoneNumber.Value;

			if (!String.IsNullOrEmpty(txtTwitterUsername.Value))
				locationProfile.TwitterUsername = txtTwitterUsername.Value;

			if (!String.IsNullOrEmpty(txtWebsiteURL.Value))
				locationProfile.WebsiteURL = txtWebsiteURL.Value;

			if (!String.IsNullOrEmpty(txtDonationURL.Value))
				locationProfile.DonationURL = txtDonationURL.Value;

			if (!String.IsNullOrEmpty(txtYouTubeURL.Value))
				locationProfile.YouTubeURL = txtYouTubeURL.Value;
	
			dc.LocationProfiles.InsertOnSubmit(locationProfile);
			dc.SubmitChanges();

			Response.Redirect(editLocationRelationships);
		}
		else
		{
			//Update
			hypViewLocation.NavigateUrl = "/V1/Location.aspx?locationProfileId=" + duplicateLocationProfile.LocationProfileId;
			duplicateLocationProfile.AllowsPets = chkAllowsPets.Checked;
			duplicateLocationProfile.IsActive = chkActive.Checked;
			duplicateLocationProfile.IsOnMap = chkIsOnMap.Checked;
			duplicateLocationProfile.HasGenerator = chkHasGenerator.Checked;
			duplicateLocationProfile.SeekingVolunteers = (bool)chkNeedsVolunteers.Checked;
			duplicateLocationProfile.ProvidesMedicalHelp = chkMedicalHelpProvided.Checked;
			duplicateLocationProfile.Capacity = !String.IsNullOrEmpty(txtCapacity.Value) ? int.Parse(txtCapacity.Value) : 0;
			duplicateLocationProfile.UpdatedBy = userId;
			duplicateLocationProfile.UpdatedOn = DateTime.Now;

			if (!String.IsNullOrEmpty(txtGeneratorSize.Value))
				duplicateLocationProfile.GeneratorSize = txtGeneratorSize.Value;

			if (!String.IsNullOrEmpty(txtDateClosed.Value))
				duplicateLocationProfile.DateClosed = Convert.ToDateTime(txtDateClosed.Value);
			else
				duplicateLocationProfile.DateClosed = null;

			if (!String.IsNullOrEmpty(txtDateOpened.Value))
				duplicateLocationProfile.DateOpened = Convert.ToDateTime(txtDateOpened.Value);
			else
				duplicateLocationProfile.DateOpened = null;

			if (!String.IsNullOrEmpty(txtLocationDescription.Value))
				duplicateLocationProfile.Description = txtLocationDescription.Value;
			else
				duplicateLocationProfile.Description = null;

			if (!String.IsNullOrEmpty(txtEmailAddress.Value))
				duplicateLocationProfile.EmailAddress = txtEmailAddress.Value;
			else
				duplicateLocationProfile.EmailAddress = null;

			if (!String.IsNullOrEmpty(txtFacebookURL.Value))
				duplicateLocationProfile.FacebookURL = txtFacebookURL.Value;
			else
				duplicateLocationProfile.FacebookURL = null;

			if (!String.IsNullOrEmpty(txtInstagramUsername.Value))
				duplicateLocationProfile.InstagramUsername = txtInstagramUsername.Value;
			else
				duplicateLocationProfile.InstagramUsername = null;

			if (!String.IsNullOrEmpty(txtLocationName.Value))
				duplicateLocationProfile.Name = txtLocationName.Value;
			else
				duplicateLocationProfile.Name = null;

			if (!String.IsNullOrEmpty(txtPhoneNumber.Value))
				duplicateLocationProfile.PhoneNumber = txtPhoneNumber.Value;
			else
				duplicateLocationProfile.PhoneNumber = null;

			if (!String.IsNullOrEmpty(txtPOCEmailAddress.Value))
				duplicateLocationProfile.PointOfContactEmail = txtPOCEmailAddress.Value;
			else
				duplicateLocationProfile.PointOfContactEmail = null;

			if (!String.IsNullOrEmpty(txtPointOfContactName.Value))
				duplicateLocationProfile.PointOfContactName = txtPointOfContactName.Value;
			else
				duplicateLocationProfile.PointOfContactName = null;

			if (!String.IsNullOrEmpty(txtPOCPhoneNumber.Value))
				duplicateLocationProfile.PointOfContactPhoneNumber = txtPOCPhoneNumber.Value;
			else
				duplicateLocationProfile.PointOfContactPhoneNumber = null;

			if (!String.IsNullOrEmpty(txtTwitterUsername.Value))
				duplicateLocationProfile.TwitterUsername = txtTwitterUsername.Value;
			else
				duplicateLocationProfile.TwitterUsername = null;

			if (!String.IsNullOrEmpty(txtWebsiteURL.Value))
				duplicateLocationProfile.WebsiteURL = txtWebsiteURL.Value;
			else
				duplicateLocationProfile.WebsiteURL = null;

			if (!String.IsNullOrEmpty(txtDonationURL.Value))
				duplicateLocationProfile.DonationURL = txtDonationURL.Value;
			else
				duplicateLocationProfile.DonationURL = null;


			if (!String.IsNullOrEmpty(txtYouTubeURL.Value))
				duplicateLocationProfile.YouTubeURL = txtYouTubeURL.Value;
			else
				duplicateLocationProfile.YouTubeURL = null;

			dc.SubmitChanges();
		}

		divMessage.Visible = true;
		lblMessage.Text = "Your location profile has been updated. ";
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Administration/LocationList.aspx");
	}
}