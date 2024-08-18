using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Member_Default : BaseWebForm
{
	public string _userId = string.Empty;
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string profileImageStyle = string.Empty;
	public string profileNumber = String.Empty;
	bool isSignedInUser = false;
	public string availabilityStyle = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		_userId = Request.QueryString["userId"];
		profileNumber = Request.QueryString["profileNumber"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if (!string.IsNullOrEmpty(profileNumber) && string.IsNullOrEmpty(_userId))
		{
			//Get this users userId
			var userIdFromProfileNumber = (from p in dc.Profiles
										   where p.ProfileNumber == Int32.Parse(profileNumber)
										   select new { p.UserId }).SingleOrDefault();

			_userId = userIdFromProfileNumber.UserId.ToString();
		}

		if (User.Identity.IsAuthenticated)
		{	//User is signed in
			if(string.IsNullOrEmpty(_userId))
			{
				//If no userid was sent on qs, then set to the signed in user.
				_userId = userId.ToString();
				isSignedInUser = true;
			}
			else if (_userId == userId.ToString())
			{
				//SIGNED IN USER
				isSignedInUser = true;
			}
			ucMemberHeader.IsSignedInUser = isSignedInUser;
		}
		if(!String.IsNullOrEmpty(_userId))
		{
			ucMemberHeader.UserId = _userId;
			//Load profile information.
			var profile = (from p in dc.Profiles
						   where p.UserId == new Guid(_userId)
						   select p).SingleOrDefault();

			if (profile != null)
			{
				ucMemberHeader.MemberFullname = profile.Firstname + " " + profile.Lastname;
				ucMemberHeader.MemberDescription = profile.Description;
				ucMemberHeader.MemberLocation = profile.City + ", " + profile.State;
				ucMemberHeader.MemberTitle = profile.Title;

				//Get profile photo
				var profilePhoto = (from ph in dc.Photos
									join pr in dc.ProfilePhotos on ph.PhotoId equals pr.PhotoId
									where pr.UserId == new Guid(_userId)
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



			//SKILLS AND RESOURCES CODE
			var userSkills = from us in dc.UserSkills
							 join s in dc.Skills on us.SkillId equals s.SkillId
							 orderby s.Name
							 where us.UserId == userId
							 select s.Name;  // Assuming you want to select the 'SkillName' field

			// Create a single comma-separated list of the skill names
			litSkills.Text = "<b>Skills: </b>" + string.Join(" • ", userSkills.ToList());



			//SKILLS AND RESOURCES CODE
			var userResources = from ur in dc.UserResources
							 join r in dc.Resources on ur.ResourceId equals r.ResourceId
							 orderby r.Name
							 where ur.UserId == userId
							 select r.Name;  // Assuming you want to select the 'SkillName' field

			// Create a single comma-separated list of the skill names
			litResources.Text = "<b>Resources: </b>" + string.Join(" • ", userResources.ToList());

			//CALENDAR CODE
			DateTime today = DateTime.Today;
			DateTime twoWeeksFromNow = today.AddDays(365);

			// LINQ query to fetch the users' availability over the next two weeks
			var userAvailability = dc.UserAvailableDates
				.Where(uad => uad.DateAvailable >= today && uad.DateAvailable <= twoWeeksFromNow && uad.UserId == new Guid(_userId))
				.OrderBy(uad => uad.DateAvailable)
				.Take(9)
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

			litDatesAvailable.Text = "Member has no dates available.";
			string datesAvailable = string.Empty;
			bool availableTodayCheck = false;
			availabilityStyle = "alert-warning";
			// Example of iterating through the results
			foreach (var availability in userAvailability)
			{
				string availableToday = string.Empty;
				if(DateTime.Today == availability.DateAvailable && !availableTodayCheck)
				{
					availableToday = "<div class=\"col-xs-4 col-lg-2 text-center\"><button class=\"btn btn-success font-small\" type=\"button\"><i class=\"fa fa-calendar\"></i> </br>TODAY</button></div>";
				}
				else if(!availableTodayCheck)
				{
					availableToday = "<div class=\"col-xs-5 col-lg-3 text-center\"><button class=\"btn btn-warning2 font-small\" type=\"button\"><i class=\"fa fa-ban\"></i> </br>NOT TODAY</button></div>";
				}
				datesAvailable += availableToday + "<div class=\"col-xs-2 col-lg-1 text-center calendar\"><div class=\"calendar-month-day\">" + availability.DayOfWeek + " " + availability.MonthAbbreviation + "</br><span class=\"calendar-date-of-month\">" + availability.DayOfMonth + "</span></div><div class=\"calendar-year\">" + availability.Year +  "</div></div>";
				availableTodayCheck = true;
				availableToday = string.Empty;
			}

			if(!String.IsNullOrEmpty(datesAvailable))
			{
				availabilityStyle = "alert-success";
				litDatesAvailable.Text = "<div class=\"row no-gutter\">" + datesAvailable + "</div>";
			}
			else
			{
				btnUpdateCalendar.Visible = true;
			}
			//CALENDAR CODE
		}
	}
}