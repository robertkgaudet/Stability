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

public partial class V1_NonProfit_AddEdit : System.Web.UI.Page
{
    public string organizationId = string.Empty;
    public Guid? campaignId;
    protected void Page_Load(object sender, EventArgs e)
    {
        organizationId = Request.QueryString["OrganizationId"];

        if (!IsPostBack && !string.IsNullOrEmpty(organizationId))
        {
            using (var context = new CrowdReliefDBDataContext())
            {
                var paymentConfig = context.PaymentConfigurations
                    .FirstOrDefault(p => p.OrganizationId == new Guid(organizationId));

                if (paymentConfig != null)
                {
                    txtPayPal.Value = paymentConfig.PayPal;
                    txtVenmo.Value = paymentConfig.Venmo;
                    txtCashPay.Value = paymentConfig.CashApp;
                    chkShowDonateButton.Checked = paymentConfig.ShowDonationButton.Value;
                }
            }
            BindOrganizationEventDropDown();
            BindDonationCampaigns();
        }
    }
    private void BindOrganizationEventDropDown()
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var query = dc.DonationCampaigns.AsQueryable();
            if (campaignId.HasValue)
            {
                query = query.Where(x => x.DonationCampaignId != campaignId);
            }
            Guid?[] eventOrgIds = query.Select(x => x.OrganizationEventId).ToArray();

            var events = (from orgEvent in dc.OrganizationEvents
                          where orgEvent.IsActive == true && orgEvent.OrganizationId == new Guid(organizationId)
                          && !eventOrgIds.Contains(orgEvent.OrganizationEventId)
                          select new
                          {
                              orgEvent.OrganizationEventId,
                              orgEvent.CampaignName
                          }).ToList();

