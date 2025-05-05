using CrowdRelief;
using GoogleMapsAPI.Places;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Tls;
using Stripe;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class V1_NonProfit_InvitedMembers : BaseOrganizationWebForm
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (User.Identity.IsAuthenticated)
            {
            }
        }
        else
        {
         }
        LoadInvitedMemberList();
    }

    private void LoadInvitedMemberList()
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var invitedMembers = (from i in dc.UserOrganizationInvites
                                  where i.InvitationCancelled == false || i.EmailSent == false
                                  orderby i.CreatedOn ascending
                                  select new
                                  {
                                      i.EmailAddress,
                                      i.CreatedOn,
                                      i.UserOrganizationInviteId,
                                      i.InvitationCancelled,
                                      i.HasAccepted,
                                      Status = i.HasAccepted == true ? "Member" : "Not a Member"
                                  });
            rptInvitedMembers.DataSource = invitedMembers;
            rptInvitedMembers.DataBind();
        }
    }

    [WebMethod]
    public static bool CancelInviteById(string userOrgInviteId)
    {
        using (var dc = new CrowdReliefDBDataContext())
        {
            var inviteMember = dc.UserOrganizationInvites.FirstOrDefault(f => f.UserOrganizationInviteId == new Guid(userOrgInviteId));

            if (inviteMember != null)
            {
                inviteMember.InvitationCancelled = true;
                dc.SubmitChanges();
            }
        }
        return true;
    }

    [WebMethod]
    public static bool ResendInviteById(string userOrgInviteId)
    {
        using (var dc = new CrowdReliefDBDataContext())
        {
            var inviteMember = dc.UserOrganizationInvites.FirstOrDefault(f => f.UserOrganizationInviteId == new Guid(userOrgInviteId));

            if (inviteMember != null)
            {
                var organizationInfo = (from p in dc.Profiles
                                        join o in dc.UserOrganizations on p.UserId equals o.UserId
                                        where o.OrganizationId == inviteMember.OrganizationId
                                        select new { organizationName = o.Organization.Name, p.Firstname, senderName = p.Firstname + " " + p.Lastname }).FirstOrDefault();

                string senderName = organizationInfo.senderName;
                string organizationName = organizationInfo.organizationName;
                string firstName = organizationInfo.Firstname;

                ListDictionary ldEmailBodyReplacements = new ListDictionary();
                ldEmailBodyReplacements.Add("<% FirstName %>", firstName);
                ldEmailBodyReplacements.Add("<% UserOrganizationInviteId %>", userOrgInviteId);
                string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
                string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();
                string emailError = string.Empty;

                Tools.SendEmail(
                    string.Empty,
                    senderName + " Has Invited You To Join His Impactoid Disaster Relief Team",
                    ldEmailBodyReplacements,
                    inviteMember.EmailAddress,
                    firstName,
                    senderName,
                    emailFrom,
                    "~\\EmailTemplates\\MemberInvitation.html",
                    out emailError 
                    );
                inviteMember.EmailSent = true;
                dc.SubmitChanges();
            }
        }
        return true;
    }
}