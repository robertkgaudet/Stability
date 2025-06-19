using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_ExternalLoginTeamSelector : BaseWebForm
{
    public string nonProfitDropDown = string.Empty;
    public string eventId = string.Empty;
    public string organizationId = string.Empty;
    public string preselectedDisasterJQuery = string.Empty;
    public string preselectedNonProfitJQuery = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        eventId = Request.QueryString["eventId"];
        organizationId = Request.QueryString["organizationId"];

        if (!IsPostBack)
        {
            LoadNonProfits(organizationId, eventId);
        }
    }
    public void LoadNonProfits(string organizationId, string eventId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        if (!String.IsNullOrEmpty(organizationId))
        {
            //Filter to just this nonprofit.
            var nonProfit = (from o in dc.Organizations
                             where o.OrganizationId == new Guid(organizationId)
                             && o.IsActive == true
                             select new { o }).SingleOrDefault();

            nonProfitDropDown = nonProfitDropDown + "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;

            preselectedNonProfitJQuery = "$(\"#btn-NonProfitDropdown.nonProfit\").html('" + nonProfit.o.Name + "');";
            hidOrganizationId.Value = organizationId;
        }
        else
        {
            if (!String.IsNullOrEmpty(eventId))
            {
                //If they chose an event, filter to orgs that are added to the event.
                var nonProfits = from o in dc.Organizations
                                 join oe in dc.OrganizationEvents on o.OrganizationId equals oe.OrganizationId
                                 where oe.EventId == new Guid(eventId)
                                && o.IsActive == true
                                 orderby o.Name
                                 select new { o, oe };

                nonProfitDropDown = nonProfitDropDown + "<li><a href=\"#\">------ None ------</a></li>" + Environment.NewLine;
                foreach (var nonProfit in nonProfits)
                {
                    nonProfitDropDown = nonProfitDropDown + "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;
                }
            }
            else
            {
                //Load all available non-profits
                //If they chose an event, filter to orgs that are added to the event.
                var nonProfits = from o in dc.Organizations
                                 where o.IsActive == true
                                 orderby o.Name
                                 select new { o };


                nonProfitDropDown = nonProfitDropDown + "<li><a href=\"#\">------ None ------</a></li>" + Environment.NewLine;
                foreach (var nonProfit in nonProfits)
                {
                    nonProfitDropDown = nonProfitDropDown + "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;
                    organizationId = nonProfit.o.OrganizationId.ToString();
                }

                if (nonProfits.Count() == 1)
                {
                    hidOrganizationId.Value = organizationId;
                }
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string organizationId = hidOrganizationId.Value;
        MembershipUser user = Membership.GetUser();
        Guid currentUserId = Guid.Empty;
        if (user != null && user.ProviderUserKey != null)
        {
            currentUserId = (Guid)user.ProviderUserKey;
        }
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        {
            if (!String.IsNullOrEmpty(organizationId))
            {
                UserOrganization userOrganization = new UserOrganization();
                userOrganization.UserOrganizationId = Guid.NewGuid();
                userOrganization.UserId = currentUserId;
                userOrganization.OrganizationId = new Guid(organizationId);
                userOrganization.ShowTeamLogo = false;
                userOrganization.TeamVerifiedDate = null;
                userOrganization.IsPrimary = true;
                userOrganization.IsPreviousOwner = false;
                userOrganization.IsTeamAdministrator = false;
                userOrganization.IsOwner = false;
                userOrganization.Status = (int)RequestStatus.Pending;
                dc.UserOrganizations.InsertOnSubmit(userOrganization);
                dc.SubmitChanges();
            }
        }
        Response.Redirect("/V1/Profile/EditSkills.aspx?register=true");
    }
    protected void btnSubmit_Cancel(object sender, EventArgs e)
    {
        Response.Redirect("/V1/Profile/EditSkills.aspx?register=true");
    }
}