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
            BindEmailTemplate();
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
                          orderby orgEvent.CampaignName // Added ordering here
                          select new
                          {
                              orgEvent.OrganizationEventId,
                              orgEvent.OrganizationId,
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


        string orgIdValue = Request.QueryString["organizationId"];
        Guid? organizationGuid = !string.IsNullOrEmpty(orgIdValue) ? new Guid(orgIdValue) : (Guid?)null;

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            //if (isDefault)
            //{
            //    var existingDefaultCampaign = dc.DonationCampaigns.SingleOrDefault(c => c.IsDefault == true);
            //    if (existingDefaultCampaign != null)
            //    {
            //        existingDefaultCampaign.IsDefault = false;
            //        dc.SubmitChanges();
            //    }
            //}

            Guid? organizationEventGuid = !string.IsNullOrEmpty(organizationEventId)
                                          ? new Guid(organizationEventId)
                                          : (Guid?)null;

            if (string.IsNullOrEmpty(hdnSelectedCampaignId.Value))
            {
                DonationCampaign newCampaign = new DonationCampaign
                {
                    DonationCampaignId = Guid.NewGuid(),
                    OrganizationEventId = organizationEventGuid,
                    OrganizationId = organizationGuid,
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
                    existingCampaign.OrganizationId = organizationGuid;
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


    protected void SaveEmailTemplate(object sender, EventArgs e)
    {
        string emailBody = emailbody.Text.Trim();
        string ccText = cc.Text.Trim();
        string bccText = bcc.Text.Trim();
        string orgIdValue = Request.QueryString["organizationId"];
        string templateIdValue = hdnSelectedEmailTemplateId.Value;

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            Guid organizationId = Guid.Parse(orgIdValue);

            if (!string.IsNullOrWhiteSpace(templateIdValue))
            {
                Guid templateId = Guid.Parse(templateIdValue);
                var existingTemplate = dc.EmailTemplates
                    .SingleOrDefault(t => t.EmailTemplateId == templateId);

                if (existingTemplate != null)
                {
                    existingTemplate.EmailBody = emailBody;
                    existingTemplate.CC = ccText;
                    existingTemplate.BCC = bccText;
                }
            }
            else
            {
                var existingTemplate = dc.EmailTemplates
                    .SingleOrDefault(t => t.OrganizationId == organizationId);

                if (existingTemplate == null)
                {
                    EmailTemplate newTemplate = new EmailTemplate
                    {
                        EmailTemplateId = Guid.NewGuid(),
                        OrganizationId = organizationId,
                        EmailBody = emailBody,
                        CC = ccText,
                        BCC = bccText
                    };
                    dc.EmailTemplates.InsertOnSubmit(newTemplate);
                }
                else
                {
                    existingTemplate.EmailBody = emailBody;
                    existingTemplate.CC = ccText;
                    existingTemplate.BCC = bccText;
                }
            }

            dc.SubmitChanges();
            ClearControls();
            hdnSelectedEmailTemplateId.Value = "";
            BindEmailTemplate();
        }
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
    private void EmailClearControls()
    {
        emailbody.Text = string.Empty;
        cc.Text = string.Empty;
        bcc.Text = string.Empty;
        
    }
    private void BindDonationCampaigns()
    {

        var orgId = new Guid(organizationId);

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var campaigns = (from campaign in dc.DonationCampaigns
                             where campaign.OrganizationId == orgId
                             select new
                             {
                                 DonationCampaignId = campaign.DonationCampaignId,
                                 OrganizationEventName = campaign.OrganizationEventId == null ? "Default" : dc.OrganizationEvents.Where(x => x.OrganizationEventId == campaign.OrganizationEventId).FirstOrDefault().CampaignName,
                                 Amount = FormatAmount(campaign.Amount),
                                 IsDefault = campaign.IsDefault
                             }).ToList();

            gvDonationCampaigns.DataSource = campaigns;
            gvDonationCampaigns.DataBind();
        }
    }
    private void BindEmailTemplate()
    {
        Guid orgId = new Guid(organizationId);

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var templates = (from template in dc.EmailTemplates
                             where template.OrganizationId == orgId
                             select new
                             {
                                 EmailTemplateId = template.EmailTemplateId,
                                 EmailBody = template.EmailBody,
                                 CC = template.CC,
                                 BCC = template.BCC
                             }).ToList();
            if (templates.Any())
            {
                btnAddEmailTemplate.Visible = false;
            }
            else
            {
                btnAddEmailTemplate.Visible = true;


            }
            gvEmailTemplates.DataSource = templates;
            gvEmailTemplates.DataBind();
        }
    }

    private string FormatAmount(string amount)
    {
        if (string.IsNullOrEmpty(amount))
            return string.Empty;
        string[] amounts = amount.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
    protected void gvEmailTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            Guid templateId = new Guid(e.CommandArgument.ToString());
            hdnSelectedEmailTemplateId.Value = templateId.ToString();

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var template = dc.EmailTemplates.FirstOrDefault(x => x.EmailTemplateId == templateId);
                if (template != null)
                {
                    emailbody.Text = template.EmailBody;
                    cc.Text = template.CC;
                    bcc.Text = template.BCC;
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "showModalEmail('Edit EmailTemplate');", true);
        }
        else if (e.CommandName == "DeleteRow")
        {
            Guid templateId = new Guid(e.CommandArgument.ToString());
            hdnSelectedEmailTemplateId.Value = templateId.ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "DeleteModalEmail();", true);
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

    protected void DeleteEmailTemplate(object sender, EventArgs e)
    {
        Guid templateId = new Guid(hdnSelectedEmailTemplateId.Value);

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var templateToDelete = dc.EmailTemplates.SingleOrDefault(t => t.EmailTemplateId == templateId);
            if (templateToDelete != null)
            {
                dc.EmailTemplates.DeleteOnSubmit(templateToDelete);
                dc.SubmitChanges();
            }
        }

        hdnSelectedEmailTemplateId.Value = "";

        BindEmailTemplate();
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearControls();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#yourModal').modal('hide');", true);
    }
    protected void btnClose_Email(object sender, EventArgs e)
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



