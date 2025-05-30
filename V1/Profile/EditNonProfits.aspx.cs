using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Services;

public partial class V1_Profile_EditNonProfits : BaseOrganizationWebForm
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];

        if (!String.IsNullOrEmpty(organizationId))
        {
            // Automatically set this user's nonprofit to this ID.
            UpdateUsersNonProfit(Membership.GetUser().ProviderUserKey.ToString(), organizationId);
        }

        if (!IsPostBack)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            // Load all active organizations
            var organizations = from c in dc.Organizations
                                where c.IsActive == true
                                orderby c.Name
                                select new { c.Name, c.OrganizationId };

            rblOrganizations.DataSource = organizations;
            rblOrganizations.DataBind();

            // Preselect user's current organizations
            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

            var userOrganizations = (from uo in dc.UserOrganizations
                                     where uo.UserId == userId && uo.Status== (int)RequestStatus.Approved
                                     select uo).ToList();

            foreach (var uo in userOrganizations)
            {
                ListItem item = rblOrganizations.Items.FindByValue(uo.OrganizationId.ToString());
                if (item != null)
                {
                    item.Selected = true;
                }
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
        userOrganization.Status = (int)RequestStatus.Pending;

        dc.SubmitChanges();

		Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=" + organizationId);
	}

  
}