using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_TeamName : BaseWebForm
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string organizationName = txtParentOrganization.Value;

        Organization organization = new Organization();

        organization.Name = organizationName;
        organization.CreatedBy = userId;
        organization.CreatedOn = DateTime.Now;
        organization.IsActive = true;
        organization.OwnerId = userId;
        organization.OrganizationId = Guid.NewGuid();

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        dc.Organizations.InsertOnSubmit(organization);
        dc.SubmitChanges();

        UserOrganization userOrganization = new UserOrganization();
        userOrganization.UserOrganizationId = Guid.NewGuid();
        userOrganization.OrganizationId = organization.OrganizationId;
        userOrganization.UserId = userId;
        userOrganization.IsPrimary = true;
        dc.UserOrganizations.InsertOnSubmit(userOrganization);
        dc.SubmitChanges();

        //var profile = ProfileBase.Create(User.Identity.Name);
        //string email = profile.GetPropertyValue("Email") as string;
        //string firstName = profile.GetPropertyValue("Firstname") as string;

        var profile = (from p in dc.Profiles
                       join u in dc.aspnet_Memberships on p.UserId equals u.UserId
                       where p.UserId == userId
                       select new { p.Firstname, u.Email,u.UserId }).SingleOrDefault();

        //Send an email.
        ListDictionary ldEmailBodyReplacements = new ListDictionary();
        ldEmailBodyReplacements.Add("<% OrganizationName %>", organizationName);
        ldEmailBodyReplacements.Add("<% RecipientsName %>", profile.Firstname);
        ldEmailBodyReplacements.Add("<% OrganizationId %>", organization.OrganizationId.ToString());

        string error = string.Empty;
        Tools.SendEmail(
        string.Empty,
        "Your Disaster Relief Team " + organizationName,
        ldEmailBodyReplacements,
        profile.Email,
        profile.Firstname,
        string.Empty,
        string.Empty,
        "~\\EmailTemplates\\TeamName.html",
        out error);
        BaseWebForm.AddNotifications(
     NotificationType.TeamIsCreated,
     FeatureTypeEnum.About,
     "Created Team ",
     "A new team has been successfully created for the organization.",
     userOrganization.OrganizationId,
     true,
     userOrganization.OrganizationId.ToString()

);

        Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=" + organization.OrganizationId);
    }
    protected void btnSubmit_Cancel(object sender, EventArgs e)
    {
        Response.Redirect("/V1/Member/Default.aspx");
    }
}