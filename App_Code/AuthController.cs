using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using System.Web.Profile;
using System.Linq;

namespace Stability
{
// Enable CORS for this controller
// [EnableCors(origins: "*", headers: "*", methods: "*")]
[EnableCors(origins: "http://localhost:19006", headers: "*", methods: "*", SupportsCredentials = true)]
[RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        public class RegisterModel { public string FullName { get; set; } public string Email { get; set; } public string Password { get; set; } }
        public class LoginModel { public string Email { get; set; } public string Password { get; set; } }

        [HttpPost, Route("register")]
        public IHttpActionResult Register(RegisterModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("FullName, Email and Password are required.");

            MembershipCreateStatus status;
            var user = Membership.CreateUser(model.Email, model.Password, model.Email, null, null, true, out status);
            if (status != MembershipCreateStatus.Success)
                return BadRequest(status.ToString());

            // Create a profile for this user using the Profile class
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            Profile userProfile = new Profile();
            userProfile.UserId = new Guid(user.ProviderUserKey.ToString());
            userProfile.ProfileId = Guid.NewGuid();
            
            // Parse full name into first and last name
            string[] nameParts = model.FullName.Split(new char[] { ' ' }, 2);
            userProfile.Firstname = nameParts[0];
            userProfile.Lastname = nameParts.Length > 1 ? nameParts[1] : string.Empty;
            
            // Set default values for other fields
            userProfile.PhoneNumber = string.Empty;
            userProfile.Address = string.Empty;
            userProfile.City = string.Empty;
            userProfile.State = string.Empty;
            userProfile.Zip = string.Empty;
            userProfile.ReceiveDeploymentSMS = false;
            
            dc.Profiles.InsertOnSubmit(userProfile);
            dc.SubmitChanges();

            return Created(string.Empty, new { id = user.ProviderUserKey, fullName = model.FullName, email = user.Email });
        }

        [HttpPost, Route("login")]
        public IHttpActionResult Login(LoginModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("Email and Password are required.");

            if (!Membership.ValidateUser(model.Email, model.Password))
                return Unauthorized();

            var user = Membership.GetUser(model.Email);
            
            // Set authentication cookie
            FormsAuthentication.SetAuthCookie(model.Email, true);
            
            // Get user profile from database
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var userProfile = dc.Profiles.FirstOrDefault(p => p.UserId == new Guid(user.ProviderUserKey.ToString()));
            string fullName = userProfile != null ? $"{userProfile.Firstname} {userProfile.Lastname}".Trim() : string.Empty;

            return Ok(new { id = user.ProviderUserKey, fullName, email = user.Email });
        }

        [HttpPost, Route("logout")]
        [Authorize]
        public IHttpActionResult Logout()
        {
            // Sign out the user
            FormsAuthentication.SignOut();
            
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        [HttpGet, Route("me")]
        [Authorize]
        public IHttpActionResult Me()
        {
            var user = Membership.GetUser(User.Identity.Name);
            if (user == null) return Unauthorized();
            
            // Get user profile from database
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var userProfile = dc.Profiles.FirstOrDefault(p => p.UserId == new Guid(user.ProviderUserKey.ToString()));
            string fullName = userProfile != null ? $"{userProfile.Firstname} {userProfile.Lastname}".Trim() : string.Empty;
            
            return Ok(new { id = user.ProviderUserKey, fullName, email = user.Email });
        }
    }
}