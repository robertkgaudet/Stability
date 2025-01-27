using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Stripe.Checkout;
using Stripe;
using System.Configuration;

public partial class V1_NonProfit_DonationDetails : System.Web.UI.Page
{
    public string LiteralDescription { get; set; }
    public List<int> LiteralAmountList { get; set; }
    public string orgId { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
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
                            firstname.Value = profile.Firstname;
                            lastname.Value = profile.Lastname;
                            email.Value = username;
                            homeaddress.Value = profile.Address;
                            txtCity.Value = profile.City;


                            ddlState.Value = profile.State;

                            txtZip.Value = profile.Zip;

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
        string organizationId = Request.QueryString["organizationId"];
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

        this.publishKey.Value = System.Configuration.ConfigurationManager.AppSettings["stripePublishKey"].ToString();
        this.organizationId.Value = Request.QueryString["organizationId"].ToString();
        Guid donationCampaignId = Guid.Parse(Request.QueryString["donationCampaignId"]);
        Session["OrderId"] = donationCampaignId;
        this.orgId = Request.QueryString["organizationId"].ToString();
        CrowdReliefDBDataContext dbContext = new CrowdReliefDBDataContext();
        DonationCampaign donationCampaign = dbContext.DonationCampaigns.Where(x => x.DonationCampaignId == donationCampaignId).FirstOrDefault();
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
            }

        }
    }


    [System.Web.Services.WebMethod]
    public static string AddTransactionDetails(TransactionData transactionData)
    {
        try
        {
            var request = HttpContext.Current.Request;

            // Get the domain URL
            string domainUrl = request.Url.GetLeftPart(UriPartial.Authority);

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            Organization organization = dc.Organizations.Where(x => x.OrganizationId == new Guid(transactionData.OrganizationId)).FirstOrDefault();
            if (organization == null || organization.OwnerId == null)
            {

                return "";
            }
            Profile profile = new Profile()
            {
                ProfileId = Guid.NewGuid(),
                Address = transactionData.HomeAddress,
                Firstname = transactionData.FirstName,
                Lastname = transactionData.LastName,
                City = transactionData.City,
                State = transactionData.State,
                Zip = transactionData.Zip,
                UserId = organization.OwnerId.Value
            };
            dc.Profiles.InsertOnSubmit(profile);
            dc.SubmitChanges();
            Address address = new Address()
            {
                AddressId = Guid.NewGuid(),
                Address1 = transactionData.HomeAddress,
                City = transactionData.City,
                State = transactionData.State,
                Zip = transactionData.Zip,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
            dc.Addresses.InsertOnSubmit(address);
            dc.SubmitChanges();
            //ProfileAddress profileAddress = new ProfileAddress() // need to update later.
            //{
            //    ProfileAddressId = Guid.NewGuid(),
            //    AddressId = address.AddressId,
            //    ProfileId = profile.ProfileId,           
            //    RebuildStatusId = new Guid("109693FE-17B8-4B43-8E90-D8D6DF8136C2");
            //};
            //dc.ProfileAddresses.InsertOnSubmit(profileAddress);
            //dc.SubmitChanges();           


            Donation donation = new Donation()
            {
                Amount = transactionData.DonationAmount,
                AuthorizedTransactionId = organization.OrganizationId.ToString(),
                EmailAddress = transactionData.Email,
                DonationId = Guid.NewGuid(),
                FirstName = transactionData.FirstName,
                LastName = transactionData.LastName,
                ItemCount = 1,
                CreatedAt = DateTime.Now,
                BasicNeedsSurveyItemId = new Guid("7CC45EAD-7FB1-44A1-9CDA-EA76C4404102"),
                CampaignId = new Guid("24C73ECD-9345-4CB0-8234-4D9EF5472302"),
                PaymentProvider = (int)Tools.TransactionType.CreditCard,
                DonationStatus = (int)Tools.TransactionStatus.Started,
                OrderId = HttpContext.Current.Session["OrderId"].ToString() ?? "",
                IsTest = Convert.ToBoolean(ConfigurationManager.AppSettings["isTestPayment"])
            };
            dc.Donations.InsertOnSubmit(donation);
            dc.SubmitChanges();

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
                        UnitAmount = Convert.ToInt32(transactionData.DonationAmount) * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Donation to " + organization.Name, // Product Name
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = domainUrl + "/V1/NonProfit/DonationSuccess.aspx?session_id={CHECKOUT_SESSION_ID}", // Redirect after successful payment,
            };

            var service = new SessionService();
            Session session = service.Create(options);
            // Pass the session ID to the client
            return session.Id;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
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
        public int Frequency { get; set; }
        public bool IsCoverFee { get; set; }
        public bool NotShareName { get; set; }
        public bool IsHonorDonation { get; set; }
    }
}