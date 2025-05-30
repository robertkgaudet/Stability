using Stability;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_Positions : BaseWebForm
{
	public string organizationId = string.Empty;
	public string teamName = string.Empty;
	public string signedInUserFullName	= string.Empty;
	public string _userId = string.Empty;
	public string _urlFriendlyName = string.Empty;
	public string deploymentName = string.Empty;
	private int collapseCounter = 1;
	public string googlePlacesId = string.Empty;
	public string mapApiKey = ConfigurationManager.AppSettings["mapApiKey"].ToString();
	protected void Page_Load(object sender, EventArgs e)
	{
		//If the user is not signed in, help them create a username/password and get back here.
		//Make sure there's a first name and a last name for the user
		//Make sure we can edit positions.
		//Make sure the users here are team admins and are the team owner for this team.
		//Delete the record, or delete the entire record from the participant screen

		deploymentName = Request.QueryString["deploymentName"];
		_urlFriendlyName = deploymentName;
		if (!IsPostBack)
		{
			BindPositionsGrid(deploymentName);
			//litLink.Text = "<i class=\"fa fa-copy\"></i> Copy A Sharable Link";
			//litLink.Attributes.Add("onclick", "copyToClipboard('https://www.stability.org/signup/" + deploymentName + "')");
		}
		this.Master.CoverImageFile = "/profilecover.png";

		if (User.Identity.IsAuthenticated)
		{
			_userId = userId.ToString();
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var signedInUserName = (from p in dc.Profiles
									where p.UserId == userId
									select new { p.Firstname, p.Lastname }).SingleOrDefault();

			signedInUserFullName = signedInUserName.Firstname + " " + signedInUserName.Lastname;
		}
	}

	private void BindPositionsGrid(string deploymentName)
	{
		// Assuming you have a LINQ to SQL data context named `YourDataContext`
		//Get the dates if there are any.
		string pageTitle = string.Empty;

		using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
		{
			var today = DateTime.Today;
			var nextWeek = today.AddDays(14);

			var organizationEventPositionDate = (from oepd in dc.OrganizationEventPositions
												 join oe in dc.OrganizationEvents on oepd.OrganizationEventId equals oe.OrganizationEventId
												 join o in dc.Organizations on oe.OrganizationId equals o.OrganizationId
												 where oe.URLFriendlyCampaignName == deploymentName
												 && oepd.IsDeleted == false
												 && oepd.DeploymentDate >= today
												 && oepd.DeploymentDate <= nextWeek
												 select new
												 {
													 oepd.DeploymentDate,
													 oe.OrganizationEventId,
													 oe.CampaignName,
													 oe.OrganizationId,
													 oe.AddressId,
													 oe.VolunteerInstructions,
													 oe.MissionPurpose,
													 o.Name
												 }).Distinct().OrderBy(o => o.DeploymentDate).ToList();

			if (organizationEventPositionDate.Count > 0 )
			{
				teamName = organizationEventPositionDate.Take(1).SingleOrDefault().Name;
				organizationId = organizationEventPositionDate.Take(1).SingleOrDefault().OrganizationId.ToString();
				ucPostionNavigation.CampaignName		= organizationEventPositionDate.Take(1).SingleOrDefault().CampaignName;
				ucPostionNavigation.OrganizationName	= organizationEventPositionDate.Take(1).SingleOrDefault().Name;
				ucPostionNavigation.PortalName			= organizationEventPositionDate.Take(1).SingleOrDefault().CampaignName;
				ucPostionNavigation.OrganizationId		= organizationEventPositionDate.Take(1).SingleOrDefault().OrganizationId.ToString();
				ucPostionNavigation.Description			= organizationEventPositionDate.Take(1).SingleOrDefault().MissionPurpose;
				ucPostionNavigation.OrganizationEventId = organizationEventPositionDate.Take(1).SingleOrDefault().OrganizationEventId.ToString();
				ucPostionNavigation.URLFriendlyName		= deploymentName;
				ucPostionNavigation.PageName			= "Positions";

				//divMap.Visible = false;
				var address = (from a in dc.Addresses
							  where a.AddressId == organizationEventPositionDate.Take(1).SingleOrDefault().AddressId
							  select a).SingleOrDefault();

				litVolunteerInstructions.Text = Server.HtmlDecode(organizationEventPositionDate.Take(1).SingleOrDefault().VolunteerInstructions);

				if (address != null )
				{
					//divMap.Visible = true;
					googlePlacesId = address.GooglePlaceId;
				}

				rptPositionDate.DataSource = organizationEventPositionDate;
				rptPositionDate.DataBind();
				//hypBack.NavigateUrl = "/V1/NonProfit/DeploymentTeams.aspx?organizationId=" + organizationEventPositionDate.Take(1).SingleOrDefault().OrganizationId;

				if (organizationEventPositionDate.Count() > 0)
				{ 
					litPageTitle.InnerText = organizationEventPositionDate.Take(1).SingleOrDefault().CampaignName + " Positions";
					this.Master.PageName = organizationEventPositionDate.Take(1).SingleOrDefault().CampaignName + " Open Positions";
				}
			}
		}
	}

	protected void rptPositionDate_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		//Get the positions.
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			Repeater rptPositions = (Repeater)e.Item.FindControl("rptPositions");
			Literal litDate =(Literal)e.Item.FindControl("litDate");
			var divCollapse = (HtmlGenericControl)e.Item.FindControl("divCollapse");
			var divHeading = (HtmlGenericControl)e.Item.FindControl("divHeading");
			var linkCollapse = (HtmlAnchor)e.Item.FindControl("linkCollapse");

			DateTime? deploymentDate = null;
			if(DataBinder.Eval(dataItem.DataItem, "DeploymentDate") != null)
			{ 
				deploymentDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "DeploymentDate");
			}

			//Get the AVAILABLE POSITIONS
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			// Fetch the positions
			var positions = from po in dc.Positions
				join oep in dc.OrganizationEventPositions on po.PositionId equals oep.PositionId
				join oe in dc.OrganizationEvents on oep.OrganizationEventId equals oe.OrganizationEventId
				where oe.URLFriendlyCampaignName == deploymentName
				&& oep.IsDeleted == false
				select new
				{
					po.Description,
					po.PositionId,
					po.IsRemote,
					po.Name,
					po.RequiresCertification,
					po.RequiresDeployment,
					po.RequiresTraining,
					oep.ArrivalTime,
					oep.DepartureTime,
					oep.HasDate,
					oep.NumberNeeded,
					oe.OrganizationEventId,
					oep.OrganizationEventPositionId,
					oep.DeploymentDate
				};

			if (deploymentDate.HasValue)
			{
				positions = positions.Where(x => x.DeploymentDate == deploymentDate.Value);
			}

			rptPositions.DataSource = positions.ToList();
			rptPositions.DataBind();

			string positionCount = "</br><span class=\"text-muted\"><small>(" + positions.Count() + " Positions)</small></span>"; ;

			string wordEquivalent = ConvertNumberToWord(collapseCounter);
			string headingValue = "heading" + wordEquivalent;
			string collapse = "collapse" + wordEquivalent;
			divCollapse.ID = collapse;
			divCollapse.ClientIDMode = ClientIDMode.Static;
			divCollapse.Attributes.Add("aria-labelledby", headingValue);
			divHeading.ID = headingValue;
			divHeading.ClientIDMode = ClientIDMode.Static;

			string datePlaceholder = "<a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#" + collapse + "\" aria-expanded=\"true\" aria-controls=\"" + collapse + "\">No Date</a>";
			litDate.Text = datePlaceholder + positionCount;

			// Apply the additional where clause only if deploymentDate is provided
			if (deploymentDate.HasValue)
			{
				datePlaceholder = deploymentDate.Value.ToString("dddd, MMMM dd, yyyy") + positionCount; ;
				litDate.Text = "<i class=\"fa fa-calendar\"></i> <a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#" + collapse + "\" aria-expanded=\"true\" aria-controls=\"" + collapse + "\">" + datePlaceholder + "</a>";
				positions = positions.Where(x => x.DeploymentDate == deploymentDate.Value);
			}

			// Increment the counter for the next item
			collapseCounter++;
		}
	}

	protected void rptPositions_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		//Get the positions for each date.
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");
			Guid positionId = (Guid)DataBinder.Eval(dataItem.DataItem, "PositionId");
			Guid organizationEventPositionId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventPositionId");
			String position = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			String description = (string)DataBinder.Eval(dataItem.DataItem, "Description");
			bool isRemote = (bool)DataBinder.Eval(dataItem.DataItem, "IsRemote");
			int numberNeeded = (int)DataBinder.Eval(dataItem.DataItem, "NumberNeeded");

			bool alreadyInPosition = false;

			//DateTime deploymentDate = DateTime.MinValue;
			//if (DataBinder.Eval(dataItem.DataItem, "DeploymentDate") != null)
			//{
			//	deploymentDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "DeploymentDate");
			//}

			TimeSpan arrivalTime = TimeSpan.Zero;
			if (DataBinder.Eval(dataItem.DataItem, "ArrivalTime") != null)
			{
				arrivalTime = (TimeSpan)DataBinder.Eval(dataItem.DataItem, "ArrivalTime");
			}

			TimeSpan departureTime = TimeSpan.Zero;
			if (DataBinder.Eval(dataItem.DataItem, "DepartureTime") != null)
			{
				departureTime = (TimeSpan)DataBinder.Eval(dataItem.DataItem, "DepartureTime");
			}

			bool hasDate = (bool)DataBinder.Eval(dataItem.DataItem, "HasDate");

			Label lblPosition = (Label)e.Item.FindControl("lblPosition");
			Label lblTime = (Label)e.Item.FindControl("lblTime");
			Literal litRemainingPositions = (Literal)e.Item.FindControl("litRemainingPositions");
			//Literal litPostionDetails = (Literal)e.Item.FindControl("litPostionDetails");
			Literal litParticipants = (Literal)e.Item.FindControl("litParticipants");
			Button btnSignUp = (Button)e.Item.FindControl("btnSignUp");
			Button btnDeleteSpot = (Button)e.Item.FindControl("btnDeleteSpot");
			HyperLink hypGetTrained = (HyperLink)e.Item.FindControl("hypGetTrained");
			HyperLink hypSignIn = (HyperLink)e.Item.FindControl("hypSignIn"); 

			hypGetTrained.NavigateUrl = "/V1/NonProfit/TeamRole.aspx?organizationId=" + organizationId +"&positionId=" + positionId;
			//hypGetTrained.Text = "Position Description and Training";

			//Check for signups and count.
			var UserOrganizationEventPositions = (from uoep in dc.UserOrganizationEventPositions
													join p in dc.Profiles on uoep.UserId equals p.UserId
													join u in dc.aspnet_Memberships on p.UserId equals u.UserId
													where uoep.OrganizationEventPositionId == organizationEventPositionId
													select new { uoep, p, u.Email }).Distinct().OrderBy(o => o.uoep.CreatedOn).ToList();

			int userSelectedPositionCount = numberNeeded - UserOrganizationEventPositions.Count();
			string participants = string.Empty;
			foreach(var UserOrganizationEventPosition in UserOrganizationEventPositions)
			{
				Guid thisUserId = UserOrganizationEventPosition.p.UserId;

				if(thisUserId == userId)
				{
					alreadyInPosition = true;
				}

				//Set this user info to this position and date.
				char firstName = UserOrganizationEventPosition.p.Firstname[0];
				char lastName = UserOrganizationEventPosition.p.Lastname[0];
				string email = UserOrganizationEventPosition.Email;
				//onclick=\"window.location.href='/V1/Member/Default.aspx?userid=" + thisUserId + "'\"
				if(User.IsInRole("administrator"))
				{
					participants += "<span onclick=\"return btnClick(this);\" data-toggle=\"modal\" data-target=\"#messageMemberModal\" data-email=\"" + email + "\" data-name=\"" + UserOrganizationEventPosition.p.Firstname + " " + UserOrganizationEventPosition.p.Lastname + "\" class=\"circle\" data-placement=\"top\" title=\"" + UserOrganizationEventPosition.p.Firstname + " " + UserOrganizationEventPosition.p.Lastname + "\">" + firstName + lastName + "</span>";
				}
				else
				{
					participants += "<span onclick=\"window.location.href='/V1/Member/Default.aspx?userid=" + thisUserId + "'\" class=\"circle\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"" + UserOrganizationEventPosition.p.Firstname + " " + UserOrganizationEventPosition.p.Lastname + "\">" + firstName + lastName + "</span>";
				}
			}

			litParticipants.Text = participants;
			litRemainingPositions.Text = userSelectedPositionCount + "/" + numberNeeded.ToString() + " remaining";





			//--------- BEGIN GET THE TIME INFO --------------
			DateTime arrivalDateTime = DateTime.MinValue;
			DateTime dpeartDateTime = DateTime.MinValue;
			if (arrivalTime != TimeSpan.Zero)
			{ 
				arrivalDateTime = DateTime.Today.Add(arrivalTime);
			}
			if (arrivalTime != TimeSpan.Zero)
			{
				dpeartDateTime = DateTime.Today.Add(departureTime);
			}

			// Format to 12-hour clock with AM/PM
			string formattedArrivalTime = arrivalDateTime != DateTime.MinValue ? arrivalDateTime.ToString("h:mmtt") : string.Empty;
			string formattedDepartureTime = dpeartDateTime != DateTime.MinValue ? dpeartDateTime.ToString("h:mmtt") : string.Empty;
			string time = string.Empty;
			if(formattedArrivalTime != string.Empty && formattedDepartureTime != string.Empty)
			{
				time = formattedArrivalTime + " to " + formattedDepartureTime;
			}
			else if
			(!String.IsNullOrEmpty(formattedArrivalTime))
			{ 
				time = "Arrive at " + formattedArrivalTime;
			}
			else
			{
				time = "Arrive anytime";
			}

			lblTime.Text = time;
			//--------- END GET THE TIME INFO --------------







			string details = description;
			//litPostionDetails.Text = (isRemote ? "Remote: YES - This can be a remote position." : "Remote: NO - Requires on site work.") + "<Br>Description: " + details;
			lblPosition.Text = position;
			if (userSelectedPositionCount == 0)
			{
				//No more open positions
				btnSignUp.Text = "No Open Spots";
				btnSignUp.Enabled = false;
				btnSignUp.CssClass = "btn w-xs btn-default";

				if (User.Identity.IsAuthenticated)
				{
					//Delete feature.
					if (User.IsInRole("administrator"))
					{
						btnDeleteSpot.CssClass = "btn btn-danger delete-btn";
						btnDeleteSpot.Text = "Delete Spot";
						btnDeleteSpot.Visible = true;
						btnDeleteSpot.Attributes.Add("data-value", organizationEventPositionId.ToString());
					}
				}
			}
			else
			{
				if (User.Identity.IsAuthenticated)
				{
					//Delete feature.
					if(User.IsInRole("administrator"))
					{
						btnDeleteSpot.CssClass = "btn btn-danger delete-btn";
						btnDeleteSpot.Text = "Delete Spot";
						btnDeleteSpot.Visible = true;
						btnDeleteSpot.Attributes.Add("data-value", organizationEventPositionId.ToString());
					}

					if(alreadyInPosition)
					{
						//User is already in this position or another position, change button.
						//User is eligiable to choose here.
						btnSignUp.CssClass = "btn btn-default";
						btnSignUp.Text = "Spot Claimed";
						btnSignUp.Enabled = false;
					}
					else
					{ 
						//User is eligiable to choose here.
						btnSignUp.CssClass = "btn btn-success insert-btn";
						btnSignUp.Text = "Claim Spot";
						btnSignUp.Attributes.Add("data-value", organizationEventPositionId.ToString());
					}
				}
				else
				{
					//Send to registration / sign up page
					btnSignUp.Visible = false;
					hypSignIn.Visible = true;
					hypSignIn.CssClass = "btn btn-info";
					hypSignIn.Text = "Claim Spot";
					hypSignIn.NavigateUrl = "/SignIn/V1/Login.aspx?ReturnUrl=/signup/" + _urlFriendlyName;
				}
			}
		}
	}
	public static string ConvertNumberToWord(int number)
	{
		var numberWords = new Dictionary<int, string>
	{
		{ 0, "zero" },
		{ 1, "one" },
		{ 2, "two" },
		{ 3, "three" },
		{ 4, "four" },
		{ 5, "five" },
		{ 6, "six" },
		{ 7, "seven" },
		{ 8, "eight" },
		{ 9, "nine" },
		{ 10, "ten" },
		{ 11, "eleven" },
		{ 12, "twelve" },
		{ 13, "thirteen" },
		{ 14, "fourteen" },
		{ 15, "fifteen" },
		{ 16, "sixteen" },
		{ 17, "seventeen" },
		{ 18, "eighteen" },
		{ 19, "nineteen" },
		{ 20, "twenty" },
		{ 21, "twentyone" },
		{ 22, "twentytwo" },
		{ 23, "twentythree" },
		{ 24, "twentyfour" },
		{ 25, "twentyfive" },
		{ 26, "twentysix" },
		{ 27, "twentyseven" },
		{ 28, "twentyeight" },
		{ 29, "twentynine" },
		{ 30, "thirty" }
	};

		if (numberWords.ContainsKey(number))
		{
			return numberWords[number];
		}
		else
		{
			return "unknown";
		}
	}

	[WebMethod]
	public static string DeleteUserOrganizationEventPosition(string organizationEventPositionId)
	{
		// Using LINQ to SQL to insert the record
		using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
		{
			//Get all of the users in the position and delete the records.
			var userOrganizationEventPositions = from oep in dc.UserOrganizationEventPositions
												 where oep.OrganizationEventPositionId == new Guid(organizationEventPositionId)
												 select oep;

			foreach (var oep in userOrganizationEventPositions)
			{
				oep.IsActive = false;
			}

			// Submit changes to apply the updates to the database
			dc.SubmitChanges();

			var organizationEventPosition = (from oep in dc.OrganizationEventPositions
											where oep.OrganizationEventPositionId == new Guid(organizationEventPositionId)
											select oep).SingleOrDefault();

			// Submit the changes to the database
			organizationEventPosition.IsDeleted = true;
			dc.SubmitChanges();
		}

		return "Spot has been deketed.";
	}

	[WebMethod]
	public static string InsertUserOrganizationEventPosition(string organizationEventPositionId, string userId)
	{
		// Generate a new GUID for UserOrganizationEventPositionId
		Guid userOrganizationEventPositionId = Guid.NewGuid();

		// Get the current user ID (this is just an example; adapt as needed)
		//string userId = "example-user-id"; // Replace with your actual method to get the user ID

		// Get the current date and time
		DateTime createdOn = DateTime.Now;

		// Default value for IsActive
		bool isActive = true;

		// Using LINQ to SQL to insert the record
		using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
		{
			UserOrganizationEventPosition newRecord = new UserOrganizationEventPosition
			{
				UserOrganizationEventPositionId = userOrganizationEventPositionId,
				OrganizationEventPositionDateId = null, // Set to null
				OrganizationEventPositionId = new Guid(organizationEventPositionId),
				UserId = new Guid(userId),
				CreatedOn = createdOn,
				IsActive = isActive
			};

			// Insert the new record
			dc.UserOrganizationEventPositions.InsertOnSubmit(newRecord);

			// Submit the changes to the database
			dc.SubmitChanges();
		}

		return "Your spot has been reserved, select Get Trained to prepare for your position.";
	}
}