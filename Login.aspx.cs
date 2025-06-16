using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(User.Identity.IsAuthenticated)
		{
			Redirect();
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string username = txtUsername.Text;
		string password = txtPassword.Text;

		int result = 0;

		bool isUserNumber = Int32.TryParse(username, out result);

		if(isUserNumber)
		{
			//Get the username for this user
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var userInfo = (from p in dc.Profiles
						   join u in dc.aspnet_Users on p.UserId equals u.UserId
						   where p.ProfileNumber == result
						   select new {u.UserName }).SingleOrDefault();

			if(userInfo != null)
			{
				username = userInfo.UserName;
			}
		}

		// Validate the user against the Membership framework user store
		if (Membership.ValidateUser(username, password))
		{
			FormsAuthentication.SetAuthCookie(username, true);
			Redirect();

			// Log the user into the site
			//FormsAuthentication.RedirectFromLoginPage(username, true);
		}
	}

	protected void Redirect()
	{
		string returnUrl = Request.QueryString["ReturnUrl"];

		
		string roleType = "helper";
		string urlRedirect = "/V1/Profile/CommunityLandingPage.aspx";
		if (returnUrl != null)
		{
			urlRedirect = returnUrl;
		}
		else
		{

			if (Roles.IsUserInRole("casemanager"))
			{
				urlRedirect = "/CaseManagement/Default.aspx";
				roleType = "CaseManagement";
			}
			else
			{
				if (Roles.IsUserInRole("survivor"))
				{
					//We know their disaster, send them there.
					urlRedirect = "/V1/DisasterList.aspx?userType=survivor";
					roleType = "Survivor";
				}
				if (Roles.IsUserInRole("nonprofitadministrator"))
				{
					urlRedirect = "/V1/DisasterList.aspx?userType=nonprofit";
					roleType = "Nonprofit";
				}
				if (Roles.IsUserInRole("business") || Roles.IsUserInRole("contractor"))
				{
					urlRedirect = "/V1/DisasterList.aspx?userType=business";
					roleType = "Business";
				}
				if (Roles.IsUserInRole("helper") || Roles.IsUserInRole("volunteer"))
				{
					urlRedirect = "/V1/DisasterList.aspx?userType=helper";
					roleType = "Helper";
				}
			}

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			if (Membership.GetUser(txtUsername.Text) != null)
			{
				//Get the latest disaster they registered for.
				var disaster = (from ue in dc.UserEvents
								join d in dc.Events on ue.EventId equals d.EventId
								where ue.UserId == new Guid(Membership.GetUser(txtUsername.Text).ProviderUserKey.ToString())
								orderby d.CreatedOn descending
								select new { d.URLFriendlyName }
								).Take(1).SingleOrDefault();

				if (disaster != null)
				{
					urlRedirect = "/Disaster/" + disaster.URLFriendlyName;// + "/" + roleType;
				}

				//See if they have a default disaster set.
				var profile = (from p in dc.Profiles
							   where p.UserId == new Guid(Membership.GetUser(txtUsername.Text).ProviderUserKey.ToString())
							   select p).SingleOrDefault();

				if (profile != null)
				{
					if (!string.IsNullOrEmpty(profile.DefaultEventId.ToString()))
					{
						var disasterEvent = (from d in dc.Events
											 where d.EventId == profile.DefaultEventId
											 select new { d.URLFriendlyName }
										).Take(1).SingleOrDefault();

						urlRedirect = "/Disaster/" + disasterEvent.URLFriendlyName;// + "/" + roleType;
					}
				}
			}
			else if (User.Identity.IsAuthenticated)
			{
				//See if they have a default disaster set.
				var profile = (from p in dc.Profiles
							   where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
							   select p).SingleOrDefault();

				if (profile != null)
				{
                    if (!string.IsNullOrEmpty(profile.DefaultEventId.ToString()))
                    {
                        var disasterEvent = (from d in dc.Events
											 where d.EventId == profile.DefaultEventId
											 select new { d.URLFriendlyName }
										).Take(1).SingleOrDefault();

						urlRedirect = "/Disaster/" + disasterEvent.URLFriendlyName;// + "/" + roleType;
					}
				}
			}
		}

		Response.Redirect(urlRedirect);
	}
}