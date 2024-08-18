using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_EditNonProfits : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string organizationId = Request.QueryString["organizationId"];

		if(!String.IsNullOrEmpty(organizationId))
		{
			//Automatically set this users nonprofit to this id.
			UpdateUsersNonProfit(Membership.GetUser().ProviderUserKey.ToString(), organizationId);
		}

		if(!IsPostBack)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var organizations = from c in dc.Organizations
								where c.IsActive == true
								orderby c.Name
								select new {name = c.Name, c.OrganizationId };

			rblOrganizations.DataSource = organizations;
			rblOrganizations.DataBind();

			var userOrganization = (from uo in dc.UserOrganizations
								   where uo.UserId == userId
								   select uo).Take(1).SingleOrDefault();
			
			//If the user has multiple orgs selected, only choose CNGF for them.

			//Preselect the orgs for this user
			if(userOrganization != null)
			{
				ListItem listItemToSelect = rblOrganizations.Items.FindByValue(userOrganization.OrganizationId.ToString());

				if (listItemToSelect != null)
				{
					listItemToSelect.Selected = true;
				}


				//rblOrganizations
				//foreach(var userOrganization in userOrganizations)
				//{
				//	for (int i = 0; i < rblOrganizations.Items.Count; i++)
				//	{
				//		if(userOrganization.OrganizationId.ToString() == rblOrganizations.Items[i].Value)
				//		{
				//			chkBoxOrganizations.Items[i].Selected = true;
				//		}
				//	}
				//}
			}
		}
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/Default.aspx");
	}


	protected void UpdateUsersNonProfit(string userId, string organizationId)
	{
		divMessage.Visible = true;
		lblMessage.Text = "Your affilicated non-profit have been updated.";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		//Delete all previous entries.. and then add the new one.

		var userChecks = from p in dc.UserOrganizations
							where p.UserId == new Guid(userId)
							select p;

		//Delete any checked records
		if (userChecks.Count() > 0)
		{
			//Item is selected.
			foreach (var userCheck in userChecks)
			{
				dc.UserOrganizations.DeleteOnSubmit(userCheck);
				dc.SubmitChanges();
			}
		}

		//If user is in the EditNonProfit role remove them from it.
		if(User.IsInRole("NonProfitEditor"))
		{
			Roles.RemoveUserFromRole(User.Identity.Name, "NonProfitEditor");
		}

		//insert the checked item.
		UserOrganization userOrganization = new UserOrganization();
		userOrganization.OrganizationId = new Guid(organizationId);
		userOrganization.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		userOrganization.UserOrganizationId = Guid.NewGuid();
		dc.UserOrganizations.InsertOnSubmit(userOrganization);
		dc.SubmitChanges();

		Response.Redirect("/V1/Member/Default.aspx");
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string userId = Membership.GetUser().ProviderUserKey.ToString();
		string organizationId = string.Empty;

		if (rblOrganizations.SelectedIndex > -1)
		{
			ListItem selectedOrganization = rblOrganizations.SelectedItem;
			organizationId = selectedOrganization.Value;
			UpdateUsersNonProfit(userId, organizationId);
		}
		else
		{
			//No item selected
		}
	}
}