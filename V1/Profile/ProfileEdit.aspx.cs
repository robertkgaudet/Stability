using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_ProfileEdit : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!User.Identity.IsAuthenticated)
		{
			Response.Redirect("~/V1/ChooseEvent.aspx");
		}

		this.Master.PageTitle			= "Stability - Edit Your Profile";
		this.Master.PageDescription		= "";
		this.Master.FbDescription		= "";
		this.Master.FbImage				= "Images/HurricaneMichael.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbSite_name			= "Stability - Edit Your Profile";
		this.Master.FbURL				= Request.Url.AbsoluteUri;

		if(!IsPostBack)
		{
			MembershipUser user = Membership.GetUser(User.Identity.Name);

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var profile = (from p in dc.Profiles
						  where p.UserId == userId
						  select p).SingleOrDefault();

			txtAddress.Value = profile.Address;
			txtCity.Value = profile.City;
			txtDatesAvailable.Value = profile.DatesAvailable;
			txtFirstname.Value = profile.Firstname;
			txtLastname.Value = profile.Lastname;
			txtNumberOfDays.Value = profile.NumberOfDaysAvailable;
			txtPhonenumber.Value = profile.PhoneNumber;
			txtVolunteerDescription.Value = profile.Description;
			txtZello.Value = profile.ZelloName;
			txtZipCode.Value = profile.Zip;
			txtTitle.Value = profile.Title;
			receiveSMS.Checked = profile.ReceiveSMSNotifications;
			receiveEmail.Checked = profile.ReceiveEmailNotifications;


            ListItemCollection statesList = new ListItemCollection();
			foreach (string state in States.Names())
			{
				ListItem li = new ListItem(state, state);
				statesList.Add(li);
			}

			ddlState.DataSource = statesList;
			ddlState.DataBind();

			ddlState.SelectedValue = profile.State;
		}
	}
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var  addressData = hidAddressData.Value;
        var profile = (from p in dc.Profiles
                       where p.UserId == userId
                       select p).SingleOrDefault();

        if (profile == null)
        {
            
            return;
        }

        var addressList = addressData.Split('|');
        for (int i = 0; i < addressList.Length; i++)
        {
            addressList[i] = addressList[i].Trim();
        }

        // Build new Address object directly from form fields
        Address newAddress = new Address();
        newAddress.AddressId = Guid.NewGuid();
        newAddress.GooglePlaceId = addressList[10];
        newAddress.FormattedAddress = addressList[11];
        newAddress.StreetNumber = addressList[3];
        newAddress.StreetName = addressList[4];
        newAddress.Address1 = txtAddress.Value.Trim();
        newAddress.City = txtCity.Value.Trim();
        newAddress.State = ddlState.SelectedValue;
        newAddress.Zip = txtZipCode.Value.Trim();
        newAddress.Country = addressList[7];
        newAddress.County = addressList[9];
        newAddress.Latitude = addressList[1];
        newAddress.Longitude = addressList[2];
        newAddress.IsActive = true;
        newAddress.CreatedOn = DateTime.Now;
        newAddress.CreatedBy = userId;
        //newAddress.CountyId = countyId;
        //newAddress.CityId = cityId;
        newAddress.LocationType = addressList[13];


        dc.Addresses.InsertOnSubmit(newAddress);
        dc.SubmitChanges();

        // Update Profile table

        profile.Address = txtAddress.Value.Trim();
        profile.City = txtCity.Value.Trim();
        profile.DatesAvailable = txtDatesAvailable.Value.Trim();
        profile.Description = txtVolunteerDescription.Value.Trim();
        profile.Firstname = txtFirstname.Value.Trim();
        profile.Lastname = txtLastname.Value.Trim();
        profile.NumberOfDaysAvailable = txtNumberOfDays.Value.Trim();
        profile.PhoneNumber = txtPhonenumber.Value.Trim();
        profile.State = ddlState.SelectedValue;
        profile.ZelloName = txtZello.Value.Trim();
        profile.Zip = txtZipCode.Value.Trim();
        profile.Title = txtTitle.Value.Trim();
        profile.ReceiveSMSNotifications = receiveSMS.Checked;
        profile.ReceiveEmailNotifications = receiveEmail.Checked;

        // Mark all previous ProfileAddresses as IsPrimaryResidence = false
        var previousAddresses = dc.ProfileAddresses.Where(pa => pa.ProfileId == profile.ProfileId);
        foreach (var pa in previousAddresses)
        {
            pa.IsPrimaryResidence = false;
        }

        // Insert new ProfileAddress
        ProfileAddress newProfileAddress = new ProfileAddress();
        newProfileAddress.ProfileAddressId = Guid.NewGuid();
        newProfileAddress.ProfileId = profile.ProfileId;
        newProfileAddress.AddressId = newAddress.AddressId;
        newProfileAddress.HomeTypeId = new Guid("C478785A-014D-4DFF-86FE-3693E6F33FAC");
        newProfileAddress.HomeRelationshipOwnRentTypeId = new Guid("58526C73-5469-4B5E-81B1-831476784C56");
        newProfileAddress.IsPrimaryResidence = true;
        newProfileAddress.HasFloodInsurance = false;
        newProfileAddress.HasHomeownersInsurance = false;
        newProfileAddress.ShowOnAgencyMap = false;
        newProfileAddress.ShowOnCleanupMap = false;
        newProfileAddress.IsMultistory = false;
        newProfileAddress.HasBasement = false;
        newProfileAddress.HasGarage = false;
        newProfileAddress.HasCarport = false;
        newProfileAddress.HasCrawlspace = false;

        dc.ProfileAddresses.InsertOnSubmit(newProfileAddress);

        // Save everything
        dc.SubmitChanges();

        // Success message + redirect
        divMessage.Visible = true;
        Response.Redirect("/V1/Member/Default.aspx");
    }

    protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/Default.aspx");
	}
}