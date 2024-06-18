using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class UserControls_TimeControls : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		litTime.Text = "TRACK YOUR TIME";
		btnTimeOut.Visible = false;
		btnTimeIn.Visible = false;
		divTimeDescription.Visible = false;
		divTaskTypedropdown.Visible = false;

		btnTimeIn.Text = "START VOLUNTEERING <span class=\"glyphicon glyphicon-time\"></span>";
		btnTimeOut.Text = "FINISH VOLUNTEERING <span class=\"glyphicon glyphicon-time\"></span>";

		if (HttpContext.Current.User.Identity.IsAuthenticated)
		{
			litTimeMessage.Text = "Earn points from Crowd Relief for tracking your time.";

			Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var timesheet = (from t in dc.Timesheets
							where t.UserId == userId
							orderby t.TimeIn descending
							select t).Take(1).SingleOrDefault();

			if(timesheet != null)
			{

				//IF Time in is not today, then close out yesterday
				//AND Make user log in today anyway.

				if(timesheet.TimeIn.Date > DateTime.Today && timesheet.TimeIn == null)
				{
					//user forgot to logout, log them out at midnight on the day they forgot.
					TimeSpan midnight = new TimeSpan(23, 59, 0);
					string missingDateTime = timesheet.TimeIn.Date.ToLongDateString();
					timesheet.TimeOut = timesheet.TimeIn.Date + midnight;
					dc.SubmitChanges();
					btnTimeIn.Visible = true;
					divTimeDescription.Visible = false;
					divTaskTypedropdown.Visible = true;

					//Show a message that they were logged out after midnight.
					litTimeMessage.Text = "You may have forgotten to log out the last time you logged in (" + missingDateTime + "). We just logged you out at midnight on that day. Please check your time sheet then contact us to have your time fixed if needed.";
				}

				//is user logged in.
				if (timesheet.TimeOut == null)
				{
					//User is logged in.
					btnTimeOut.Visible = true;
					divTimeDescription.Visible = true;
					divTaskTypedropdown.Visible = false;
				}
				else
				{
					//Use is not logged in. 
					btnTimeIn.Visible = true;
					divTimeDescription.Visible = false;
					divTaskTypedropdown.Visible = true;
				}
			}
			else
			{
				//Use is not logged in. 
				btnTimeIn.Visible = true;
				divTimeDescription.Visible = false;
				divTaskTypedropdown.Visible = true;
			}
		}
		else
		{
			//Dont show the time time tracking.

		}
	}

	protected void btnTimeIn_Click(object sender, EventArgs e)
	{
		//Add new time entry.

		//IF Time in is not today, then close out yesterday
		//AND Make user log in today anyway.
		Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

		string taskTypeId = hidTaskType.Value;

		//If this is for a survey, get the org id off the survey if there is one.

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		Guid timesheetIdGuid = Guid.NewGuid();

		Timesheet timesheet = new Timesheet();
		timesheet.TimesheetId = timesheetIdGuid;
		timesheet.TimeIn = DateTime.Now;
		timesheet.UserId = userId;
		if(!String.IsNullOrEmpty(taskTypeId))
		{
			timesheet.TaskTypeId = new Guid(taskTypeId);
		}
		timesheet.IsDeleted = false;
		timesheet.Error = false;

		dc.Timesheets.InsertOnSubmit(timesheet);
		dc.SubmitChanges();

		string surveyId = Request["surveyId"];
		if(!String.IsNullOrEmpty(surveyId))
		{
			Guid SurveyIdGuid = new Guid(surveyId);

			SurveyTimesheet surveyTimesheet = new SurveyTimesheet();
			surveyTimesheet.SurveyId = SurveyIdGuid;
			surveyTimesheet.TimesheetId = timesheetIdGuid;
			surveyTimesheet.SurveyTimesheetId = Guid.NewGuid();
			dc.SurveyTimesheets.InsertOnSubmit(surveyTimesheet);
			dc.SubmitChanges();
		}
		btnTimeIn.Visible = false;
		divTaskTypedropdown.Visible = false;
		btnTimeOut.Visible = true;
		divTimeDescription.Visible = true;
	}

	protected void btnTimeOut_Click(object sender, EventArgs e)
	{
		//Update the last time entry if there is a place for it.
		//If 
		Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var timesheet = (from t in dc.Timesheets
						 where t.UserId == userId
						 orderby t.TimeIn descending
						 select t).Take(1).SingleOrDefault();

		timesheet.TimeOut = DateTime.Now;
		timesheet.Description = txtTimeDescription.Text;
		dc.SubmitChanges();

		btnTimeIn.Visible = true;
		divTaskTypedropdown.Visible = true;
		btnTimeOut.Visible = false;
		divTimeDescription.Visible = false;
	}
}