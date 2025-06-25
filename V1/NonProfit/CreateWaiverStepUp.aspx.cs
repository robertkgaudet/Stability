using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CrowdRelief;
using System.IdentityModel.Metadata;
using Braintree;
using Stability;
using System.Activities;
using System.Web.Security;

public partial class V1_NonProfit_CreateWaiverStepUp : System.Web.UI.Page
{
    public string organizationId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            organizationId = Request.QueryString["OrganizationId"];
            BindGrid();
            if (!string.IsNullOrEmpty(organizationId))
            {

                using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
                {
                    var waiver = dc.OrganizationWaivers.FirstOrDefault(w => w.OrganizationId == new Guid(organizationId));
                    if (waiver != null)
                    {
                        txtWaiver.Text = waiver.WaiverText;
                        chkRequireWaiver.Checked = waiver.IsRequired;
                    }
                }
            }
        }
    }

    protected void btnSaveWaiver_Click(object sender, EventArgs e)
    {
        string waiverText = txtWaiver.Text;
        bool isRequired = chkRequireWaiver.Checked;
        organizationId = Request.QueryString["OrganizationId"];
        if (!string.IsNullOrEmpty(organizationId))
        {

            Guid currentUserId = (Guid)Membership.GetUser().ProviderUserKey;

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var waiver = dc.OrganizationWaivers.FirstOrDefault(w => w.OrganizationId == new Guid(organizationId));

                if (waiver == null)
                {
                    waiver = new OrganizationWaiver
                    {
                        OrganizationId = new Guid(organizationId),
                        WaiverText = waiverText,
                        IsRequired = isRequired,
                        UpdatedOn = DateTime.Now,
                        UpdatedBy = currentUserId
                    };
                    dc.OrganizationWaivers.InsertOnSubmit(waiver);
                }
                else
                {
                    waiver.WaiverText = waiverText;
                    waiver.IsRequired = isRequired;
                    waiver.UpdatedOn = DateTime.Now;
                    waiver.UpdatedBy = currentUserId;
                }

                dc.SubmitChanges();
            }
        }

    }
    private void BindGrid()
    {
        Guid orgId = new Guid(Request.QueryString["OrganizationId"]);

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var allUsers = from wa in dc.WaiverSignatures
                           where wa.OrganizationId == orgId
                           join uo in dc.UserOrganizations
                               on new { wa.UserId, wa.OrganizationId } equals new { uo.UserId, uo.OrganizationId }
                           join p in dc.Profiles on wa.UserId equals p.UserId
                           join m in dc.aspnet_Memberships on wa.UserId equals m.UserId
                           select new
                           {
                               Name = p.Firstname + " " + p.Lastname,
                               Email = m.Email,
                               JoinDate = uo.TeamVerifiedDate,
                               IsSigned = true,
                               SignedDate = wa.SignedOn
                           };



            gvSignatures.DataSource = allUsers.ToList();
            gvSignatures.DataBind();
        }
    }



}
