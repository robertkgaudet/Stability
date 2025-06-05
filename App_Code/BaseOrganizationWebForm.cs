using System;
using System.Linq;
using System.Web.Security;
using System.Web.SessionState;




public class VolunteerStatus
{
	private VolunteerStatus(string value)
	{ Value = value; }

	public string Value
	{ get; set; }

	public static VolunteerStatus NotYetApplied
	{ get { return new VolunteerStatus("NotYetApplied"); } }
	public static VolunteerStatus ApplicationComplete
	{ get { return new VolunteerStatus("ApplicationComplete"); } }
	public static VolunteerStatus VettingStarted
	{ get { return new VolunteerStatus("VettingStarted"); } }
	public static VolunteerStatus VettingComplete_Failed
	{ get { return new VolunteerStatus("VettingComplete_Failed"); } }
	public static VolunteerStatus VettingComplete_Passed
	{ get { return new VolunteerStatus("VettingComplete_Passed"); } }



	public static VolunteerStatus GetVolunteerStatus(Guid userId)
	{
		VolunteerStatus volunteerStatus = VolunteerStatus.NotYetApplied;

		if (userId != null && userId != Guid.Empty)
		{
			//Get the volunteer status and return it.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var profile = (from p in dc.Profiles
						   where p.UserId == userId
						   select p).SingleOrDefault();

			if (profile != null)
			{
				if ((profile.VolunteerApplicationIsComplete != null) && (Convert.ToBoolean(profile.VolunteerApplicationIsComplete)) && (profile.PassedVetting == null) && (profile.VettingActive == null))
				{
					//Application is complete
					volunteerStatus = VolunteerStatus.ApplicationComplete;
				}
				else if ((profile.VettingActive != null) && (Convert.ToBoolean(profile.VettingActive)))
				{
					//Vetting started
					volunteerStatus = VolunteerStatus.VettingStarted;
				}
				else if ((profile.PassedVetting != null) && (Convert.ToBoolean(profile.PassedVetting)))
				{
					//Vetting Complete Passed
					volunteerStatus = VolunteerStatus.VettingComplete_Passed;
				}
				else if ((profile.PassedVetting != null) && (!Convert.ToBoolean(profile.PassedVetting)))
				{
					//Vetting Complete Failed
					volunteerStatus = VolunteerStatus.VettingComplete_Failed;
				}
			}
		}

		return volunteerStatus;
	}



}


/// <summary>
/// Summary description for BaseOrganizationWebForm
/// </summary>
public class BaseOrganizationWebForm : System.Web.UI.Page, IRequiresSessionState
{
	private Guid m_userId;
	private Guid m_organizationId;
	private string m_organizationName;
	private string m_organizationLogo;
	private string m_userFirstname;
	private string m_userLastname;
	private string m_userFullname;
	private string m_userPhonenumber;
	private string m_userEmail;
	private string m_userUsername;
	private string m_progress;
	private Guid m_userOrganizationId;

