using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_VolunteerList : BaseOrganizationWebForm
{
	public string volunteerLink = string.Empty; 
	public string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
        if (String.IsNullOrEmpty(Request.QueryString["organizationId"]))
		{
			Response.Write("No organization Id provided.");
			Response.End();
        }

		organizationId = Request.QueryString["organizationId"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var peopleList = from uo in dc.UserOrganizations
						 join p in dc.Profiles on uo.UserId equals p.UserId
						 join net in dc.aspnet_Memberships on p.UserId equals net.UserId
						 join u in dc.aspnet_Users on p.UserId equals u.UserId
						 where uo.OrganizationId == new Guid(organizationId) && uo.Status== (int)RequestStatus.Approved && uo.Status == (int)RequestStatus.Pending
                         && net.IsApproved == true
						 orderby net.LastLoginDate descending
						 select new { p.Firstname, net.CreateDate, net.LoweredEmail, p.Lastname, p.UserId, p.PassedVetting, p.Title, p.ZelloName, LastLoginDate = u.LastActivityDate };

		rptVolunteers.DataSource = peopleList;
		rptVolunteers.DataBind();

		var organization = (from o in dc.Organizations
						   where o.OrganizationId == new Guid(organizationId)
						   select o).SingleOrDefault();

		if(organization != null)
        {
            if (User.IsInRole("Administrator"))
            {	
                if (!String.IsNullOrEmpty(Request.QueryString["ownerId"]))
                {
                    //Set the new ownerId
                    organization.OwnerId = new Guid(Request.QueryString["ownerId"]);
                    dc.SubmitChanges();
                }
            }

            Master.PageTitle			= organization.Name + " - Volunteer List";
			Master.PageDescription		= organization.Description;
			Master.FbDescription		= organization.Description;
			Master.FbImage				= organization.CoverImage;
			Master.FbImageType			= "image/jpg";
			Master.FbSite_name			= organization.Name + " - Volunteer List";
			Master.FbURL				= Request.Url.AbsoluteUri;

			litOrganizationName.Text	= organization.Name;
		}
	}

	protected void rptVolunteers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String firstname = (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String lastname = (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
			String zelloName = (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
			String title = (String)DataBinder.Eval(dataItem.DataItem, "Title");
			String loweredEmail = (String)DataBinder.Eval(dataItem.DataItem, "LoweredEmail"); 
			DateTime lastOnlineActiveDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "LastLoginDate");
			DateTime createDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "CreateDate"); 

			HyperLink hypID = (HyperLink)e.Item.FindControl("hypID");
			Literal litActiveStatus = (Literal)e.Item.FindControl("litActiveStatus");
			Literal litCreateDate = (Literal)e.Item.FindControl("litCreateDate");
			Literal litLastLoginDate = (Literal)e.Item.FindControl("litLastLoginDate");
			Literal litEmail = (Literal)e.Item.FindControl("litEmail");

			DateTime lastLoginDate = DateTime.Now;
			double timeSpanHoursInt = 0;
			double timeSpanMinutesInt = 0;
			double timeSpanDaysInt = 0;
			string signedInInfo = string.Empty;

			string activedMessage = string.Empty;
			string deActivedMessage = string.Empty;

			string activeColorClass = string.Empty;
			string totalTimeToday = string.Empty;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			//Is the user signed in?
			var timesheet = (from t in dc.Timesheets
							 join tt in dc.TaskTypes on t.TaskTypeId equals tt.TaskTypeId into timeJoin
							 from time in timeJoin.DefaultIfEmpty()
							 where t.UserId == userId && t.TimeIn != null
							 orderby t.TimeIn descending
							 select new { t.TimeIn, t.TimeOut, t.Description, taskname = time.Name }).Take(1).SingleOrDefault();

			if (timesheet != null)
			{
				//If user has no timeout and more than 8 hours has passed then...
				TimeSpan? spanToday = (DateTime.Now - timesheet.TimeIn);
				timeSpanDaysInt = spanToday.Value.TotalDays;
				timeSpanHoursInt = spanToday.Value.TotalHours;
				timeSpanMinutesInt = spanToday.Value.Minutes;

				activedMessage = "<span class='" + activeColorClass + "'>Active at: " + timesheet.TimeIn.ToLongTimeString() + " " + timesheet.TimeIn.ToLongDateString() + "</span>";

				if (timesheet.TimeOut == null)
				{
					//User is either signed in or they forgot to sign out.
					if (timeSpanHoursInt > 8)
					{
						signedInInfo = "<span class='text-danger'>Forgot to sign out.</span><br>";
						//PROBLEM - User signed in more than 8 hours ago and did not sign out, show how many days/hours.
						activeColorClass = "text-danger";
						deActivedMessage = "<br>De-Activated at: <i>User has been Active for more than 8 hours.</i>";
						totalTimeToday = "<span class='text-danger'>User did not De-Activate. They have been Active for more than 8 hours.</span>";
					}
					else if (timeSpanHoursInt <= 8)
					{
						signedInInfo = "<span class='text-success'>Currently Tracking Time</span><br>";
						//User recently signed in.//GOOD ZONE USER IS SIGNED IN AND HAS BEEN SO FOR LESS THAN 8 HOURS
						//User signed in for today and has been active for less than 8 hours.
						deActivedMessage = "<br>De-Activated at: <i><small>User Activated less than 8 hours ago.</small></i>";
						activeColorClass = "text-success";
						totalTimeToday = "<span class='text-success'>" + (spanToday.Value.Hours > 0 ? spanToday.Value.Hours + " hours " + timeSpanMinutesInt + " minutes " : timeSpanMinutesInt + " minutes ") + "</span>";
					}
				}
				else
				{
					//User has a sign out that matches their sign in.
					signedInInfo = "<span>Not Tracking Time</span><br>";
					spanToday = (timesheet.TimeOut - timesheet.TimeIn);
					activeColorClass = string.Empty;
					deActivedMessage = "<br>De-Activated at: " + timesheet.TimeOut.Value.ToLongTimeString() + " " + timesheet.TimeOut.Value.ToLongDateString();
					totalTimeToday = "<span class='text-success'>" + (spanToday.Value.Hours > 0 ? spanToday.Value.Hours + " hours " + spanToday.Value.Minutes + " minutes " : spanToday.Value.Minutes + " minutes ") + "</span>";
				}

				signedInInfo = signedInInfo + activedMessage + deActivedMessage;
			}

			title = String.IsNullOrEmpty(title) ? "none" : title;
			string active = (GetElapsedTime(lastOnlineActiveDate).Substring(0, 1) == "-" ? "<span class='text-success'>Online Now</span> <i class=\"fa fa-wifi text-success\"></i>" : "Last Online: " + GetElapsedTime(lastOnlineActiveDate) + " <i class=\"fa fa-wifi text-muted\"></i>");
			litActiveStatus.Text = "<dl class=\"dl-vertical\"><dt><b><a href=\"/V1/Profile/Profile.aspx?userId=" + userId.ToString() + "\"  style=\"text-decoration:underline;\">" + firstname + " " + lastname + "</a></b> <small>" + active + "</small></dt><dd><small>" + title + "</small></dd><dd><small>" + signedInInfo + "</small></dd><dd><small>Last Active Total: " + totalTimeToday + "</small></dd></dl>";
			litCreateDate.Text = createDate.ToLongDateString();
			litEmail.Text = loweredEmail;
			litLastLoginDate.Text = lastOnlineActiveDate.ToLongDateString();

			//HyperLink hypMakeOwner = (HyperLink)e.Item.FindControl("hypMakeOwner");
			//hypMakeOwner.NavigateUrl = "NonProfit.aspx?organizationId=" + Request.QueryString["organizationId"] +"&ownerId=" + userId.ToString();
			//hypMakeOwner.Text = "Set '" + firstname + "' As Owner";
			//if (User.IsInRole("Administrator"))
			//{
			//    hypMakeOwner.Visible = true;
			//}
		}
	}
}