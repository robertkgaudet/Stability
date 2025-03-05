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


    protected void PaymentConfigration_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtPayPal.Value) ||
            string.IsNullOrWhiteSpace(txtVenmo.Value) ||
            string.IsNullOrWhiteSpace(txtCashPay.Value))
        {
            return;
        }
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
            }
            context.SubmitChanges();
        }
    }
  
    protected void SaveCampaign(object sender, EventArgs e)
    {
        string organizationEventId = ddlOrganizationEvent.SelectedValue;
        string summary = txtCampaignSummary.Text.Trim();
        string address = txtCampaignMailingAddress.Text.Trim();
        string amount = textAmount.Value.Trim();
        string description = txtCampaignDescription.Text.Trim();
        bool isDefault = chkIsDefault.Checked;

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
 
            if (isDefault)
            {
                var existingDefaultCampaign = dc.DonationCampaigns.SingleOrDefault(c => c.IsDefault == true);
                if (existingDefaultCampaign != null)
                {
                    existingDefaultCampaign.IsDefault = false;
                    dc.SubmitChanges();
                }
            }

            Guid? organizationEventGuid = !string.IsNullOrEmpty(organizationEventId)
                                          ? new Guid(organizationEventId)
                                          : (Guid?)null; 

            if (string.IsNullOrEmpty(hdnSelectedCampaignId.Value))
            {
                DonationCampaign newCampaign = new DonationCampaign
                {
                    DonationCampaignId = Guid.NewGuid(),
                    OrganizationEventId = organizationEventGuid,
                    Summary = summary,
                    Address = address,
                    Description = description,
                    Amount = amount,
                    IsDefault = isDefault
                };

                dc.DonationCampaigns.InsertOnSubmit(newCampaign);
            }
            else 
            {
                Guid campaignId = new Guid(hdnSelectedCampaignId.Value);
                var existingCampaign = dc.DonationCampaigns.SingleOrDefault(c => c.DonationCampaignId == campaignId);
                if (existingCampaign != null)
                {
                    existingCampaign.OrganizationEventId = isDefault ? null : organizationEventGuid; 
                    existingCampaign.Summary = summary;
                    existingCampaign.Address = address;
                    existingCampaign.Description = description;
                    existingCampaign.Amount = amount;
                    existingCampaign.IsDefault = isDefault;
                }
            }

            dc.SubmitChanges();
        }

        ClearControls();
        BindDonationCampaigns();
        BindOrganizationEventDropDown();
    }



    private void ClearControls()
    {
        txtCampaignSummary.Text = string.Empty;
        txtCampaignMailingAddress.Text = string.Empty;
        txtCampaignDescription.Text = string.Empty;
        chkIsDefault.Text = string.Empty;
        ddlOrganizationEvent.ClearSelection();
        textAmount.Value = string.Empty;
        campaignId = null;
        chkIsDefault.Checked = false;
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
                                 //campaign.Amount,
                                 Amount = FormatAmount(campaign.Amount),
                                 campaign.IsDefault,


                             });

            gvDonationCampaigns.DataSource = campaigns;
            gvDonationCampaigns.DataBind();
        }
    }
    private string FormatAmount(string amount)
    {
        if (string.IsNullOrEmpty(amount))
            return string.Empty;
        string[] amounts = amount.Split(new[] { ',',' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < amounts.Length; i++)
        {
            amounts[i] = "$" + amounts[i].Trim();
        }
        return string.Join(", ", amounts);
    }

    protected void gvCampaigns_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument); 
            campaignId = new Guid(gvDonationCampaigns.DataKeys[rowIndex].Value.ToString());
            hdnSelectedCampaignId.Value = campaignId.ToString();

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                BindOrganizationEventDropDown();
                var campaign = dc.DonationCampaigns.Where(x => x.DonationCampaignId == campaignId).FirstOrDefault();
                ddlOrganizationEvent.SelectedValue = campaign.OrganizationEventId.ToString();
                txtCampaignSummary.Text = campaign.Summary;
                txtCampaignMailingAddress.Text = campaign.Address;
                txtCampaignDescription.Text = campaign.Description;
                textAmount.Value = campaign.Amount;
                chkIsDefault.Checked = campaign.IsDefault;
                hdnSelectedCampaignId.Value = campaignId.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "showModal('Edit Campaign');", true);
        }
        else if (e.CommandName == "DeleteRow")
        {
            campaignId = new Guid(e.CommandArgument.ToString());
            hdnSelectedCampaignId.Value = campaignId.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "showDeleteModal();", true);
        }
        else if (e.CommandName == "Preview")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            
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

    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearControls();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#yourModal').modal('hide');", true);
    }
    protected void gvDonationCampaigns_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var amount = DataBinder.Eval(e.Row.DataItem, "Amount");
            Button deleteButton = (Button)e.Row.FindControl("btnDelete");
            if (deleteButton != null)
            {
                if (string.IsNullOrEmpty(amount.ToString()) || amount.ToString() == "0")
                {
                    deleteButton.Visible = true;
                }
                else
                {
                    deleteButton.Visible = false;
                }
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];

        if (!string.IsNullOrEmpty(organizationId))
        {
            Response.Redirect("~/V1/NonProfitAdministration/Settings.aspx?organizationId=" + organizationId);
        }
        else
        {
            Response.Redirect("~/V1/NonProfit/AddEditTransactionConfigSetting.aspx");
        }
    }
}
   


