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
using System.IO;

public partial class V1_Login : System.Web.UI.Page
{
    public Guid? photoId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string code = Request.QueryString["code"];
            string provider = Request.QueryString["provider"];

            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(provider))
            {
                HandleExternalLogin(code, provider);
            }
        }
    }
    private void HandleExternalLogin(string code, string provider)
    {
        string email = "", firstName = "", lastName = "", picture = "";

        using (var client = new WebClient())
        {
            if (provider == "google")
            {
                // GOOGLE
                string clientId = ConfigurationManager.AppSettings["GoogleClientId"];
                string clientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
                string redirectUri = ConfigurationManager.AppSettings["GoogleRedirectUri"];

                var values = new NameValueCollection();
                values.Add("code", code);
                values.Add("client_id", clientId);
                values.Add("client_secret", clientSecret);
                values.Add("redirect_uri", redirectUri);
                values.Add("grant_type", "authorization_code");

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

                try
                {
                    picture = user.picture != null ? user.picture.ToString() : null;
                }
                catch { picture = null; }
            }
            else if (provider == "facebook")
            {
                // FACEBOOK
                string appId = ConfigurationManager.AppSettings["FacebookAppId"];
                string appSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];
                string redirectUri = ConfigurationManager.AppSettings["FacebookRedirectUri"];

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

                try
                {
                    if (user.picture != null && user.picture.data != null && user.picture.data.url != null)
                    {
                        picture = user.picture.data.url.ToString();
                    }
                }
                catch { picture = null; }
            }
        }

        if (!string.IsNullOrEmpty(email))
        {
            ProcessUserLoginOrRegistration(email, firstName, lastName, picture);
        }
    }
    private void ProcessUserLoginOrRegistration(string email, string firstName, string lastName, string picture)
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
                Response.Redirect("/V1/Profile/CommunityLandingPage.aspx?login=true");
            }
            else
            {
                string randomPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
                MembershipCreateStatus status;
                MembershipUser newUser = Membership.CreateUser(username, randomPassword, email, "OAuth", "N/A", true, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    Guid userId = new Guid(newUser.ProviderUserKey.ToString());

                    if (!string.IsNullOrEmpty(picture))
                    {
                        SaveExternalProfilePicture(picture, userId);
                    }
                    Profile profile = new Profile
                    {
                        UserId = userId,
                        ProfileId = Guid.NewGuid(),
                        Firstname = firstName ?? "",
                        Lastname = lastName ?? "",
                        PhotoId=photoId
                    };

                    dc.Profiles.InsertOnSubmit(profile);
                    dc.SubmitChanges();
                    FormsAuthentication.SetAuthCookie(username, true);
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
    private void SaveExternalProfilePicture(string pictureUrl, Guid userId)
    {
        try
        {
            string folder = ConfigurationManager.AppSettings["profilePhotoFolder"];
            int width = int.Parse(ConfigurationManager.AppSettings["profileImageWidth"]);
            int height = int.Parse(ConfigurationManager.AppSettings["profileImageHeight"]);

            string imageGuid = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(pictureUrl);
            if (string.IsNullOrEmpty(extension) || extension.Length > 5) extension = ".jpg";

            string nameOriginal = imageGuid + extension;
            string nameCropped = imageGuid  + extension;
            string nameResized = imageGuid + extension;

            string folderPath = Server.MapPath(folder);
            string pathOriginal = Path.Combine(folderPath, nameOriginal);
            string pathResized = Path.Combine(folderPath, nameResized);

            using (var client = new WebClient())
            {
                client.DownloadFile(pictureUrl, pathOriginal);
            }

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                Photo photo = new Photo
                {
                    PhotoId = Guid.NewGuid(),
                    Filename = nameOriginal,
                    FilenameCropped = nameCropped,
                    FilenameResized = nameResized,
                    Title = "Profile Photo",
                    Description = "Profile Photo",
                    Hidden = false,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId
                };
                dc.Photos.InsertOnSubmit(photo);

                ProfilePhoto pp = new ProfilePhoto
                {
                    ProfilePhotoId = Guid.NewGuid(),
                    UserId = userId,
                    PhotoId = photo.PhotoId,
                    IsCurrrent = true
                };
                dc.ProfilePhotos.InsertOnSubmit(pp);
                dc.SubmitChanges();
               photoId=photo.PhotoId;
            }

           
        }
        catch
        {
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
