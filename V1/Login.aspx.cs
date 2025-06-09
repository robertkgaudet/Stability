using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IdentityModel.Metadata;
using CrowdRelief;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;

public partial class V1_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/V1/Member/Default.aspx", false);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;

        int result = 0;

        bool isUserNumber = Int32.TryParse(username, out result);

        if (isUserNumber)
        {
            //Get the username for this user
            CrowdReliefDBDataContext dc1 = new CrowdReliefDBDataContext();

            var userInfo = (from p in dc1.Profiles
                            join u in dc1.aspnet_Users on p.UserId equals u.UserId
                            where p.ProfileNumber == result
                            select new { u.UserName }).SingleOrDefault();

            if (userInfo != null)
            {
                username = userInfo.UserName;
            }
        }
		var user = Membership.GetUser(username);
		if (user == null)
		{
			lblErrorMessage.Text = "Username does not exist.";
			lblErrorMessage.Visible = true;
		}
		else if (!Membership.ValidateUser(username, password))
		{
			lblErrorMessage.Text = "Incorrect password.";
			lblErrorMessage.Visible = true;
		}
		else
		{
			// Success: proceed with login

			// Validate the user against the Membership framework user store
			if (Membership.ValidateUser(username, password))
			{
				FormsAuthentication.SetAuthCookie(username, true);

				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
				var userId = (from u in dc.aspnet_Users
							  where u.UserName == username
							  select u.UserId).SingleOrDefault();
				ListDictionary ldEmailBodyReplacements = new ListDictionary();
				ldEmailBodyReplacements.Add("<% UserName %>", username);
				ldEmailBodyReplacements.Add("<% UserId %>", userId.ToString());
				string error = string.Empty;
				Tools.SendEmail(
				string.Empty,
				"Stability User Has Signed In",
				ldEmailBodyReplacements,
				"robertkgaudet@gmail.com",
				"Stability Login Alert",
				string.Empty,
				string.Empty,
				"~\\EmailTemplates\\SignIn.html",
				out error);

				Redirect(username);
			}
			else
			{
				//Error
				lblErrorMessage.Text = "Unknown error, check your username or password and try again.";
				lblErrorMessage.Visible = true;
			}
		}
	}
    protected void Redirect(string username)
    {
        //SEND USER TO THEIR MOST RECENT CAUSE PAGE.
        string returnUrl = Request.QueryString["ReturnUrl"];

        string urlRedirect = string.Empty;
        if (returnUrl != null)
        {
            urlRedirect = returnUrl;
        }
        else
        {
            MembershipUser user = Membership.GetUser(txtUsername.Text);

            if (user != null)
            {
                urlRedirect = "/V1/Member/Default.aspx";

                //Does the user belong to a team yet?
                //If not, send to the team page.
                CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
                var organizationUser = from ou in dc.UserOrganizations
                                       where ou.UserId == new Guid(user.ProviderUserKey.ToString()) && ou.Status == (int)RequestStatus.Approved
                                       select ou;

                if (organizationUser == null)
                {
                    urlRedirect = "/V1/NonProfit/TeamList.aspx?team=false";
                }
                else
                {
                    //Does the team have a deployment the user can join.
                }
            }
        }

        Response.Redirect(urlRedirect);
    }
    protected void btnGoogle_Click(object sender, EventArgs e)
    {
        string clientId = ConfigurationManager.AppSettings["GoogleClientId"];
        string redirectUri = "http://localhost:64915/V1/ExternalLoginCallBack.aspx";
        string googleUrl = "https://accounts.google.com/o/oauth2/v2/auth" +
            "?response_type=code" +
            "&scope=email%20profile" +
            "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
            "&client_id=" + clientId +
            "&access_type=online";

        Response.Redirect(googleUrl);
    }
    //protected void btnFacebook_Click(object sender, EventArgs e)
    //{
    //    string fbAppId = ConfigurationManager.AppSettings["FacebookAppId"];
    //    string redirectUri = ConfigurationManager.AppSettings["FacebookRedirectUri"];

    //    string fbLoginUrl = "https://www.facebook.com/v17.0/dialog/oauth?" +
    //        "client_id=" + fbAppId +
    //        "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
    //        "&response_type=code" +
    //        "&scope=email,public_profile";

    //    Response.Redirect(fbLoginUrl);
    //}
}
