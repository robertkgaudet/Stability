using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_Username : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			txtUsername.Value = User.Identity.Name;
		}
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/Default.aspx");
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		lblMessage.Text = ChangeUsername(User.Identity.Name, txtUsername.Value);
		divMessage.Visible = true;
		Response.Redirect("/V1/Member/Default.aspx");
	}

	public string ChangeUsername(string oldUsername, string newUsername)
    {
		string result = string.Empty;
        if (string.IsNullOrWhiteSpace(oldUsername))
		{
			divMessage.Attributes.Add("class", "alert alert-danger");
            return "Old username cannot be null or empty.";
		}

        if (string.IsNullOrWhiteSpace(newUsername))
		{
			divMessage.Attributes.Add("class", "alert alert-danger");
            return "New username cannot be null or empty.";
		}

        if (oldUsername == newUsername)
		{
			divMessage.Attributes.Add("class", "alert alert-danger");
			return "New username and old username are the same.";
		}
		
		//make sure the new username does not already exist
		if(Membership.GetUser(newUsername) == null)
		{
			//Username does not exist, change it.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var user = (from u in dc.aspnet_Users
					   where u.UserId == userId
					   select u).SingleOrDefault();
			user.UserName = newUsername;
			user.LoweredUserName = newUsername;
			dc.SubmitChanges();
			
			divMessage.Attributes.Add("class", "alert alert-success");
			FormsAuthentication.SignOut();
            return "Username changed successfully.";
		}
		else
		{
			//This username already exists, don't change it.
			divMessage.Attributes.Add("class", "alert alert-danger");
            return "Your new username already exists, please choose another.";
		}
    }
}