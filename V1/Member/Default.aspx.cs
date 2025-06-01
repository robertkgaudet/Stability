using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Net.PeerToPeer;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel.Activities;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Member_Default : BaseWebForm
{
	public string pageUserId = string.Empty;
	public string _receiverUserId = string.Empty;
	public string _sendingUserId = string.Empty;
	bool isSignedInUser = false;

	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string profileImageStyle = string.Empty;
	public string profileNumber = String.Empty;
	public string availabilityStyle = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		pageUserId = Request.QueryString["userId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (User.Identity.IsAuthenticated)
		{	//User is signed in
			if(string.IsNullOrEmpty(pageUserId))
			{
				//SIGNED IN USER
				pageUserId = userId.ToString();
				isSignedInUser = true;
			}
			else if (!string.IsNullOrEmpty(pageUserId))
			{
				//QS was sent, is this the signed in user?
				//userId = new Guid(pageUserId);
				//Check by seeing if the base userId is the same as the id sent on the QS.
				if (pageUserId == userId.ToString())
				{
					//Signed in user, set the signedInUser value to the userId
					_receiverUserId = pageUserId;
					_sendingUserId	= userId.ToString();
					pageUserId = userId.ToString();
					isSignedInUser = true;
					//userId = new Guid(pageUserId);
				}
				else
				{
					_receiverUserId = pageUserId;
					_sendingUserId	= userId.ToString(); 
					isSignedInUser	= false;
				}
			}

			ucMemberHeader.IsSignedInUser = isSignedInUser;
			ucMemberNavigation.UserId = userId.ToString();
		}

		//Does not have to be signed in to do all of this.
		if (!String.IsNullOrEmpty(pageUserId))
		{
			ucMemberHeader.UserId = pageUserId;
			ucDeploymentListCard.UserId = new Guid(pageUserId);
			DeploymentListCard1.UserId = new Guid(pageUserId);
			userId = new Guid(pageUserId);
			ucMemberHeader.UserId = pageUserId;
			//Load profile information.
			var profile = (from p in dc.Profiles
						   join a in dc.aspnet_Users on p.UserId equals a.UserId
						   where p.UserId == new Guid(pageUserId)
						   select new { p, a }).SingleOrDefault();

			//Get profile photo
			if (profile != null)
			{
				if (Roles.IsUserInRole(profile.a.UserName, "Administrator"))
				{
                    
                    ucMemberHeader.BadgeVettingStatus = "fa-approved-color";
					ucMemberHeader.BadgeCertificationStatus = "fa-approved-color";
					ucMemberHeader.BadgeDeployedStatus = "fa-approved-color";
					ucMemberHeader.BadgeHoursRecordedStatus = "fa-approved-color";
					ucMemberHeader.BadgeTOPStatus = "fa-approved-color";
				}


				ucMemberHeader.MemberFullname = profile.p.Firstname + " " + profile.p.Lastname;
             

                ucMemberHeader.MemberDescription = profile.p.Description;
				ucMemberHeader.MemberLocation = profile.p.City + ", " + profile.p.State;
				ucMemberHeader.MemberTitle = profile.p.Title;
				bool passedVetting = Convert.ToBoolean(profile.p.PassedVetting != null ? profile.p.PassedVetting : false);
				if (passedVetting)
				{
					ucMemberHeader.BadgeVettingStatus = "fa-approved-color";
				}

				//Get profile photo
				var profilePhoto = (from ph in dc.Photos
									join pr in dc.ProfilePhotos on ph.PhotoId equals pr.PhotoId
									where pr.UserId == new Guid(pageUserId)
									orderby ph.CreatedOn descending
									select new { ph.FilenameCropped }).Take(1).SingleOrDefault();

				if(profilePhoto != null) 
				{
					ucMemberHeader.MemberProfileImageFilename = profilePhoto.FilenameCropped;
				}
                else
                {
					ucMemberHeader.MemberProfileImageFilename = "profilepicture.png";
				}
			}

			//CHECK THE ADD FRIEND BUTTON
			if(User.Identity.IsAuthenticated && !isSignedInUser)
			{
				//This is A VISITIN USER SO LET"S FIGURE OUT WHICH ADD CONNECTION BUTTON TO SHOW THEM.
				// - Add Connection (blue)
				// - Remove Connection (white)
				// - Pending (white)
				// - Connected (white)
				// - Blocked (white)

				Guid sendingUserId = new Guid(_sendingUserId);
				Guid receiverUserId = new Guid(_receiverUserId);

				//FREIND CHECK
				var UserUser = (from uu in dc.UserUsers
								join uus in dc.UserUserStatus on uu.UserUserStatusId equals uus.UserUserStatusId
								where
								(uu.RequestingUserId == sendingUserId && 
								uu.AcceptingUserId == receiverUserId)
								||
								(uu.RequestingUserId == receiverUserId && 
								uu.AcceptingUserId == sendingUserId)
								select new { uus.Status }).Take(1).SingleOrDefault();

				if(UserUser != null)
				{
					if (UserUser.Status == "Connected")
					{
						ucMemberHeader.FriendStatus = Tools.FriendStatus.Connected;
					
					}
					else if (UserUser.Status == "Pending")
					{ ucMemberHeader.FriendStatus = Tools.FriendStatus.Pending; }
					else if (UserUser.Status == "Delete")
					{ ucMemberHeader.FriendStatus = Tools.FriendStatus.Delete; }
					else if (UserUser.Status == "Blocked")
					{ ucMemberHeader.FriendStatus = Tools.FriendStatus.Blocked; }
				}
				else
				{
					//NO CONNECTION YET EXISTS
					ucMemberHeader.FriendStatus = Tools.FriendStatus.AddConnection;
				}
			}

			//Count number of connections.
			ucMemberHeader.ConnectionCount = Tools.MyConnections(new Guid(pageUserId), 0).Count();

            //Get team information
            var orgUser = (from o in dc.Organizations
                           join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                           where uo.UserId == new Guid(pageUserId) && uo.Status== (int)RequestStatus.Approved
                           orderby o.CreatedOn descending
                           select new
                           {
                               o.OrganizationId,
                               o.Name,
                               o.LogoSquare,
                               uo.ShowTeamLogo  // Include the ShowTeamLogo field
                           }).Take(1).SingleOrDefault();
            if (orgUser != null)
            {
                //ucMemberNavigation.OrganizationId = orgUser.OrganizationId.ToString();
                ucMemberHeader.TeamId = orgUser.OrganizationId.ToString();
                ucMemberHeader.TeamName = orgUser.Name;
                ucMemberHeader.TeamLogo = orgUser.LogoSquare;
                
            }


            //Count deployments
            var deployments = (from ue in dc.UserEvents
							  join uoe in dc.UserOrganizationEvents on ue.UserId equals uoe.UserId
							  join oe in dc.OrganizationEvents on uoe.OrganizationEventId equals oe.OrganizationEventId
							  join org in dc.Organizations on oe.OrganizationId equals org.OrganizationId
							  join s in dc.USStates on oe.StagingStateId equals s.StatesId
							  join c in dc.Counties on oe.StagingCountyId equals c.CountyId
							  where uoe.UserId == new Guid(pageUserId)
							  select new { uoe }).Distinct().Count();

			ucMemberHeader.DeploymentCount = deployments.ToString();
			//if(Session["deploymentCount"] != null)
			//{
			//	deploymentCount = (Session["deploymentCount"] != null) ? Convert.ToInt32(Session["deploymentCount"]) : 0;

			//}
			//ucMemberHeader.DeploymentCount = deploymentCount.ToString(); // deployments.Count().ToString();


			//SKILLS AND RESOURCES CODE
			var userSkills = from us in dc.UserSkills
							 join s in dc.Skills on us.SkillId equals s.SkillId
							 orderby s.Name
							 where us.UserId == new Guid(pageUserId)
							 select s.Name;  // Assuming you want to select the 'SkillName' field


			var pillHtml = new StringBuilder();

			foreach (var skill in userSkills)
			{
				pillHtml.AppendFormat(
					"<span class='skill-pill'>{0}</span> ",
					System.Web.HttpUtility.HtmlEncode(skill)
				);
			}

			litSkills.Text = pillHtml.ToString();

			//SKILLS AND RESOURCES CODE
			var userResources = from ur in dc.UserResources
							 join r in dc.Resources on ur.ResourceId equals r.ResourceId
							 orderby r.Name
							 where ur.UserId == new Guid(pageUserId)
								select r.Name;  // Assuming you want to select the 'SkillName' field

			pillHtml.Clear();
			foreach (var equipmentItem in userResources)
			{
				pillHtml.AppendFormat(
					"<span class='skill-pill'>{0}</span> ",
					System.Web.HttpUtility.HtmlEncode(equipmentItem)
				);
			}
			litResources.Text = pillHtml.ToString();




			//CALENDAR CODE
			DateTime today = DateTime.Today;
			DateTime twoWeeksFromNow = today.AddDays(365);

			// LINQ query to fetch the users' availability over the next two weeks
			var userAvailability = dc.UserAvailableDates
				.Where(uad => uad.DateAvailable >= today && uad.DateAvailable <= twoWeeksFromNow && uad.UserId == new Guid(pageUserId))
				.OrderBy(uad => uad.DateAvailable)
				.Take(7)
				.Select(uad => new
				{
					uad.UserAvailableDateId,
					uad.UserId,
					DayOfWeek = uad.DateAvailable.ToString("ddd", CultureInfo.InvariantCulture),  // Full day of the week
					MonthAbbreviation = uad.DateAvailable.ToString("MMM", CultureInfo.InvariantCulture),  // Abbreviated month
					DayOfMonth = uad.DateAvailable.Day.ToString(),  // Day of the month
					Year = uad.DateAvailable.Year.ToString(),  // Year
					uad.DateAvailable,
					uad.DateCreated
				})
				.ToList();

			litDatesAvailable.Text = string.Empty;

			string datesAvailable = string.Empty;
			bool availableTodayCheck = false;
			availabilityStyle = "alert-warning";
			// Example of iterating through the results
			foreach (var availability in userAvailability)
			{
				string availableToday = string.Empty;
				if(DateTime.Today == availability.DateAvailable && !availableTodayCheck)
				{
					availableToday = "<div class=\"col-xs-4 col-lg-2 text-center\"><button class=\"btn btn-success font-small\" type=\"button\"><i class=\"fa fa-calendar\"></i> </br>AVAILABLE<br/>TODAY</button></div>";
				}
				else if(!availableTodayCheck)
				{
					availableToday = "<div class=\"col-xs-5 col-lg-3 text-center\"><button class=\"btn btn-warning2 font-small\" type=\"button\"><i class=\"fa fa-ban\"></i> </br>NOT TODAY</button></div>";
				}
				datesAvailable += availableToday + "<div class=\"col-xs-2 col-lg-1 text-center calendar\"><div class=\"calendar-month-day\">" + availability.DayOfWeek + "</br><span class=\"calendar-date-of-month\">" + availability.MonthAbbreviation  + " " + availability.DayOfMonth + "</span></div><div class=\"calendar-year\">" + availability.Year +  "</div></div>";
				availableTodayCheck = true;
				availableToday = string.Empty;
			}

			if(isSignedInUser)
			{
				btnUpdateCal.Visible = true;
				btnUpdateCalendar.Visible = true;
				btnUpdateSkills.Visible = true;
				btnUpdateEquipment.Visible = true;
			}
			if(!String.IsNullOrEmpty(datesAvailable))
			{
				availabilityStyle = "alert-success";
				litDatesAvailable.Text = "<div class=\"row no-gutter\">" + datesAvailable + "</div>";
			}
			else
			{
				//No dates...
				btnUpdateCalendar.Visible = false;
				divNoDates.Visible = true;
			}
			if(!isSignedInUser)
			{
				//Hide update buttons from non-signed in users
				btnUpdateCalendar.Visible = false;
				btnUpdateCal.Visible = false;
			}
			//CALENDAR CODE
		}
	}

	[WebMethod]
	public static string RequestConnection(string receiverUserIdString, string requestorUserIdString)
	{
		string result = "success";
		Guid receiverUserId = new Guid(receiverUserIdString);
		Guid requestorUserId = new Guid(requestorUserIdString);

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//Make sure there is not a request yet, then add it.
		int UserUser = (from uu in dc.UserUsers
					   where
					   (uu.RequestingUserId == requestorUserId && uu.AcceptingUserId == receiverUserId)
					   ||
					   (uu.RequestingUserId == receiverUserId && uu.AcceptingUserId == requestorUserId)
					   select new { uu }).Count();

		var UserUserStatus = (from uus in dc.UserUserStatus
							  where uus.Status == "Pending"
							  select new { uus.UserUserStatusId }).SingleOrDefault();

		if(UserUser == 0)
		{
			//No relationship exists yet.
			//Add it
			Guid userUserRelationshipId = new Guid("aae059ad-b986-4675-a86c-039b28e6b296");//FRIEND TYPE ID
			UserUser userUser = new UserUser();
			userUser.UserUserId = Guid.NewGuid();
			userUser.RequestingUserId = requestorUserId;
			userUser.AcceptingUserId = receiverUserId;
			userUser.UserUserRelationshipId = userUserRelationshipId;
			userUser.RequestedOn = DateTime.Now;
			userUser.UserUserStatusId = UserUserStatus.UserUserStatusId;//Pending
			userUser.IsActive = false;
			dc.UserUsers.InsertOnSubmit(userUser);
			dc.SubmitChanges();


			//Get info to send email to the recipient.
			string recipientsEmail = string.Empty;
			string recipientsName = string.Empty;
			string sendersName = string.Empty;

			var recipientsProfile = (from p in dc.Profiles
									 join a in dc.aspnet_Memberships on p.UserId equals a.UserId
									 where p.UserId == receiverUserId
									 select new { p.Firstname, a.Email }).SingleOrDefault();

			if (recipientsProfile != null)
			{
				recipientsEmail = recipientsProfile.Email;
				recipientsName = recipientsProfile.Firstname;

				var sendersProfile = (from p in dc.Profiles
									  join a in dc.aspnet_Memberships on p.UserId equals a.UserId
									  where p.UserId == requestorUserId
									  select new { Fullname = p.Firstname + " " + p.Lastname }).SingleOrDefault();

				if (sendersProfile != null)
				{
					sendersName = sendersProfile.Fullname;
				}
			}


			SendConnectionEmail(sendersName, receiverUserId.ToString(), recipientsEmail, recipientsName);
		}
		else
		{
			result = "error";
		}

			return result;
	}

	public static void SendConnectionEmail(string SenderName, string receiverUserId, string recipientsEmail, string recipientsName)
	{
		//Send an email.
		ListDictionary ldEmailBodyReplacements = new ListDictionary();
		ldEmailBodyReplacements.Add("<% SenderName %>", SenderName);
		ldEmailBodyReplacements.Add("<% RecipientsUserId %>", receiverUserId);

		string error = string.Empty;
		Tools.SendEmail(
		string.Empty,
		SenderName + " sent you a connection request on Stability",
		ldEmailBodyReplacements,
		recipientsEmail,
		recipientsName,
		string.Empty,
		string.Empty,
		"~\\EmailTemplates\\FriendInvitation.html",
		out error);
	}
}