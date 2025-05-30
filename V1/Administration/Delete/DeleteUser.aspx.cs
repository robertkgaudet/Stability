using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_Delete_DeleteUser : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string userId = Request.QueryString["userId"];

		if(!String.IsNullOrEmpty(userId))
		{
			//Delete User
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			//Delete the users events
			var userUsersRequesting = from us in dc.UserUsers
									 where us.RequestingUserId == new Guid(userId)
									 select us;

			if (userUsersRequesting.Count() > 0)
			{
				foreach (var userUser in userUsersRequesting)
				{
					dc.UserUsers.DeleteOnSubmit(userUser);
					dc.SubmitChanges();
				}
			}

			//Delete the users events
			var userUsersAccepting = from us in dc.UserUsers
								  where us.AcceptingUserId == new Guid(userId)
								  select us;

			if (userUsersAccepting.Count() > 0)
			{
				foreach (var userUser in userUsersAccepting)
				{
					dc.UserUsers.DeleteOnSubmit(userUser);
					dc.SubmitChanges();
				}
			}

			//Delete the users events
			var userSliderTicks = from us in dc.UserSliderTicks
							 where us.UserId == new Guid(userId)
								 select us;

			if (userSliderTicks.Count() > 0)
			{
				foreach (var userSliderTick in userSliderTicks)
				{
					dc.UserSliderTicks.DeleteOnSubmit(userSliderTick);
					dc.SubmitChanges();
				}
			}

			//Delete the users events
			var userQualifiers = from us in dc.UserQualifiers
									   where us.UserId == new Guid(userId)
									   select us;

			if (userQualifiers.Count() > 0)
			{
				foreach (var userQualifier in userQualifiers)
				{
					dc.UserQualifiers.DeleteOnSubmit(userQualifier);
					dc.SubmitChanges();
				}
			}

			//Delete the users events
			var userInteractionTypes = from us in dc.UserInteractionTypes
							   where us.UserId == new Guid(userId)
							   select us;

			if (userInteractionTypes.Count() > 0)
			{
				foreach (var userInteractionType in userInteractionTypes)
				{
					dc.UserInteractionTypes.DeleteOnSubmit(userInteractionType);
					dc.SubmitChanges();
				}
			}


			//Delete the users events
			var userHousings = from us in dc.UserHousings
							 where us.UserId == new Guid(userId)
							 select us;

			if (userHousings.Count() > 0)
			{
				foreach (var userHousing in userHousings)
				{
					dc.UserHousings.DeleteOnSubmit(userHousing);
					dc.SubmitChanges();
				}
			}

			var userSkills = from us in dc.UserSkills
							 where us.UserId == new Guid(userId)
							 select us;

			//Delete the users events
			if (userSkills.Count() > 0)
			{
				foreach (var userSkill in userSkills)
				{
					dc.UserSkills.DeleteOnSubmit(userSkill);
					dc.SubmitChanges();
				}
			}

			var userEvents = from ue in dc.UserEvents
							 where ue.UserId == new Guid(userId)
							 select ue;

			//Delete the users events
			if (userEvents.Count() > 0)
			{
				foreach (var userEvent in userEvents)
				{
					dc.UserEvents.DeleteOnSubmit(userEvent);
					dc.SubmitChanges();
				}
			}
			var aspNetUsersInRoles = from uir in dc.aspnet_UsersInRoles
									 where uir.UserId == new Guid(userId)
									 select uir;

			//Delete the users roles
			if(aspNetUsersInRoles.Count() > 0)
			{
				foreach(var aspNetUsersInRole in aspNetUsersInRoles)
				{
					dc.aspnet_UsersInRoles.DeleteOnSubmit(aspNetUsersInRole);
					dc.SubmitChanges();
				}
			}

			var userOrganizations = from uir in dc.UserOrganizations
									 where uir.UserId == new Guid(userId) && uir.Status == 1
                                    select uir;

			//Delete the users roles
			if (userOrganizations.Count() > 0)
			{
				foreach (var userOrganization in userOrganizations)
				{
					dc.UserOrganizations.DeleteOnSubmit(userOrganization);
					dc.SubmitChanges();
				}
			}

			var profile = (from p in dc.Profiles
						   where p.UserId == new Guid(userId)
						   select p).SingleOrDefault();
			if(profile != null)
			{ 
				dc.Profiles.DeleteOnSubmit(profile);
				dc.SubmitChanges();
			}

			var aspNetMembership = (from anm in dc.aspnet_Memberships
									where anm.UserId == new Guid(userId)
									select anm).SingleOrDefault();

			if (aspNetMembership != null)
			{
				dc.aspnet_Memberships.DeleteOnSubmit(aspNetMembership);
				dc.SubmitChanges();
			}

			var profilePhoto = (from anu in dc.ProfilePhotos
							  where anu.UserId == new Guid(userId)
							  select anu).SingleOrDefault();

			if (profilePhoto != null)
			{
				dc.ProfilePhotos.DeleteOnSubmit(profilePhoto);
				dc.SubmitChanges();
			}

			var aspNetUser = (from anu in dc.aspnet_Users
						where anu.UserId == new Guid(userId)
						select anu).SingleOrDefault();

			if (aspNetUser != null)
			{
				dc.aspnet_Users.DeleteOnSubmit(aspNetUser);
				dc.SubmitChanges();
			}
		}

		Response.Redirect(Request.UrlReferrer.ToString());
	}
}