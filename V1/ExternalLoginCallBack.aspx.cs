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
using System.Text;

public partial class V1_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string code = Request.QueryString["code"];
        string provider = Request.QueryString["provider"]; 

        if (!IsPostBack && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(provider))
        {
            string email = "", firstName = "", lastName = "", picture = "";

            using (var client = new WebClient())
            {
                if (provider == "google")
                {
                    string clientId = ConfigurationManager.AppSettings["GoogleClientId"];
                    string clientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
                    string redirectUri = "http://localhost:64915/V1/ExternalLoginCallBack.aspx?provider=google";

                    var values = new NameValueCollection();
                    values["code"] = code;
                    values["client_id"] = clientId;
                    values["client_secret"] = clientSecret;
                    values["redirect_uri"] = redirectUri;
                    values["grant_type"] = "authorization_code";

                    byte[] response = client.UploadValues("https://oauth2.googleapis.com/token", "POST", values);
                    string result = Encoding.UTF8.GetString(response);
                    dynamic tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    string accessToken = tokenData.access_token;

                    client.Headers.Clear();
                    client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
                    string userInfo = client.DownloadString("https://openidconnect.googleapis.com/v1/userinfo");

                    dynamic user = Newtonsoft.Json.JsonConvert.DeserializeObject(userInfo);
                    email = user.email;
                    firstName = user.given_name;
                    lastName = user.family_name;
                    picture = user.picture;
                }
                else if (provider == "facebook")
                {
                    // FACEBOOK AUTH
                    string appId = ConfigurationManager.AppSettings["FacebookAppId"];
                    string appSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];
                    string redirectUri = "http://localhost:64915/V1/ExternalLoginCallBack.aspx?provider=facebook";

                    string tokenUrl = "https://graph.facebook.com/v19.0/oauth/access_token" +
                  "?client_id=" + appId +
                  "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
                  "&client_secret=" + appSecret +
                      "&code=" + code;

                    string result = client.DownloadString(tokenUrl);
                    dynamic tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    string accessToken = tokenData.access_token;

                    string userInfoUrl = "https://graph.facebook.com/me?fields=id,first_name,last_name,email,picture.width(200)&access_token=" + accessToken;
                    string userInfo = client.DownloadString(userInfoUrl);

                    dynamic user = Newtonsoft.Json.JsonConvert.DeserializeObject(userInfo);
                    email = user.email;
                    firstName = user.first_name;
                    lastName = user.last_name;
                    picture = user.picture.data.url;
                }
            }

            if (!string.IsNullOrEmpty(email))
            {
                string username = email;
                using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
                {
                    string existingUserName = Membership.GetUserNameByEmail(email);
                    if (!string.IsNullOrEmpty(existingUserName))
                    {
                        FormsAuthentication.SetAuthCookie(existingUserName, true);
                        var userId = (from u in dc.aspnet_Users where u.UserName == existingUserName select u.UserId).SingleOrDefault();
                        SendSignInEmail(existingUserName, userId.ToString());
                        Response.Redirect("/feed"); 
                    }
                    else
                    {
                        string randomPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
                        MembershipCreateStatus status;
                        MembershipUser newUser = Membership.CreateUser(username, randomPassword, email, "OAuth", "N/A", true, out status);

                        if (status == MembershipCreateStatus.Success)
                        {
                            Guid photoId = Guid.NewGuid();
                            dc.Photos.InsertOnSubmit(new Photo
                            {
                                PhotoId = photoId,
                                Filename = picture,
                                CreatedOn = DateTime.Now,
                                CreatedBy = new Guid(newUser.ProviderUserKey.ToString()),
                                Hidden = false,
                                Title = "Profile Picture",
                                Description = "OAuth profile picture"
                            });
                            dc.SubmitChanges();

                            dc.Profiles.InsertOnSubmit(new Profile
                            {
                                UserId = new Guid(newUser.ProviderUserKey.ToString()),
                                ProfileId = Guid.NewGuid(),
                                Firstname = firstName,
                                Lastname = lastName,
                                PhotoId = photoId
                            });
                            dc.SubmitChanges();

                            dc.ProfilePhotos.InsertOnSubmit(new ProfilePhoto
                            {
                                ProfilePhotoId = Guid.NewGuid(),
                                UserId = new Guid(newUser.ProviderUserKey.ToString()),
                                PhotoId = photoId,
                                IsCurrrent = true,
                                CreatedOn = DateTime.Now
                            });
                            dc.SubmitChanges();

                            FormsAuthentication.SetAuthCookie(username, true);
                            var userId = (from u in dc.aspnet_Users where u.UserName == username select u.UserId).SingleOrDefault();
                            SendSignInEmail(username, userId.ToString());
                            Response.Redirect("/V1/Profile/EditSkills.aspx?register=true"); 
                        }
                        else
                        {
                            Response.Write("Registration failed: " + status.ToString());
                            Response.End();
                        }
                    }
                }
            }
        }
    }


    private void SendSignInEmail(string username, string userId)
    {
        ListDictionary ldEmailBodyReplacements = new ListDictionary();
        ldEmailBodyReplacements.Add("<% UserName %>", username);
        ldEmailBodyReplacements.Add("<% UserId %>", userId);

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
    }

}