	public BaseOrganizationWebForm()
	{
		if (User.Identity.IsAuthenticated)
		{
			if(Membership.GetUser().ProviderUserKey != null)
			{ 
				m_userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

				var userOrganization = (from uo in dc.UserOrganizations
										join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
										join p in dc.Profiles on uo.UserId equals p.UserId
										join m in dc.aspnet_Memberships on uo.UserId equals m.UserId
										join u in dc.aspnet_Users on uo.UserId equals u.UserId
										where uo.UserId == m_userId && (uo.Status== (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                                        && uo.IsPrimary == true
										orderby o.CreatedOn descending
										select new { o.Name, o.OrganizationId, o.Logo, p.Firstname, p.Lastname, p.Photo, fullname = (p.Firstname + " " + p.Lastname), p.PhoneNumber, m.Email, u.UserName }).Take(1).SingleOrDefault();
			
			
				if (userOrganization != null)
				{
					m_organizationId = userOrganization.OrganizationId;
					m_organizationName = userOrganization.Name;
					m_organizationLogo = userOrganization.Logo;
					m_userFirstname = userOrganization.Firstname;
					m_userLastname = userOrganization.Lastname;
					m_userFullname = userOrganization.fullname;
					m_userPhonenumber = userOrganization.PhoneNumber;
					m_userEmail = userOrganization.Email;
					m_userUsername = userOrganization.UserName;
					m_userOrganizationId = userOrganization.OrganizationId;
				}
			}
		}
	}

	public string GetsDisastersForDropDown()
	{
		string preselectedDisasterJQuery = string.Empty;
		string returnedEventName = string.Empty;
		return GetsDisastersForDropDown(string.Empty, out preselectedDisasterJQuery, out returnedEventName);
	}
	public string GetsDisastersForDropDown(string eventId)
	{
		string preselectedDisasterJQuery = string.Empty;
		string returnedEventName = string.Empty;
		return GetsDisastersForDropDown(eventId, out preselectedDisasterJQuery, out returnedEventName);
	}
	public string GetsDisastersForDropDown(string eventId, out string preselectedDisasterJQuery, out string returnedEventName)
	{
		string disasterDropDown = string.Empty;
		returnedEventName = string.Empty;
		preselectedDisasterJQuery = string.Empty;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						orderby d.BeginDate descending
						select new { d };

		int idNumber = 0;
		foreach (var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			disasterDropDown = disasterDropDown + "<option value=\"" + disaster.d.URLFriendlyName + "\">" + disasterDate + " - " + disaster.d.Name + "</option>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
		if (!String.IsNullOrEmpty(eventId))
		{
			//Hide the Dropdown and show the selected disaster
			var disaster = (from d in dc.Events
							where d.EventId == new Guid(eventId)
							orderby d.BeginDate descending
							select new { d }).SingleOrDefault();

			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			preselectedDisasterJQuery = "$(\"#btn-dropdown.disasterEvent\").html('" + disaster.d.Name + " - " + disasterDate + "');";

			var eventDetails = (from ev in dc.Events
								where ev.EventId == new Guid(eventId)
								select ev).SingleOrDefault();

			if (eventDetails != null)
			{
				returnedEventName = eventDetails.Name;
			}
		}

		return disasterDropDown;
	}
	public Profile GetUserProfileByUserId(Guid userId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var profile = (from p in dc.Profiles
					   where p.UserId == userId
					   select p).SingleOrDefault();

		return profile;
	}

	public string GetMonthsYearsElapsed(DateTime time)
	{
		DateTime rightNow = DateTime.Now;
		int monthsApart = 12 * (time.Year - rightNow.Year) + time.Month - rightNow.Month;

		int input = Math.Abs(monthsApart);
		int years = input / 12;
		int months = input % 12;

		string monthsMessage = months.ToString() + " Months";
		string returnMessage = monthsMessage;

		if (years > 0)
		{
			returnMessage = years.ToString() + " Years and " + monthsMessage;
		}

		return returnMessage;
	}

	public string GetElapsedTime(DateTime time)
	{
		string elapsedTime = "0";
		DateTime rightNow = DateTime.Now;

		TimeSpan interval = rightNow - time;

		if (interval.TotalSeconds < 60)
		{
			//Less than 60 minutes then show the interval in minutes.
			elapsedTime = interval.Seconds.ToString() + " " + (interval.Seconds == 1 || interval.Seconds == 0 ? "second ago" : "seconds ago");
		}
		else if (interval.TotalMinutes < 60)
		{
			//Less than 60 minutes then show the interval in minutes.
			elapsedTime = interval.Minutes.ToString() + " " + (interval.Minutes > 1 ? "minutes ago" : "minute ago");
		}
		else if (interval.TotalHours < 24)
		{
			//Less than 24 hours show hours passed.
			elapsedTime = interval.Hours.ToString() + " " + (interval.Hours > 1 ? "hours ago" : "hour ago");
		}
		else if (interval.TotalDays < 7)
		{
			//More than 24 hours show hours passed.
			elapsedTime = time.Date.DayOfWeek + " at " + time.ToShortTimeString();
		}
		else
		{
			elapsedTime = time.ToLongDateString();
		}

		return elapsedTime;
	}
	public string GetLatestProgressTick(Guid survivorId, Guid sliderId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var sliderProgress = (from ust in dc.UserSliderTicks
							  join st in dc.SliderTicks on ust.SliderTickId equals st.SliderTickId
							  where ust.SurvivorId == survivorId
							  && ust.SliderId == sliderId
							  && st.IsActive == true
							  orderby ust.CreatedOn descending
							  select new { st.Tick, st.Lable }).Take(1).SingleOrDefault();

		string progressLabel = string.Empty;
		if (sliderProgress != null)
		{ progressLabel = sliderProgress.Lable; }

		return progressLabel;
	}

	protected int GetCollaboratingOrganizationCount()
	{

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var allOrganizations = (from o in dc.Organizations
								select new { o.OrganizationId }).Count();

		return allOrganizations;
	}

	protected void UpdateEventPost(string post, Guid userId, Guid eventId)
	{
		if (!string.IsNullOrEmpty(post))
		{
			EventPost eventPost = new EventPost();
			eventPost.CreatedOn = DateTime.Now;
			eventPost.IsVisible = true;
			eventPost.Post = post;
			eventPost.EventPostId = Guid.NewGuid();
			eventPost.UserId = userId;
			eventPost.EventId = eventId;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			dc.EventPosts.InsertOnSubmit(eventPost);
			dc.SubmitChanges();
		}
	}
	protected void UpdatePost(string post, Guid userId, Guid rebuildId)
	{
		if (!string.IsNullOrEmpty(post))
		{
			RebuildPost rebuildPost = new RebuildPost();
			rebuildPost.CreatedOn = DateTime.Now;
			rebuildPost.IsVisible = true;
			rebuildPost.Post = post;
			rebuildPost.RebuildPostId = Guid.NewGuid();
			rebuildPost.UserId = userId;
			rebuildPost.RebuildId = rebuildId;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			dc.RebuildPosts.InsertOnSubmit(rebuildPost);
			dc.SubmitChanges();
		}
	}

	protected void InsertLocationNote(string noteText, Guid addressId, Guid createdByUserid)
	{
		if (!string.IsNullOrEmpty(noteText))
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			Guid noteId = Guid.NewGuid();
			Note note = new Note();
			note.NoteId = noteId;
			note.CreatedOn = DateTime.Now;
			note.IsDeleted = false;
			note.Note1 = noteText;
			note.HTML = false;
			note.CreatedBy = createdByUserid;
			note.IsActive = true;

			dc.Notes.InsertOnSubmit(note);
			dc.SubmitChanges();

			LocationNote locationNote = new LocationNote();
			locationNote.AddressId = addressId;
			locationNote.LocationNoteId = Guid.NewGuid();
			locationNote.NoteId = noteId;
			dc.LocationNotes.InsertOnSubmit(locationNote);
			dc.SubmitChanges();
		}
	}
	protected void InsertProfileNote(string noteText, Guid profileUserId, Guid createdByUserid)
	{
		if (!string.IsNullOrEmpty(noteText))
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			Guid noteId = Guid.NewGuid();
			Note note = new Note();
			note.NoteId = noteId;
			note.CreatedOn = DateTime.Now;
			note.IsDeleted = false;
			note.Note1 = noteText;
			note.HTML = false;
			note.CreatedBy = createdByUserid;
			note.IsActive = true;

			dc.Notes.InsertOnSubmit(note);
			dc.SubmitChanges();

			ProfileNote profileNote = new ProfileNote();
			profileNote.NoteId = noteId;
			profileNote.UserId = profileUserId;
			profileNote.ProfileNoteId = Guid.NewGuid();

			dc.ProfileNotes.InsertOnSubmit(profileNote);
			dc.SubmitChanges();
		}
	}

	public decimal GetPercent(bool isDisaster, Guid eventId, Guid survivorId, Guid sliderId, out string tickLabel)
	{
		tickLabel = string.Empty;
		decimal tickPercent = 0;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//Get the total number of SliderTicks for this tickId
		var topTick = (from st in dc.SliderTicks
					   where st.SliderId == sliderId
					   orderby st.Tick descending
					   select new { st.Tick }).Take(1).SingleOrDefault();

		if (survivorId != Guid.Empty)
		{
			//Get the number of ticks for this user.
			var myDistinctTickCount = (from st in dc.SliderTicks
									   join ust in dc.UserSliderTicks on st.SliderTickId equals ust.SliderTickId
									   where st.SliderId == sliderId
									   && ust.SurvivorId == survivorId
									   orderby ust.CreatedOn descending
									   select new { st.Tick, st.Lable }).Take(1).SingleOrDefault();

			//decimal tickPercent = ((myDistinctTickCount / tickTotalCount) * 100);
			if (myDistinctTickCount != null)
			{
				//(myDistinctTickCount / tickTotalCount) * 100;
				tickPercent = (int)Math.Round((double)(100 * myDistinctTickCount.Tick) / topTick.Tick);
				tickLabel = myDistinctTickCount.Lable;
			}
		}
		else
		{
			if (isDisaster)
			{
				//Filter on all ticks in the disaster.
				var myDistinctTickCount = from st in dc.SliderTicks
										  join ust in dc.UserSliderTicks on st.SliderTickId equals ust.SliderTickId
										  join r in dc.Rebuilds on ust.RebuildId equals r.RebuildId
										  where st.SliderId == sliderId && r.EventId == eventId
										  orderby ust.CreatedOn descending
										  select new { tick = st.Tick, st.SliderTickId };

				//decimal tickPercent = ((myDistinctTickCount / tickTotalCount) * 100);
				if (myDistinctTickCount != null && myDistinctTickCount.Count() > 0)
				{
					double avg = myDistinctTickCount.Average(x => x.tick);

					tickPercent = Convert.ToDecimal(avg);
					tickLabel = string.Empty;
				}
			}
			else
			{
				//Filter on userId instead of survivor.... we want the ALL stats for them.
				//Get the number of ticks for this user.
				var myDistinctTickCount = (from st in dc.SliderTicks
										   join ust in dc.UserSliderTicks on st.SliderTickId equals ust.SliderTickId
										   where st.SliderId == sliderId
										   && ust.UserId == m_userId
										   orderby ust.CreatedOn descending
										   select new { st.Tick, st.Lable }).Take(1).SingleOrDefault();

				//decimal tickPercent = ((myDistinctTickCount / tickTotalCount) * 100);
				if (myDistinctTickCount != null)
				{
					//(myDistinctTickCount / tickTotalCount) * 100;
					tickPercent = (int)Math.Round((double)(100 * myDistinctTickCount.Tick) / topTick.Tick);
					tickLabel = myDistinctTickCount.Lable;
				}
			}
		}

		return tickPercent;
	}

	public string CalculateVolunteersNeeded(Guid userId, int points, bool returnAllNeededVolunteers, Guid eventId)
	{
		return CrowdRelief.Tools.CalculateVolunteersNeeded(userId, points, returnAllNeededVolunteers, eventId);
	}

	public Guid userOrganizationId
	{
		get
		{ return m_userOrganizationId; }
		set
		{ m_userOrganizationId = value; }
	}
	public Guid userId
	{
		get
		{ return m_userId; }
		set
		{ m_userId = value; }
	}
	public Guid organizationId
	{
		get
		{ return m_organizationId; }
		set
		{ m_organizationId = value; }
	}
	public string organizationName
	{
		get
		{ return m_organizationName; }
		set
		{ m_organizationName = value; }
	}
	public string organizationLogo
	{
		get
		{ return m_organizationLogo; }
		set
		{ m_organizationLogo = value; }
	}
	public string userFirstname
	{
		get
		{ return m_userFirstname; }
		set
		{ m_userFirstname = value; }
	}
	public string userLastname
	{
		get
		{ return m_userLastname; }
		set
		{ m_userLastname = value; }
	}
	public string userFullname
	{
		get
		{ return m_userFullname; }
		set
		{ m_userFullname = value; }
	}
	public string userPhonenumber
	{
		get
		{ return m_userPhonenumber; }
		set
		{ m_userPhonenumber = value; }
	}
	public string userEmail
	{
		get
		{ return m_userEmail; }
		set
		{ m_userEmail = value; }
	}
	public string userUsername
	{
		get
		{ return m_userUsername; }
		set
		{ m_userUsername = value; }
	}
}