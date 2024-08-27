using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_PositionsNeeded : BaseWebForm
{
	public string todaysDate = DateTime.Now.ToShortDateString();
	public string organizationEventPositionsId = string.Empty;
	public bool signedInUser = true;
	public string readOnlyCalendar = string.Empty;

	string organizationEventId = string.Empty;
	string pageName = "Positions";
	protected void Page_Load(object sender, EventArgs e)
	{
		//Server.HtmlEncode();
		organizationEventId = Request.QueryString["organizationEventId"];
		ucPostionNavigation.PageName = pageName;
		ucPostionNavigation.OrganizationEventId = organizationEventId;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var deployment = (from or in dc.OrganizationEvents
						 where or.OrganizationEventId == new Guid(organizationEventId)
						 select or).SingleOrDefault();

		if (deployment != null )
		{
			litEventName.Text = deployment.CampaignName;
		}





		organizationEventPositionsId = Request.QueryString["userId"];

		if (!IsPostBack)
		{
			//Get the organizationEventPositionsId info here.
			//List<DateTime> datesAvailable = (from uad in dc.UserAvailableDates
			//								 where uad.UserId == new Guid(calendarUserId)
			//								 orderby uad.DateAvailable
			//								 select uad.DateAvailable).ToList();

			//if (datesAvailable.Count() > 0)
			//{
			//	string datesAvailableJSON = JsonConvert.SerializeObject(datesAvailable);
			//	hiddenAvailableDates.Value = datesAvailableJSON;
			//}
		}
	}


	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/");
	}


	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		//TYPE IN AND POPULATE THE POSITION BOX PER TEAM

		//INSERT ORGANIZATIONEVENTPOSITION

		//INSERT ORGANIZATIONEVENTDATE





		//----------------CALENDAR UPDATES-----------------------
		//Only allow the signed in user to do updates.
		string jsonString = hiddenAvailableDates.Value;

		// Deserialize the JSON string to a C# object
		List<DateTime> jsonDates = JsonConvert.DeserializeObject<List<DateTime>>(jsonString);
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var databaseDates = from uad in dc.UserAvailableDates
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

			if (deleteDate)
			{
				var dateCheck = (from uad in dc.UserAvailableDates
								 where uad.UserId == userId &&
								 uad.DateAvailable == databaseDate.DateAvailable
								 select uad).SingleOrDefault();
				if (dateCheck != null)
				{
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

			if (dateCheck.Count() == 0)
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