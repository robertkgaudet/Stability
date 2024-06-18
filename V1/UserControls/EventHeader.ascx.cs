using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_EventHeader : System.Web.UI.UserControl
{
	public string eventName = HttpContext.Current.Request.QueryString["eventName"];
	public string headerColor = string.Empty;
	public string color = string.Empty;
	public string icon = string.Empty;
	public string _pageTitle = string.Empty; 
	public string _eventName = string.Empty; 
	public string _pageDescription = string.Empty;
	public string _teamCount = string.Empty;
	public string _causeCount = string.Empty;
	public string _ticketCount = string.Empty;
	public string eventBackgroundImage = "MichaelImage.jpg";
	public Guid? eventId = Guid.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		//Get the eventId from the eventname
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disaster = (from ev in dc.Events
						join et in dc.EventTypes on ev.EventTypeId equals et.EventTypeId
						where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
						select new { ev, et }).SingleOrDefault();

		litDate.Text = String.Format("{0:Y}", disaster.ev.BeginDate);
		litWeatherType.Text = disaster.et.Name;
		litEventName.Text = _eventName;
		litEventDescription.Text = _pageDescription;
		LoadStates(disaster.ev.EventId);
		DateTime beginDate = (DateTime) disaster.ev.BeginDate;
		string timePassed = GetMonthsYearsElapsed(beginDate);
		litAge.Text = timePassed + " Ago";
		//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.ev.EventId);

		litTicketCount.Text = _ticketCount;
		litTeamCount.Text = _teamCount;
		litCauseCount.Text = _causeCount;
		litStatesCounties.Text = CrowdRelief.Tools.GetImpactedStateCountyString(disaster.ev.EventId);
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

	public void LoadStates(Guid eventId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var states = from s in dc.USStates
									  join es in dc.EventStates on s.StatesId equals es.StatesId
									  where es.EventId == eventId
									  orderby s.Name
									  select s;

		string commaSeparatedNames = string.Join(", ", states.Select(c => c.Name));

		litStates.Text = commaSeparatedNames;
	}

	public string PageDescription
	{
		get { return _pageDescription; }
		set { _pageDescription = value; }
	}
	public string PageTitle
	{
		get { return _pageTitle; }
		set { _pageTitle = value; }
	}
	public string EventName
	{
		get { return _eventName; }
		set { _eventName = value; }
	}
	public string TeamCount
	{
		get { return _teamCount; }
		set { _teamCount = value; }
	}
	public string CauseCount
	{
		get { return _causeCount; }
		set { _causeCount = value; }
	}
	public string TicketCount
	{
		get { return _ticketCount; }
		set { _ticketCount = value; }
	}

	protected void btnSetDefaultDisaster_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

        var profile = (from p in dc.Profiles
					  where p.UserId == userId
                       select p).SingleOrDefault();

		profile.DefaultEventId = eventId;
		dc.SubmitChanges();
		
		var disaster = (from ev in dc.Events
						where ev.EventId == eventId
                        select ev).SingleOrDefault();

        //Makes sure the user is assocated with the response if not already.
        var userCheck = from p in dc.UserEvents
                        where p.UserId == userId
                        && p.EventId == disaster.EventId
                        select p;

        if (userCheck.Count() == 0)
        {
            UserEvent userEvent = new UserEvent();
            userEvent.EventId = disaster.EventId;
            userEvent.UserId = userId;
            userEvent.UserEventId = Guid.NewGuid();
            dc.UserEvents.InsertOnSubmit(userEvent);
            dc.SubmitChanges();
        }

        Response.Redirect("/Disaster/" + disaster.URLFriendlyName);
	}

	public string CalculateVolunteersNeeded(Guid userId, int points, bool returnAllNeededVolunteers, Guid eventId)
	{
		return CrowdRelief.Tools.CalculateVolunteersNeeded(userId, points, returnAllNeededVolunteers, eventId);
	}
}