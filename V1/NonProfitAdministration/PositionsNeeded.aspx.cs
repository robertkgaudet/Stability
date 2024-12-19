using Newtonsoft.Json;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
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
	public string _userId = string.Empty;
	public string organizationId = string.Empty;

	public string urlFriendlyname = string.Empty;
	string organizationEventId = string.Empty;
	string pageName = "Positions";

	protected void Page_Load(object sender, EventArgs e)
	{
		_userId = userId.ToString();
		//Server.HtmlEncode();
		organizationEventId = Request.QueryString["organizationEventId"];
		ucPostionNavigation.PageName = pageName;
		ucPostionNavigation.OrganizationEventId = organizationEventId;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var deployment = (from or in dc.OrganizationEvents
						  join o in dc.Organizations on or.OrganizationId equals o.OrganizationId
						  join ev in dc.Events on or.EventId equals ev.EventId
						 where or.OrganizationEventId == new Guid(organizationEventId)
						 select new { or, o, ev }).SingleOrDefault();

		if (deployment != null )
		{
			if(deployment.o.OwnerId == userId || User.IsInRole("Administrator"))
			{
				ucPostionNavigation.IsTeamOwner = true;
			}
			ucPostionNavigation.URLFriendlyName = deployment.or.URLFriendlyCampaignName;
			organizationId = deployment.or.OrganizationId.ToString();
			litEventName.Text = deployment.or.CampaignName;
			btnPreviewPositions.NavigateUrl = "/signup/" + deployment.or.URLFriendlyCampaignName;

			ucPostionNavigation.CampaignName = deployment.or.CampaignName;
			ucPostionNavigation.OrganizationName = deployment.o.Name;
			ucPostionNavigation.PortalName = deployment.ev.Name;
			ucPostionNavigation.OrganizationId = deployment.o.OrganizationId.ToString();
			ucPostionNavigation.Description = deployment.or.MissionPurpose;
		}

		organizationEventPositionsId = Request.QueryString["userId"];

		if (!IsPostBack)
		{
			//LoadExistingDates();
		}
	}

	//private void LoadExistingDates()
	//{
	//	CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
	//	List<DateTime> datesDeployed = (from oepd in dc.OrganizationEventPositionDates
	//									where oepd.OrganizationEventId == new Guid(organizationEventId)
	//									orderby oepd.DeploymentDate
	//									select oepd.DeploymentDate).ToList();

	//	if (datesDeployed.Count() > 0)
	//	{
	//		string datesDeployedJSON = JsonConvert.SerializeObject(datesDeployed);
	//		hiddenDeployedDates.Value = datesDeployedJSON;
	//	}
	//}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/");
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string datesJsonString = hiddenDeployedDates.Value;

		bool hasDate = !String.IsNullOrEmpty(datesJsonString) ? true : false;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organizationEvent = (from oe in dc.OrganizationEvents
								 where oe.OrganizationEventId == new Guid(organizationEventId)
								 select oe).SingleOrDefault();

		organizationEvent.VolunteerInstructions = Server.HtmlEncode(txtMessage.Text);
		dc.SubmitChanges();

		//Get the info, go through the dates and insert with a date if there is one.
		if (hasDate)
		{
			// Deserialize the JSON string to a C# object
			List<DateTime> jsonDates = JsonConvert.DeserializeObject<List<DateTime>>(datesJsonString);
			//LoadCalendar(jsonDates);

			//Insert dates
			foreach (var jsonDate in jsonDates)
			{
				insertPositions(true, jsonDate);
			}
		}
		else
		{
			insertPositions(false, DateTime.MinValue);
		}

		Response.Redirect("/V1/NonProfitAdministration/PositionsNeeded.aspx?organizationEventId=" + organizationEventId);
	}

	public void LoadCalendar(List<DateTime> jsonDates)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var databaseDates = from oepd in dc.OrganizationEventPositionDates
							where oepd.OrganizationEventId == new Guid(organizationEventId)
							orderby oepd.DeploymentDate
							select oepd;

		//Go through existing dates and see if any need to be deleted.
		foreach (var databaseDate in databaseDates)
		{
			bool deleteDate = true;
			DateTime dateToDelete = DateTime.Now;

			//Is this date in the json list?
			foreach (var jsonDate in jsonDates)
			{
				//Is the date from the database in the list? If so, we do not delete it.
				if (databaseDate.DeploymentDate == jsonDate)
				{
					//Date matched, do not delete
					deleteDate = false;
					break;
				}
			}

			if (deleteDate)
			{
				var dateCheck = (from oepd in dc.OrganizationEventPositionDates
								 where oepd.OrganizationEventId == new Guid(organizationEventId) &&
								 oepd.DeploymentDate == databaseDate.DeploymentDate
								 select oepd).SingleOrDefault();

				if (dateCheck != null)
				{
					dc.OrganizationEventPositionDates.DeleteOnSubmit(dateCheck);
					dc.SubmitChanges();
				}
			}
		}

		List<DateTime> datesDeployed = (from oepd in dc.OrganizationEventPositionDates
										where oepd.OrganizationEventId == new Guid(organizationEventId)
										orderby oepd.DeploymentDate
										select oepd.DeploymentDate).ToList();

		string datesDeployedJSON = JsonConvert.SerializeObject(datesDeployed);
		hiddenDeployedDates.Value = datesDeployedJSON;
	}

	public void insertPositions(bool hasDate, DateTime deploymentDate)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		//TYPE IN AND POPULATE THE POSITION BOX PER TEAM
		// Getting all submitted name[] and email[] fields
		string[] positionArr = Request.Form.GetValues("position[]");
		string[] positionCountArr = Request.Form.GetValues("positionCount[]");
		string[] arrivalTimeArr = Request.Form.GetValues("arrivalTime[]");
		string[] leaveTimeArr = Request.Form.GetValues("leaveTime[]");
		string[] isRemoteArr = Request.Form.GetValues("isRemote[]");

		//Insert the message.

		// Iterate through the arrays and process each entry without a date
		for (int i = 0; i < positionArr.Length; i++)
		{
			string position = positionArr[i];
			string positionCount = positionCountArr[i];
			bool isRemote = isRemoteArr[i] == "1";

			string arrivalTime = arrivalTimeArr[i];
			string leaveTime = leaveTimeArr[i];

			TimeSpan arrivalTimeSpan = TimeSpan.Zero;
			TimeSpan leaveTimeSpan = TimeSpan.Zero;

			try
			{
				if (!String.IsNullOrEmpty(arrivalTime))
				{
					// Parse the string to a DateTime object using ParseExact with a format that includes AM/PM
					DateTime parsedArrivalTime = DateTime.ParseExact(arrivalTime, "h:mmtt", System.Globalization.CultureInfo.InvariantCulture);
					arrivalTimeSpan = parsedArrivalTime.TimeOfDay;
				}
				if (!String.IsNullOrEmpty(leaveTime))
				{
					DateTime parsedDepartureTime = DateTime.ParseExact(leaveTime, "h:mmtt", System.Globalization.CultureInfo.InvariantCulture);
					leaveTimeSpan = parsedDepartureTime.TimeOfDay;
				}
			}
			catch (FormatException)
			{
			}

			Guid organizationEventPositionId = Guid.NewGuid();
			int positionCountInt = 1;

			//Is position in the database for this team?
			var positions = (from p in dc.Positions
							 where p.Name == position
							 && p.OrganizationId == new Guid(organizationId)
							 select p).SingleOrDefault();

			Guid positionId = Guid.NewGuid();
			if (positions == null)
			{
				//Ths position is not in the database for this organization, so add it.
				Position positionAdd = new Position();
				positionAdd.PositionId = positionId;
				positionAdd.Name = position;
				positionAdd.CreatedOn = DateTime.Now;
				positionAdd.CreatedBy = userId;
				positionAdd.IsRemote = isRemote;
				positionAdd.OrganizationId = new Guid(organizationId);
				dc.Positions.InsertOnSubmit(positionAdd);
				dc.SubmitChanges();
			}
			else
			{
				positionId = positions.PositionId;
			}

			//INSERT ORGANIZATIONEVENTPOSITION
			OrganizationEventPosition oep = new OrganizationEventPosition();
			oep.OrganizationEventPositionId = organizationEventPositionId;
			oep.OrganizationEventId = new Guid(organizationEventId);

			if (hasDate)
			{
				oep.DeploymentDate = deploymentDate;
				oep.HasDate = true;
			}
			if (!String.IsNullOrEmpty(arrivalTime))
			{
				oep.ArrivalTime = arrivalTimeSpan;
			}
			if (!String.IsNullOrEmpty(leaveTime))
			{
				oep.DepartureTime = leaveTimeSpan;
			}
			if (int.TryParse(positionCount, out positionCountInt))
			{
				oep.NumberNeeded = positionCountInt;
			}
			oep.PositionId = positionId;
			oep.IsDeleted = false;
			oep.CreatedBy = userId;
			oep.CreatedOn = DateTime.Now;
			dc.OrganizationEventPositions.InsertOnSubmit(oep);
			dc.SubmitChanges();
		}
	}
}