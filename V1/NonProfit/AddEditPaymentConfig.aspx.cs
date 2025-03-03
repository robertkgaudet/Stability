using System;
using System.Linq;

public partial class V1_NonProfit_AddEditPaymentConfig : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["OrganizationId"];



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
                    txtpaymentSummary.Text = paymentConfig.Summary;
                    txtAddress.Text = paymentConfig.Address;
                    chkShowDonateButton.Checked = paymentConfig.ShowDonationButton == null ? false : paymentConfig.ShowDonationButton.Value;
                }
            }
        }
    }
    protected void btnSelectPayment_Click(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["OrganizationId"];
        string positionChecklist = Server.HtmlEncode(txtpaymentSummary.Text);
        string positionSummary = Server.HtmlEncode(txtAddress.Text);

        if (string.IsNullOrWhiteSpace(txtPayPal.Value) ||
            string.IsNullOrWhiteSpace(txtVenmo.Value) ||
            string.IsNullOrWhiteSpace(txtCashPay.Value) ||
            string.IsNullOrWhiteSpace(txtpaymentSummary.Text) ||
            string.IsNullOrWhiteSpace(txtAddress.Text))
        {
            Response.Write("<script>alert('Please fill out all fields.');</script>");
            return;
        }

        try
        {
            using (var context = new CrowdReliefDBDataContext())
            {
                var paymentConfig = context.PaymentConfigurations.FirstOrDefault(p => p.OrganizationId == new Guid(organizationId));

                if (paymentConfig != null)
                {
                    paymentConfig.PayPal = txtPayPal.Value;
                    paymentConfig.Venmo = txtVenmo.Value;
                    paymentConfig.CashApp = txtCashPay.Value;
                    paymentConfig.Summary = txtpaymentSummary.Text;
                    paymentConfig.Address = txtAddress.Text;
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
                        Summary = txtpaymentSummary.Text,
                        Address = txtAddress.Text,
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
}




