using CrowdRelief;
using Stripe;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

public partial class V1_NonProfit_DonationPaymentReturn : System.Web.UI.Page
{
    private string WebhookSecret = System.Configuration.ConfigurationManager.AppSettings["stripeWebhookSecret"].ToString();

    CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod != "POST")
        {
            Response.StatusCode = 405; // Method Not Allowed
            Response.End();
            return;
        }

        try
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }

            // Validate the Stripe signature
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                WebhookSecret,
                throwOnApiVersionMismatch: false
            );

            // Handle specific event types
            switch (stripeEvent.Type)
            {
                case ("checkout.session.completed"):
                    var paymentIntent = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    HandlePaymentSuccess(paymentIntent);
                    break;

                case ("payment_intent.failed"):
                    var failedIntent = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    HandlePaymentFailure(failedIntent);
                    break;

                default:
                    LogUnhandledEvent(stripeEvent.Type);
                    break;
            }

            // Respond to Stripe with a 200 OK
            Response.StatusCode = 200;
            Response.Write("{\"status\":\"success\"}");
        }
        catch (StripeException ex)
        {
            Response.StatusCode = 400; // Bad Request            
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500; // Internal Server Error            
        }
        finally
        {
            Response.End();
        }
    }

    private void HandlePaymentSuccess(Stripe.Checkout.Session paymentIntent)
    {
        Donation donation = dc.Donations.Where(x => x.DonationId == new Guid(paymentIntent.Metadata.Values.FirstOrDefault().ToString())).FirstOrDefault();
        if (donation != null)
        {
            donation.TransactionId = paymentIntent.PaymentIntentId;
            donation.AuthorizedTransactionId = paymentIntent.PaymentIntentId;
            donation.DonationStatus = (int)Tools.TransactionStatus.Succeeded;
            dc.SubmitChanges();


            ListDictionary ldEmailBodyReplacements = new ListDictionary
            {
                { "<% DonorFirstName %>", donation.FirstName },
                { "<% DonorLastName %>", donation.LastName },
                { "<% RecipLastName %>", "Update Later" }
            };

            string error = string.Empty;

            Tools.SendEmail(
                 string.Empty,
                 "Thanks for Donation to Stability",
                 ldEmailBodyReplacements,
                 donation.EmailAddress,
                 donation.FirstName + " " + donation.LastName,
                 string.Empty,
                 string.Empty,
                 "~\\EmailTemplates\\DonorLetter.html",
                 out error);

            if (!string.IsNullOrEmpty(donation.AuthorizedTransactionId))
            {
                Organization organization = dc.Organizations.Where(x => x.OrganizationId == new Guid(donation.AuthorizedTransactionId)).FirstOrDefault();
                ldEmailBodyReplacements = new ListDictionary
                {
                    { "<% OwnerName %>", organization.Name },
                    { "<% donAmount %>", paymentIntent.AmountTotal/100 }
                };
                Tools.SendEmail(
                 string.Empty,
                 "Donation Received",
                 ldEmailBodyReplacements,
                 organization.PointOfContactEmail,
                 organization.PointOfContactName,
                 string.Empty,
                 string.Empty,
                 "~\\EmailTemplates\\InformToOrganizationOwner.html",
                 out error);

                if (!string.IsNullOrEmpty(organization.PointOfContactPhoneNumber))
                {
                    string accountSid = System.Configuration.ConfigurationManager.AppSettings["twilioAccountSID"].ToString();  // Replace with your account SID
                    string authToken = System.Configuration.ConfigurationManager.AppSettings["twilioAuthToken"].ToString(); ;    // Replace with your auth token
                    string fromNumber = System.Configuration.ConfigurationManager.AppSettings["twilioPhoneNumber"].ToString(); ; // Replace with your Twilio number

                    // Array of phone numbers to send the SMS to
                    string[] phoneNumbers = { organization.PointOfContactPhoneNumber };

                    // The message you want to send
                    string messageBody = "Dear " + organization.Name +
                        ",\r\n\r\nWe are excited to inform you that a generous donation of " + donation.Amount + " has been made to your organization.";

                    // Create an instance of Tools and send the SMS
                    var tools = new Tools(accountSid, authToken, fromNumber);
                    tools.SendSms(messageBody, phoneNumbers);
                }
            }
        }


    }

    private void HandlePaymentFailure(Stripe.Checkout.Session paymentIntent)
    {

    }

    private void LogUnhandledEvent(string eventType)
    {

    }

}