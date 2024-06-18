using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class CaseManagement_Time : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		LoadTimeSheet();
	}

	protected void LoadTimeSheet()
	{
		Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var timesheet = from t in dc.Timesheets
						join tt in dc.TaskTypes on t.TaskTypeId equals tt.TaskTypeId
						orderby t.TimeIn descending
						select new { t.TimeIn, t.TimeOut, t.Description, taskname = tt.Name, t.UserId };

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

		if(DataBinder.Eval(dataItem.DataItem, "TimeOut") != null)
		{
			String description = (String)DataBinder.Eval(dataItem.DataItem, "Description");
			String taskname = (String)DataBinder.Eval(dataItem.DataItem, "taskname");
			DateTime timeIn = (DateTime)DataBinder.Eval(dataItem.DataItem, "TimeIn");
			DateTime timeOut = (DateTime)DataBinder.Eval(dataItem.DataItem, "TimeOut");

			int hours = 0;

			if (timeIn != null && timeOut != null)
			{
				TimeSpan? span = (timeOut - timeIn);

				string currentSpan = span.Value.Hours.ToString() + " hours " + span.Value.Minutes.ToString() + " minutes";
				lblPoints.Text = span.Value.Hours.ToString();
				lblTime.Text = "Total - " + currentSpan;
				lblTaskName.Text = taskname;
				lblDate.Text = timeIn.ToShortDateString();
				lblTimeIn.Text = "In - " + timeIn.ToShortTimeString();
				lblTimeOut.Text = "Out - " + timeOut.ToShortTimeString();
				lblDescription.Text = description;
			}
		}

	}
}