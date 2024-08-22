using System;
using System.Collections.Generic;
using System.Linq;
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
		dc.SubmitChanges();
		divMessage.Visible = true;
		Response.Redirect("/V1/Member/Default.aspx");
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/Default.aspx");
	}
}