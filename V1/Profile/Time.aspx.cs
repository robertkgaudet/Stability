using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_Time : System.Web.UI.Page
{
	public int totalHours = 0;
	protected void Page_Load(object sender, EventArgs e)
	{
		LoadTimeSheet();
	}

	protected void LoadTimeSheet()
	{
		Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var timesheet = from t in dc.Timesheets
						join tt in dc.TaskTypes on t.TaskTypeId equals tt.TaskTypeId into timeJoin
						from time in timeJoin.DefaultIfEmpty()
						orderby t.TimeIn descending
						select new { t.TimeIn, t.TimeOut, t.Description, taskname = time.Name, t.UserId };

		if (!String.IsNullOrEmpty(Request["all"]))
		{
			dlTimesheet.DataSource = timesheet;
			dlTimesheet.DataBind();
		}
		else
		{
			dlTimesheet.DataSource = timesheet.Where(x => x.UserId == userId);
			dlTimesheet.DataBind();
		}
	}

	protected void dlTimesheet_ItemDataBound(object sender, DataListItemEventArgs e)
	{
		DataListItem dataItem = (DataListItem)e.Item;
		Label lblTaskName = (Label)e.Item.FindControl("lbltaskName"); 
		Label lblDescription = (Label)e.Item.FindControl("lblDescription"); 
		Label lblTimeIn = (Label)e.Item.FindControl("lblTimeIn");
		Label lblTimeOut = (Label)e.Item.FindControl("lblTimeOut");
		Label lblTime = (Label)e.Item.FindControl("lblTime");
		Label lblPoints = (Label)e.Item.FindControl("lblPoints");
		Label lblDate = (Label)e.Item.FindControl("lblDate");

		String description = (String)DataBinder.Eval(dataItem.DataItem, "Description");
		String taskname = (String)DataBinder.Eval(dataItem.DataItem, "taskname");
		DateTime timeIn = (DateTime)DataBinder.Eval(dataItem.DataItem, "TimeIn");
		string timeInString = timeIn.ToShortTimeString();
		string timeOutString = "User did not sign out.";
		int timeSpanHoursInt = 0;
		int timeSpanMinutesInt = 0;
		string currentSpan = string.Empty;

		if (DataBinder.Eval(dataItem.DataItem, "TimeOut") != null)
		{
			DateTime timeOut = (DateTime)DataBinder.Eval(dataItem.DataItem, "TimeOut");
			timeOutString = timeOut.ToShortTimeString();
			TimeSpan? span = (timeOut - timeIn);
			timeSpanHoursInt = span.Value.Hours;
			timeSpanMinutesInt = span.Value.Minutes;
			currentSpan = timeSpanHoursInt + " hours " + timeSpanMinutesInt + " minutes";

			if(timeSpanMinutesInt > 30)
			{
				timeSpanHoursInt = timeSpanHoursInt + 1;
			}
		}

		lblPoints.Text = "Total Hours: " + timeSpanHoursInt.ToString(); //1 point for every hour completed.
		lblTaskName.Text = taskname;
		lblDate.Text = timeIn.ToShortDateString();
		lblTimeIn.Text = "In - " + timeInString;
		lblTimeOut.Text = "Out - " + timeOutString;
		lblDescription.Text = description;

		totalHours += timeSpanHoursInt;

	}
}