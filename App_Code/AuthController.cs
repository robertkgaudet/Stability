using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using System.Web.Profile;

namespace Stability
{
// Enable CORS for this controller
[EnableCors(origins: "*", headers: "*", methods: "*")]
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

            // Save FullName into profile
            var profile = ProfileBase.Create(model.Email);
            profile.SetPropertyValue("FullName", model.FullName);
            profile.Save();

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
            // Generate a simple token (replace with JWT as needed)
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var expiresIn = 3600; // seconds
            var profile = ProfileBase.Create(model.Email);
            var fullName = profile.GetPropertyValue("FullName") as string;

            return Ok(new { token, expiresIn, user = new { id = user.ProviderUserKey, fullName, email = user.Email } });
        }

        [HttpPost, Route("logout")]
        [Authorize]
        public IHttpActionResult Logout()
        {
            // Token-based logout: client can discard token
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        [HttpGet, Route("me")]
        [Authorize]
        public IHttpActionResult Me()
        {
            var user = Membership.GetUser(User.Identity.Name);
            if (user == null) return Unauthorized();
            var profile = ProfileBase.Create(user.UserName);
            var fullName = profile.GetPropertyValue("FullName") as string;
            return Ok(new { id = user.ProviderUserKey, fullName, email = user.Email });
        }
    }
}