            ddlOrganizationEvent.DataTextField = "CampaignName";
            ddlOrganizationEvent.DataValueField = "OrganizationEventId";
            ddlOrganizationEvent.DataSource = events;
            ddlOrganizationEvent.DataBind();
            ddlOrganizationEvent.Items.Insert(0, new ListItem("Select Deployment", ""));
        }
    }
    protected void btnSelectPayment_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrWhiteSpace(txtPayPal.Value) ||
            string.IsNullOrWhiteSpace(txtVenmo.Value) ||
            string.IsNullOrWhiteSpace(txtCashPay.Value))
        {
            Response.Write("<script>alert('Please fill out all fields.');</script>");
            return;
        }

        try
        {
            using (var context = new CrowdReliefDBDataContext())
            {
                var paymentConfig = context.PaymentConfigurations
                    .FirstOrDefault(p => p.OrganizationId == new Guid(organizationId));
                if (paymentConfig != null)
                {
                    paymentConfig.PayPal = txtPayPal.Value;
                    paymentConfig.Venmo = txtVenmo.Value;
                    paymentConfig.CashApp = txtCashPay.Value;
                    paymentConfig.ShowDonationButton = chkShowDonateButton.Checked;

                    context.SubmitChanges();
                    Response.Write("<script>alert('Payment Configuration updated successfully!');</script>");
                }
                else
                {
                    var newPaymentConfig = new PaymentConfiguration
                    {
                        PaymentConfigurationId = Guid.NewGuid(),
                        PayPal = txtPayPal.Value,
                        Venmo = txtVenmo.Value,
                        CashApp = txtCashPay.Value,
                        OrganizationId = new Guid(organizationId),
                        ShowDonationButton = chkShowDonateButton.Checked
                    };

                    context.PaymentConfigurations.InsertOnSubmit(newPaymentConfig);
                    context.SubmitChanges();
                    Response.Write("<script>alert('Payment Configuration saved successfully!');</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
        }
    }
    protected void saveCampaign(object sender, EventArgs e)
    {
        string organizationEventId = ddlOrganizationEvent.SelectedValue;
        string summary = txtPaymentSummary.Text.Trim();
        string address = txtAddress.Text.Trim();
        string amount = textAmount.Value.Trim();
        string description = txtDescription.Text.Trim();
        bool isDefault = chkIsDefault.Checked;
        if (string.IsNullOrEmpty(hdnSelectedCampaignId.Value))
        {
            DonationCampaign newCampaign = new DonationCampaign
            {
                DonationCampaignId = Guid.NewGuid(),
                OrganizationEventId = new Guid(organizationEventId),
                Summary = summary,
                Address = address,
                Description = description,
                Amount = amount,
                IsDefault = isDefault
            };

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                dc.DonationCampaigns.InsertOnSubmit(newCampaign);
                dc.SubmitChanges();
            }
        }
        else
        {
            Guid campaignId = new Guid(hdnSelectedCampaignId.Value);

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var existingCampaign = dc.DonationCampaigns.SingleOrDefault(c => c.DonationCampaignId == campaignId);
                if (existingCampaign != null)
                {
                    existingCampaign.OrganizationEventId = new Guid(organizationEventId);
                    existingCampaign.Summary = summary;
                    existingCampaign.Address = address;
                    existingCampaign.Description = description;
                    existingCampaign.Amount = amount;
                    existingCampaign.IsDefault = isDefault;

                    dc.SubmitChanges();
                }
            }
        }
        ClearControls();
        BindDonationCampaigns();
        BindOrganizationEventDropDown();

    }
    private void ClearControls()
    {
        txtPaymentSummary.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtDescription.Text = string.Empty;
        chkIsDefault.Text = string.Empty;
        ddlOrganizationEvent.ClearSelection();
        textAmount.Value = string.Empty;
        campaignId = null;
        hdnSelectedCampaignId.Value = "";
    }
    private void BindDonationCampaigns()
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var campaigns = (from campaign in dc.DonationCampaigns
                             join orgEvent in dc.OrganizationEvents
                             on campaign.OrganizationEventId equals orgEvent.OrganizationEventId
                             where orgEvent.OrganizationId == new Guid(organizationId) && orgEvent.IsActive == true
                             orderby orgEvent.BeginDate
                             select new
                             {
                                 campaign.DonationCampaignId,
                                 OrganizationEventName = orgEvent.CampaignName,
                                 campaign.Amount,
                                 campaign.IsDefault
                             });

            gvDonationCampaigns.DataSource = campaigns;
            gvDonationCampaigns.DataBind();
        }
    }
    protected void gvCampaigns_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        campaignId = new Guid(gvDonationCampaigns.DataKeys[rowIndex].Value.ToString());
        hdnSelectedCampaignId.Value = campaignId.ToString();
        if (e.CommandName == "EditRow")
        {
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                BindOrganizationEventDropDown();
                var campaign = dc.DonationCampaigns.Where(x => x.DonationCampaignId == campaignId).FirstOrDefault();
                ddlOrganizationEvent.SelectedValue = campaign.OrganizationEventId.ToString();
                txtPaymentSummary.Text = campaign.Summary;
                txtAddress.Text = campaign.Address;
                txtDescription.Text = campaign.Description;
                textAmount.Value = campaign.Amount;
                chkIsDefault.Checked = campaign.IsDefault;
                hdnSelectedCampaignId.Value = campaignId.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "showModal('Edit Campaign');", true);
        }
        else if (e.CommandName == "DeleteRow")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "showDeleteModal();", true);
        }
    }
    protected void DeleteCampaign(object sender, EventArgs e)
    {
        Guid campaignId = new Guid(hdnSelectedCampaignId.Value);

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var campaignToDelete = dc.DonationCampaigns.SingleOrDefault(c => c.DonationCampaignId == campaignId);
            if (campaignToDelete != null)
            {
                dc.DonationCampaigns.DeleteOnSubmit(campaignToDelete);
                dc.SubmitChanges();
            }
        }
        BindDonationCampaigns();
        BindOrganizationEventDropDown();
    }
}

