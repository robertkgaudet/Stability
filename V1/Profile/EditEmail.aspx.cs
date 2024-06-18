using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_EditEmail : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			MembershipUser userCheck = Membership.GetUser(User.Identity.Name);
			txtEmailAddress.Value = userCheck.Email;
		}
	}
	public string updateEmail(string email)
	{
		string message = string.Empty;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var userEmailCheck = (from u in dc.aspnet_Memberships
							   where u.Email.ToLower() == email.ToLower()
							   select u).Take(1).SingleOrDefault();

		if(userEmailCheck == null)
		{
			//email does not exist
			MembershipUser userCheck = Membership.GetUser(User.Identity.Name);
			userCheck.Email = txtEmailAddress.Value;
			Membership.UpdateUser(userCheck);
			
			divMessage.Attributes.Add("class", "alert alert-success");
			message = "Email has been changed.";
		}
		else
		{
			divMessage.Attributes.Add("class", "alert alert-danger");
			message = "This email already exists";
		}
		
		return message;
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		lblMessage.Text = updateEmail(txtEmailAddress.Value);
		divMessage.Visible = true;
	}
	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("Profile.aspx");
	}
}