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
        public Guid addressId = Guid.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/V1/ChooseEvent.aspx");
        }

        this.Master.PageTitle = "Stability - Edit Your Profile";
        this.Master.PageDescription = "";
        this.Master.FbDescription = "";
        this.Master.FbImage = "Images/HurricaneMichael.jpg";
        this.Master.FbImageType = "image/jpg";
        this.Master.FbSite_name = "Stability - Edit Your Profile";
        this.Master.FbURL = Request.Url.AbsoluteUri;

        if (!IsPostBack)
        {
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            Guid userId = user != null && user.ProviderUserKey != null ? (Guid)user.ProviderUserKey : Guid.Empty;

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var profile = (from p in dc.Profiles
                           where p.UserId == userId
                           select p).SingleOrDefault();

            Address address = null;
            if (profile != null)
            {
                var profileAddress = (from pa in dc.ProfileAddresses
                                      where pa.ProfileId == profile.ProfileId
                                      select pa).FirstOrDefault();

                if (profileAddress != null)
                {
                    address = (from a in dc.Addresses
                               where a.AddressId == profileAddress.AddressId
                               select a).FirstOrDefault();

                    if (address != null)
                    {
                        txtAddress.Value = address.Address1;
                        txtCity.Value = address.City;
                        txtZipCode.Value = address.Zip;
                    }
                }
                if (address == null)
                {
                    txtAddress.Value = profile.Address;
                    txtCity.Value = profile.City;
                    txtZipCode.Value = profile.Zip;
                }

                txtDatesAvailable.Value = profile.DatesAvailable;
                txtFirstname.Value = profile.Firstname;
                txtLastname.Value = profile.Lastname;
                txtNumberOfDays.Value = profile.NumberOfDaysAvailable;
                txtPhonenumber.Value = profile.PhoneNumber;
                txtVolunteerDescription.Value = profile.Description;
                txtZello.Value = profile.ZelloName;
                txtTitle.Value = profile.Title;
                receiveSMS.Checked = profile.ReceiveSMSNotifications;
                receiveEmail.Checked = profile.ReceiveEmailNotifications;
            }

            ListItemCollection statesList = new ListItemCollection();
            foreach (string state in States.Names())
            {
                statesList.Add(new ListItem(state, state));
            }

            ddlState.DataSource = statesList;
            ddlState.DataBind();
            if (ddlState.SelectedValue == "")
            {
                if (address != null)
                    ddlState.SelectedValue = address.State;
                else 
                    ddlState.SelectedValue = profile.State;
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string addressData = hidAddressData.Value;
        string address = txtAddress.Value;
        string city = txtCity.Value;
        string state = ddlState.SelectedValue;
        string zip = txtZipCode.Value;
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        MembershipUser user = Membership.GetUser();
        Guid currentUserId = Guid.Empty;

        if (user != null && user.ProviderUserKey != null)
        {
            currentUserId = (Guid)user.ProviderUserKey;
        }
        var addressList = addressData.Split('|');
        var duplicateAddress = dc.Addresses.FirstOrDefault(f => f.GooglePlaceId == addressList[10]);
        if (duplicateAddress == null)
        {
            var duplicateCounty = dc.Counties.FirstOrDefault(f => f.Name == addressList[9]);
            var countyId = Guid.NewGuid();
            if (duplicateCounty == null)
            {
                var states = dc.USStates.FirstOrDefault(f => f.Code == state);
                County county = new County();
                county.CountyId = countyId;
                county.Name = addressList[9];
                county.StateId = states != null ? states.StatesId : new Guid();
                county.State = state;
                dc.Counties.InsertOnSubmit(county);
                dc.SubmitChanges();
            }
            else
            {
                countyId = duplicateCounty.CountyId;
            }

            var duplicateCity = dc.Cities.FirstOrDefault(f => f.City1 == city);
            var cityId = Guid.NewGuid();
            if (duplicateCity == null)
            {
                City cit = new City();
                cit.CityId = cityId;
                cit.City1 = city;
                cit.Code = addressList[14];
                dc.Cities.InsertOnSubmit(cit);
                dc.SubmitChanges();
            }
            else
            {
                cityId = duplicateCity.CityId;
            }

            Address newAddress = new Address();
            newAddress.AddressId = Guid.NewGuid();
            newAddress.GooglePlaceId = addressList[10];
            newAddress.FormattedAddress = addressList[11];
            newAddress.StreetNumber = addressList[3];
            newAddress.StreetName = addressList[4];
            newAddress.Address1 = address;
            newAddress.City = city;
            newAddress.State = state;
            newAddress.Zip = zip;
            newAddress.Country = addressList[7];
            newAddress.County = addressList[9];
            newAddress.Latitude = addressList[1];
            newAddress.Longitude = addressList[2];
            newAddress.IsActive = true;
            newAddress.CreatedOn = DateTime.Now;
            newAddress.CreatedBy = currentUserId;
            newAddress.CountyId = countyId;
            newAddress.CityId = cityId;
            newAddress.LocationType = addressList[13];
            dc.Addresses.InsertOnSubmit(newAddress);
            dc.SubmitChanges();

            addressId = newAddress.AddressId;
        }
        else
        {
            addressId = duplicateAddress.AddressId;
        }
        var profile = (from p in dc.Profiles
                       where p.UserId == userId
                       select p).SingleOrDefault();

        profile.Address = txtAddress.Value;
        profile.City = txtCity.Value;
        profile.DatesAvailable = txtDatesAvailable.Value;
        profile.Description = txtVolunteerDescription.Value;
        profile.Firstname = txtFirstname.Value;
        profile.Lastname = txtLastname.Value;
        profile.NumberOfDaysAvailable = txtNumberOfDays.Value;
        profile.PhoneNumber = txtPhonenumber.Value;
        profile.State = ddlState.SelectedValue;
        profile.ZelloName = txtZello.Value;
        profile.Zip = txtZipCode.Value;
        profile.Title = txtTitle.Value;
        profile.ReceiveSMSNotifications = receiveSMS.Checked;
        profile.ReceiveEmailNotifications = receiveEmail.Checked;
        dc.SubmitChanges();
        divMessage.Visible = true;
        Response.Redirect("/V1/Member/Default.aspx");
    }

    protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/Default.aspx");
	}
}