using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Braintree;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Secure_Test : System.Web.UI.Page
{
	public string ClientName = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		bool testMode					= Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["brainTreeTestMode"].ToString());
		string testNonce				= System.Configuration.ConfigurationManager.AppSettings["testNonce"].ToString();
		string testCampaignId			= System.Configuration.ConfigurationManager.AppSettings["testCampaignId"].ToString();
		string testBasicSurveyItemId	= System.Configuration.ConfigurationManager.AppSettings["testBasicNeedsSurveyItemId"].ToString();

		Guid userId = Guid.NewGuid();
		if (User.Identity.IsAuthenticated)
		{
			userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		}

		if (!IsPostBack)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var itemInfo = (from bnsi in dc.BasicNeedsSurveyItems
							join i in dc.Items on bnsi.ItemId equals i.ItemId
							select new { ItemName = i.Name, ItemCount = bnsi.Count, ItemCost = i.Cost }).Take(1).SingleOrDefault();

			hidItemCostEach.Value	= itemInfo.ItemCost.ToString();
			hidItemCount.Value		= itemInfo.ItemCount.ToString();
			decimal? cost			= itemInfo.ItemCost * itemInfo.ItemCount;
			txtAmount.Text			= cost.ToString();
		}
		else
		{
			string nonceFromTheClient = hidNonce.Value;
			string amountStr = txtAmount.Text;
			string message = string.Empty;

			decimal resultDec;

			if (!String.IsNullOrEmpty(amountStr) && decimal.TryParse(amountStr, out resultDec))
			{
				decimal amountDecimal = decimal.Parse(amountStr);

				BraintreeGateway gateway = new BraintreeGateway();

				if (testMode)
				{
					gateway.Environment = Braintree.Environment.SANDBOX;
					gateway.MerchantId = "bzzxdb3b496mcmg8";
					gateway.PublicKey = "8pt4km256dh5d8xm";
					gateway.PrivateKey = "44c448397615c0dbad22baf364ab1885";
					nonceFromTheClient = testNonce;
				}
				else
				{
					gateway.Environment = Braintree.Environment.PRODUCTION;
					gateway.MerchantId = "2fpmr2kcw3km3q6y";
					gateway.PublicKey = "5xf6jgccdtvzykqq";
					gateway.PrivateKey = "3f036ed0d48a2337b0c702043e32b224";
				}

				//Create the client token
				var clientToken = gateway.ClientToken.Generate();

				var request = new TransactionRequest
				{
					Amount				= amountDecimal,
					PaymentMethodNonce	= nonceFromTheClient,
					Customer			= new CustomerRequest
					{
						Id			= userId.ToString(),
						FirstName	= txtFirstName.Text,
						LastName	= txtLastName.Text,
						Phone		= txtPhone.Text,
						Email		= txtEmail.Text
					},
					CustomFields = new Dictionary<string, string>
						{
						///////PUT THE CAMPAIGN ID HERE
							{ "campaignid", "CAMPAIGN ID HERE" },
							{ "userid", userId.ToString() },
							{ "itemcount", hidItemCount.Value },
							{ "itemcosteach", hidItemCostEach.Value }
						},
					Options = new TransactionOptionsRequest
					{
						SubmitForSettlement = true
					}
				};

				Result<Transaction> result = gateway.Transaction.Sale(request);
				bool success = result.IsSuccess();

				message = result.Message;

				if (success)
				{
					Dictionary<string, string> keys = result.Target.CustomFields;
					string campaignid = keys["campaignid"];
					string userid = keys["userid"];

					Transaction transaction = result.Target;

					string transactionId = transaction.Id;
					string orderId = transaction.OrderId;
					decimal? amount = transaction.Amount;
					string authorizedTransactionId = transaction.AuthorizedTransactionId;
					string settlementBatchId = transaction.SettlementBatchId;
					DateTime? createdAt = transaction.CreatedAt;

					//Save in the donation table.

					//Donation donation = new Donation();
					//donation.Amount = amountDecimal;
					//donation.AuthorizedTransactionId = authorizedTransactionId;
					//donation.BasicNeedsSurveyItemId = new Guid(basicSurveyItemId);
					//donation.CampaignId = new Guid(campaignId);
					//donation.CreatedAt = (DateTime) createdAt;
					//donation.DonationId = Guid.NewGuid();
					//donation.EmailAddress = txtEmail.Text;
					//donation.FirstName = txtFirstName.Text;
					//donation.IsTest = testMode;
					//donation.UserId = userId;
					//donation.LastName = txtLastName.Text;
					//donation.OrderId = orderId;
					//donation.PhoneNumber = txtPhone.Text;
					//donation.SettlementBatchId = settlementBatchId;
					//donation.ItemCount = int.Parse(hidItemCount.Value);
					//donation.ItemCostEach = decimal.Parse(hidItemCostEach.Value);

					//CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
					//dc.Donations.InsertOnSubmit(donation);
					//dc.SubmitChanges()



					//string emailMessage = "Thank you for supporting disaster victim" + clientName + " on CrowdRelief.<br/>The information below is your payment reciept.<h3>Payment Reciept</h3>" +
					//	"<table><tr><td>Amount</td><td>Item</td><td>Recipient</td><td>Date</td><td>Transaction Keys</td></tr>" +
					//	"<tr><td>$" + amountDecimal  + "</td><td>" + itemName + "s</td><td>Recipient</td><td>" + createdAt.Value.ToShortDateString() + "</td><td>orderId: " + orderId + "<br/>authorizedTransactionId: " + authorizedTransactionId + " <br/>transactionId: " + transactionId + "<br/>campaignid: " + campaignid + "<br/>userid: " + userid + "</td></tr></table>";

					//Tools.SendEmailTemplate(txtFirstName.Text, txtLastName.Text, txtEmail.Text, emailMessage, "Your donation to a disaster victim on on CrowdRelief", "~\\EmailTemplates\\BaseEmailTemplate.html", this);
				}
				else
				{
					//Post the error message
					divDonationContainer.Visible = true;
					divWarning.Visible = true;
					lblWarning.Text = result.Message;
				}
			}
			else
			{
				lblWarning.Text = "Invalid amount sent.";
			}
		}
	}
}