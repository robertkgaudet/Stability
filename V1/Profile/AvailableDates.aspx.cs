using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class V1_Profile_AvailableDates : BaseWebForm
{
	public string todaysDate = DateTime.Now.ToShortDateString();
	public string calendarUserId = string.Empty;
	public bool signedInUser = true;
	public string readOnlyCalendar = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!User.Identity.IsAuthenticated)
		{
			Response.Redirect("/SignIn");
		}

		Master.PageTitle = "Schedule Future Dates To Help - Stability";
		Master.FbSite_name = "Schedule Future Dates To Help - Stability";
		Master.PageDescription = "Earmark the future dates you will be available to join your team to help.";
		Master.FbDescription = "Earmark the future dates you will be available to join your team to help.";
		Master.FbImage = "/V1/Images/AvailableTime.png";
		Master.FbImageType = "image/png";
		Master.FbURL = Request.Url.AbsoluteUri;







		calendarUserId = Request.QueryString["userId"];

		if(!String.IsNullOrEmpty(calendarUserId))
		{
			//Possibly not the signed in user.
			if(calendarUserId != userId.ToString())
			{
				//We need to disable/hide the edit features of the calendar.
				signedInUser = false;
				btnSubmit.Visible = false;
				btnCancel.Visible = false;

				readOnlyCalendar = ", beforeShowDay: function(date) {return false;}";
			}
		}
		else
		{
			calendarUserId = userId.ToString();
		}

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if (!IsPostBack)
		{
			List<DateTime> datesAvailable = (from uad in dc.UserAvailableDates
											where uad.UserId == new Guid(calendarUserId)
											orderby uad.DateAvailable
											select uad.DateAvailable).ToList();

			if(datesAvailable.Count() > 0)
			{ 
				string datesAvailableJSON = JsonConvert.SerializeObject(datesAvailable);
				hiddenAvailableDates.Value = datesAvailableJSON;
			}
		}

		var userOrganization = (from uo in dc.UserOrganizations
								join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
								join p in dc.Profiles on uo.UserId equals p.UserId
								where uo.UserId == new Guid(calendarUserId) && uo.IsEnabled == true
                                select new { TeamName = o.Name, o.OrganizationId, fullName = p.Firstname + " " + p.Lastname }).Take(1).SingleOrDefault(); ;

		if (userOrganization == null)
		{
			hypBreadcrumbTeamName.Text = "Find A Team";
			hypBreadcrumbTeamName.NavigateUrl = "/V1/Profile/EditNonProfits.aspx";

		}
		else
		{
			hypBreadcrumbTeamName.Text = userOrganization.TeamName;
			hypBreadcrumbTeamName.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + userOrganization.OrganizationId;

			hypBreadcrumbTeamCalendar.Text			= "Team Calendar";
			hypBreadcrumbTeamCalendar.NavigateUrl	= "/V1/NonProfit/TeamAvailabilityCalendar.aspx?organizationId=" + userOrganization.OrganizationId;

			litTeamMemberName.Text = userOrganization.fullName + "'s Calendar";
		}
	}
	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/");
	}


	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		//Only allow the signed in user to do updates.
		string jsonString = hiddenAvailableDates.Value;

		// Deserialize the JSON string to a C# object
		List<DateTime> jsonDates = JsonConvert.DeserializeObject<List<DateTime>>(jsonString);
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var databaseDates =	from uad in dc.UserAvailableDates
							where uad.UserId == userId
							orderby uad.DateAvailable
							select uad;

		//Go through existing dates and see if any need to be deleted.
		foreach (var databaseDate in databaseDates)
		{
			bool deleteDate = true;
			DateTime dateToDelete = DateTime.Now;

			//Is this date in the json list?
			foreach (var jsonDate in jsonDates)
			{
				//Is the date from the database in the list? If so, we do not delete it.
				if (databaseDate.DateAvailable == jsonDate)
				{
					//Date matched, do not delete
					deleteDate = false;
					break;
				}
			}

			if(deleteDate)
			{
				var dateCheck = (from uad in dc.UserAvailableDates
								where uad.UserId == userId &&
								uad.DateAvailable == databaseDate.DateAvailable
								 select uad).SingleOrDefault();
				if(dateCheck != null) { 
					dc.UserAvailableDates.DeleteOnSubmit(dateCheck);
					dc.SubmitChanges();
				}
			}
		}

		foreach (var jsonDate in jsonDates)
		{
			var dateCheck = from uad in dc.UserAvailableDates
						where uad.UserId == userId && 
						uad.DateAvailable == jsonDate
							select uad;

			if(dateCheck.Count() == 0)
			{ 
				//Make sure this user has not already selected these dates.
				UserAvailableDate userAvailabelDate = new UserAvailableDate();
				userAvailabelDate.DateAvailable = jsonDate;
				userAvailabelDate.DateCreated = DateTime.Now;
				userAvailabelDate.UserId = userId;
				userAvailabelDate.UserAvailableDateId = Guid.NewGuid();

				dc.UserAvailableDates.InsertOnSubmit(userAvailabelDate);
				dc.SubmitChanges();
			}
		}

		List<DateTime> datesAvailable = (from uad in dc.UserAvailableDates
										 where uad.UserId == userId
										 orderby uad.DateAvailable
										 select uad.DateAvailable).ToList();

		string datesAvailableJSON = JsonConvert.SerializeObject(datesAvailable);
		hiddenAvailableDates.Value = datesAvailableJSON;

		Response.Redirect("/V1/Member/");
	}
}