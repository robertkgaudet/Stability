using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IdentityModel.Metadata;

public partial class V1_Login : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (User.Identity.IsAuthenticated)
		{
			//Redirect(User.Identity.Name);
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

			//Response.Redirect("V1/NonProfit/TakeAction.aspx?organizationId=e1c2150a-056c-45dc-9cdc-31153384e732");
			Redirect(username);

			// Log the user into the site
			//FormsAuthentication.RedirectFromLoginPage(username, true);
		}
	}

	protected void Redirect(string username)
	{
		//SEND USER TO THEIR MOST RECENT CAUSE PAGE.

		string urlRedirect = string.Empty;

		MembershipUser user = Membership.GetUser(txtUsername.Text);

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        if (user != null)
		{
			//If user has a nonprofit org, send them to a page with their team members and their causes.

			//note, user might have more than org... 
			var userOrganization = (from uo in dc.UserOrganizations
										 where uo.UserId == new Guid(user.ProviderUserKey.ToString())
										 select new { uo.OrganizationId }).Take(1).SingleOrDefault();

			if(userOrganization != null) 
			{ 
				urlRedirect = "/V1/NonProfit/TakeAction.aspx?OrganizationId=" + userOrganization.OrganizationId.ToString();
			}
			else
			{
				urlRedirect = "/V1/Profile/EditNonProfits.aspx";
			}
			//if (userOrganizationOwner != null)
			//{
			//	//SEND TO PAGE TO SEE CAUSES.
			//	urlRedirect = "/V1/NonProfit/NonProfit.aspx?OrganizationId=" + userOrganizationOwner.OrganizationId.ToString();
			//}
			//else
			//{
			//	//Get the latest disaster they registered for.
			//	var disaster = (from ue in dc.UserEvents
			//					join d in dc.Events on ue.EventId equals d.EventId
			//					where ue.UserId == new Guid(Membership.GetUser(txtUsername.Text).ProviderUserKey.ToString())
			//					orderby d.CreatedOn descending
			//					select new {d.URLFriendlyName }
			//					).Take(1).SingleOrDefault();

			//	if(disaster != null)
			//	{
			//		urlRedirect = "/Disaster/" + disaster.URLFriendlyName;// + "/" + roleType;
			//	}

			//	//See if they have a default disaster set.
			//	var profile = (from p in dc.Profiles
			//				  where p.UserId == new Guid(Membership.GetUser(txtUsername.Text).ProviderUserKey.ToString())
			//				  select p).SingleOrDefault();

			//	if(profile != null)
			//	{
			//		if(profile.DefaultEventId != null)
			//		{
			//			var disasterEvent = (from d in dc.Events
			//							where d.EventId == profile.DefaultEventId
			//							select new {d.URLFriendlyName }
			//							).Take(1).SingleOrDefault();

			//			urlRedirect = "/Disaster/" + disasterEvent.URLFriendlyName;// + "/" + roleType;
			//		}
			//	}
			//}
		}
    //    else if(User.Identity.IsAuthenticated)
    //    {
    //        //See if they have a default disaster set.
    //        var profile = (from p in dc.Profiles
    //                       where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
    //                       select p).SingleOrDefault();

    //        if (profile != null)
    //        {
    //            if (profile.DefaultEventId != null)
    //            {
    //                var disasterEvent = (from d in dc.Events
    //                                     where d.EventId == profile.DefaultEventId
    //                                     select new { d.URLFriendlyName }
    //                                ).Take(1).SingleOrDefault();

    //                urlRedirect = "/Disaster/" + disasterEvent.URLFriendlyName;// + "/" + roleType;
				//}
    //        }
    //    }

		Response.Redirect(urlRedirect);
	}
}