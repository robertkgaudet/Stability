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

      
            if (!string.IsNullOrEmpty(code))
            {
                string clientId = ConfigurationManager.AppSettings["GoogleClientId"];
                string clientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
                string redirectUri = "http://localhost:64915/V1/ExternalLoginCallBack.aspx";            

                using (var client = new WebClient())
                {
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
                    string email = user.email;
                    string firstName = user.given_name;
                    string lastName = user.family_name;
                     string picture = user.picture;
                  Session["GoogleUserEmail"] = email;

                    string username = email; // ya apna username logic
                    using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
                    {
                        string existingUserName = Membership.GetUserNameByEmail(email);
                        if (!string.IsNullOrEmpty(existingUserName))
                        {
                            // User exists - login
                            FormsAuthentication.SetAuthCookie(existingUserName, true);

                            var userId = (from u in dc.aspnet_Users
                                          where u.UserName == existingUserName
                                          select u.UserId).SingleOrDefault();

                            SendSignInEmail(existingUserName, userId.ToString());

                            Response.Redirect("/feed");
                        }
                        else
                        {
                            // User nahi mila, naya create karo
                            string randomPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
                            string passwordQuestion = "Google OAuth Registration";
                            string passwordAnswer = "N/A";

                            MembershipCreateStatus status;
                            MembershipUser newUser = Membership.CreateUser(username, randomPassword, email, passwordQuestion, passwordAnswer, true, out status);

                            if (newUser == null || status != MembershipCreateStatus.Success)
                            {
                                Response.Write("User registration failed: " + status.ToString());
                                Response.End();
                            }
                            else
                            {
                            Guid photoId = Guid.NewGuid();
                            var photo = new Photo()
                            {
                                PhotoId = photoId,
                                Filename = picture,
                                FilenameCropped = null,
                                FilenameResized = null,
                                CreatedOn = DateTime.Now,
                                CreatedBy = new Guid(newUser.ProviderUserKey.ToString()),
                                Hidden = false,
                                Title = "Profile Picture",
                                Description = "Google profile picture uploaded on registration"
                            };

                            dc.Photos.InsertOnSubmit(photo);
                            dc.SubmitChanges();

                            Profile userProfile = new Profile();
                            userProfile.UserId = new Guid(newUser.ProviderUserKey.ToString());
                            userProfile.ProfileId = Guid.NewGuid();
                            userProfile.Firstname = firstName;
                            userProfile.Lastname = lastName;
                            userProfile.PhoneNumber = "";
                            userProfile.Address = "";
                            userProfile.City = "";
                            userProfile.State = "";
                            userProfile.Zip = "";
                            userProfile.ReceiveDeploymentSMS = false;
                            userProfile.PhotoId = photoId;  

                            dc.Profiles.InsertOnSubmit(userProfile);
                            dc.SubmitChanges();
                            var profilePhoto = new ProfilePhoto()
                            {
                                ProfilePhotoId = Guid.NewGuid(),
                                UserId = new Guid(newUser.ProviderUserKey.ToString()),
                                PhotoId = photoId,
                                IsCurrrent = true,
                                CreatedOn = DateTime.Now
                            };

                            dc.ProfilePhotos.InsertOnSubmit(profilePhoto);
                            dc.SubmitChanges();

                            FormsAuthentication.SetAuthCookie(username, true);

                                var userId = (from u in dc.aspnet_Users
                                              where u.UserName == username
                                              select u.UserId).SingleOrDefault();

                                SendSignInEmail(username, userId.ToString());

                                Response.Redirect("/V1/Profile/EditSkills.aspx?register=true");
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