using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Stripe.Checkout;
using Stripe;
using System.Configuration;
using SendGrid;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_DonationDetails : System.Web.UI.Page
{
    public string LiteralDescription { get; set; }
    public List<int> LiteralAmountList { get; set; }
    public string orgId { get; set; }
    public Guid DonationCampaignId { get; set; }
    public string CampaignName { get; set; }
    public string OrganizationName { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {

        bool showDonateButton = false;

        if (!IsPostBack)
        {
            using (var context = new CrowdReliefDBDataContext())
            {
                var paymentConfig = context.PaymentConfigurations.FirstOrDefault();

                if (paymentConfig != null)
                {
                    showDonateButton = paymentConfig.ShowDonationButton ?? false;
                }
            }
            string organizationId = Request.QueryString["organizationId"];

            if (!showDonateButton)
            {
                Response.Redirect("~/V1/NonProfit/Donation.aspx?organizationId=" + organizationId);

                return;
            }
            else
            {
                ListItemCollection statesList = new ListItemCollection();
                foreach (string state in States.Names())
                {
                    ListItem li = new ListItem(state, state);
                    statesList.Add(li);
                }

                ddlState.DataSource = statesList;
                ddlState.DataBind();

                if (User.Identity.IsAuthenticated)
                {
                    string username = User.Identity.Name;

                    if (!string.IsNullOrEmpty(username))
                    {
                        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
                        {
                            var user = (from u in dc.aspnet_Users
                                        where u.UserName == username
                                        select u).SingleOrDefault();

                            if (user != null)
                            {
                                Guid userId = user.UserId;
                                var profile = (from p in dc.Profiles
                                               where p.UserId == userId
                                               select new
                                               {
                                                   p.Firstname,
                                                   p.Lastname,
                                                   p.Address,
                                                   p.City,
                                                   p.State,
                                                   p.Zip
                                               }).SingleOrDefault();
                                if (profile != null)
                                {
                                    txtfirstname.Text = profile.Firstname;
                                    txtlastname.Text = profile.Lastname;
                                    txtemail.Text = username;
                                    txthomeaddress.Text = profile.Address;
                                    txtCity.Text = profile.City;


                                    ddlState.DataSource = statesList;

                                    txtZip.Text = profile.Zip;

                                }

                            }

                        }
                    }
                    btnLogin.Visible = false;
                }
                else
                {
                    btnLogin.Visible = true;
                }

                if (String.IsNullOrEmpty(organizationId))
                {
                    Response.Write("No organization Id provided.");
                    Response.End();
                    return;
                }
                string returnUrl = Request.QueryString["returnUrl"];
                if (String.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = Request.Url.AbsoluteUri;
                }
                Session["ReturnUrl"] = returnUrl;

                this.publishKey.Text = System.Configuration.ConfigurationManager.AppSettings["stripePublishKey"].ToString();
                this.organizationId.Text = Request.QueryString["organizationId"].ToString();
                DonationCampaignId = Guid.Parse(Request.QueryString["donationCampaignId"]);

                this.orgId = Request.QueryString["organizationId"].ToString();
                CrowdReliefDBDataContext dbContext = new CrowdReliefDBDataContext();
                DonationCampaign donationCampaign = dbContext.DonationCampaigns.Where(x => x.DonationCampaignId == DonationCampaignId).FirstOrDefault();
                if (donationCampaign != null)
                {
                    LiteralDescription = donationCampaign.Description;
                    var amounts = donationCampaign.Amount.Split(',');
                    if (amounts.Length > 0)
                    {
                        LiteralAmountList = new List<int>();
                        foreach (var amount in amounts)
                        {
                            LiteralAmountList.Add(Convert.ToInt32(amount));
                        }
                        if (LiteralAmountList.Any())
                        {
                            txtDonationAmount.Text = LiteralAmountList[0].ToString();
                        }
                    }
                    string orgId = Request.QueryString["organizationId"].ToString();
                    string donationCampaignId = Request.QueryString["donationCampaignId"].ToString();
                    var organizationEventId = dbContext.DonationCampaigns.Where(x => x.DonationCampaignId == new Guid(donationCampaignId)).Select(x => x.OrganizationEventId).FirstOrDefault();
                    if (organizationEventId != null)
                    {
                        var Campaign = dbContext.OrganizationEvents.Where(x => x.OrganizationEventId == new Guid(organizationEventId.ToString())).Select(x => x.CampaignName).FirstOrDefault();
                        CampaignName = ", " + Campaign;
                    }
                    OrganizationName = dbContext.Organizations.Where(x => x.OrganizationId == new Guid(orgId)).Select(x => x.Name).FirstOrDefault();
                }
            }
        }
    }






    protected void AddTransactionDetails_Click(object sender, EventArgs e)
    {
        Guid donationCampaignId = new Guid(Request.QueryString["donationCampaignId"]);
        var request = HttpContext.Current.Request;
        // Get the domain URL
        string domainUrl = request.Url.GetLeftPart(UriPartial.Authority);

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        Organization organization = dc.Organizations.Where(x => x.OrganizationId == new Guid(organizationId.Text)).FirstOrDefault();
        if (organization == null || organization.OwnerId == null)
        {
        }
        if (!User.Identity.IsAuthenticated)
        {
            Profile profile = new Profile()
            {
                ProfileId = Guid.NewGuid(),

                Address = txthomeaddress.Text,
                Firstname = txtfirstname.Text,
                Lastname = txtlastname.Text,
                City = txtCity.Text,
                State = ddlState.SelectedValue,
                Zip = txtZip.Text,

                UserId = organization.OwnerId.Value
            };
            dc.Profiles.InsertOnSubmit(profile);
            dc.SubmitChanges();
            Address address = new Address()
            {
                AddressId = Guid.NewGuid(),
                Address1 = txthomeaddress.Text,
                City = txtCity.Text,
                State = ddlState.SelectedValue,
                Zip = txtZip.Text,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
            dc.Addresses.InsertOnSubmit(address);
            dc.SubmitChanges();
        }




        var donationAmount = Request.Form["txtDonationAmount"];
        decimal originalAmount = Decimal.Parse(donationAmount);
        decimal transactionFee = string.IsNullOrEmpty(coverfee.Text) ? 0 : originalAmount * 0.06M;
        decimal totalAmount = originalAmount + transactionFee; // For display only, not saved

        Donation donation = new Donation()
        {
            Amount = originalAmount,
            TransactionFee = transactionFee,
            EmailAddress = txtemail.Text,
            DonationId = Guid.NewGuid(),
            FirstName = txtfirstname.Text,
            LastName = txtlastname.Text,
            ItemCount = 1,
            CreatedAt = DateTime.Now,
            PaymentProvider = (int)Tools.TransactionType.CreditCard,
            DonationStatus = (int)Tools.TransactionStatus.Started,
            DonationCampaignId = donationCampaignId,
            IsTest = Convert.ToBoolean(ConfigurationManager.AppSettings["isTestPayment"])
        };

        dc.Donations.InsertOnSubmit(donation);
        dc.SubmitChanges();


        txtDonationAmount.Text = totalAmount.ToString("F2");




        StripeConfiguration.ApiKey = System.Configuration.ConfigurationManager.AppSettings["stripeSecretKey"].ToString();
        Dictionary<string, string> transactionInfo = new Dictionary<string, string>
                {
                    { "transactionId", donation.DonationId.ToString() }
                };
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            Metadata = transactionInfo,
            LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = Convert.ToInt32((donation.Amount + donation.TransactionFee) * 100), 
                             ProductData = new SessionLineItemPriceDataProductDataOptions
                             {
                                Name = "Donation to " + organization.Name, 
                             },
                        },
                                        Quantity = 1,

                    },
                },
            Mode = "payment",
            SuccessUrl = domainUrl + "/V1/NonProfit/DonationSuccess.aspx?session_id={CHECKOUT_SESSION_ID}",
        };

        var service = new SessionService();
        Session session = service.Create(options);

        Response.Redirect(session.Url);

    }

    //protected void btnPrevious_Click(object sender, EventArgs e)
    //{

    //    if (Request.UrlReferrer != null)
    //    {
    //        Response.Redirect("~/V1/NonProfit/Donation.aspx?organizationId=" + organizationId);
    //    }
    //    else
    //    {
    //        Response.Redirect(Request.UrlReferrer.ToString());

    //    }
    //}
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];

        if (!string.IsNullOrEmpty(organizationId))
        {
            Response.Redirect("~/V1/NonProfit/Donation.aspx?organizationId=" + organizationId);
        }
        else
        {
            Response.Redirect("~/V1/NonProfit/Donation.aspx?organizationId.aspx");
        }
    }


    //protected void btnCancel_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.UrlReferrer.ToString());
    //}

}




public class TransactionData
{
    public string OrganizationId { get; set; }
    public decimal DonationAmount { get; set; }
    public string HomeAddress { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string DateOfBirth { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string DonationNote { get; set; }
    // public int Frequency { get; set; }
    public bool IsCoverFee { get; set; }
    public bool NotShareName { get; set; }
    public bool IsHonorDonation { get; set; }
}
