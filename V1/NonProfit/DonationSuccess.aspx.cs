using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stripe.Checkout;
using Stripe;
public partial class V1_NonProfit_DonationSuccess : System.Web.UI.Page
{
    public Boolean ShowRegisterButton { get; set; }
    public string TransactionId { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            ShowRegisterButton = true;
        }
        else
        {
            ShowRegisterButton = false;
        }
        string sessionId = Request.QueryString["session_id"].ToString();
        var service = new SessionService();
        var session = service.Get(sessionId); // sessionId is the value from the query string
        this.TransactionId = session.PaymentIntentId;

    }
}