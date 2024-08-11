using Braintree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_TeamAvailabilityCalendar : BaseWebForm
{
	public string organizationId = string.Empty;
	public string jsonEvents = string.Empty;
	public string availableDates = string.Empty;
	public string teamCounts = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationId = Request.QueryString["organizationId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage }).SingleOrDefault();

		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		Master.PageTitle = organization.Name + " Team Calendar on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = "/V1/Images/AvailableTime.png";
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " Team Calendar on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;

		ucTeamNavigation.PageName = "teamCalendarPage";
		ucTeamNavigation.TeamName = organization.Name;

		string logo = string.Empty;
		string squareLogo = string.Empty;
		if (!String.IsNullOrEmpty(organization.Logo))
		{
			logo = "/Impactoid/Images/Logos/" + organization.Logo;
		}
		else
		{
			//Use placeholder image.imgLogo.Visible = true;
			logo = "/V1/Images/Logo-Placeholder.png";
		}
		if (!String.IsNullOrEmpty(organization.LogoSquare))
		{
			squareLogo = "/Impactoid/Images/Logos/" + organization.LogoSquare;
		}

		ucTeamNavigation.TeamName = organization.Name;
		ucTeamHeader.Logo = logo;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.PageName = "Team Calendar";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;
		ucTeamHeader.TeamSquareLogo = squareLogo;

		if (!User.Identity.IsAuthenticated)
		{
			//User must be signed in to see the list.
			divUpdateMessage.Visible = true;
			divCalendarRow.Visible = false;
			litMessage.Text = "<i class=\"fa fa-2x fa-exclamation-circle\"></i><hr><a href=\"\\signin\">Sign in</a> to see the team calendar.";
		}
		hypMyCalendar.NavigateUrl = "/V1/Profile/AvailableDates.aspx";

		LoadTeamCountGraph(organizationId);
	}

	public void LoadTeamCountGraph(string organizationId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		DateTime currentDate = DateTime.Now;
		var startOfCurrentWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
		var eightWeeksLater = startOfCurrentWeek.AddDays(7 * 8);

		var query = from ur in dc.UserAvailableDates
					join uo in dc.UserOrganizations on ur.UserId equals uo.UserId
					where ur.DateAvailable >= startOfCurrentWeek && ur.DateAvailable <= eightWeeksLater
					&& uo.OrganizationId == new Guid(organizationId)
					group ur by new
					{
						WeekStart = ur.DateAvailable.AddDays(-(int)ur.DateAvailable.DayOfWeek)
					} into g
					select new
					{
						WeekStart = g.Key.WeekStart,
						UserCount = g.Select(ur => ur.UserId).Distinct().Count()
					};

		var result = query.ToList();

		List<string> weekStarts = new List<string>();
		List<int> userCounts = new List<int>();

		foreach (var item in result)
		{
			weekStarts.Add(String.Format("\"{0}\"",
				item.WeekStart.ToString("MMMM dd")));
			userCounts.Add(item.UserCount);
		}

		string weekStartsString = String.Join(", ", weekStarts);
		string userCountsString = String.Join(", ", userCounts);

		availableDates = weekStartsString;
		teamCounts = userCountsString;
	}

	public class UserResponse
	{
		public int UserId { get; set; }
		public DateTime ResponseDate { get; set; }
	}

	[WebMethod]
	public static string LoadCalendar(string organizationId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var events = (from uad in dc.UserAvailableDates
					 join uo in dc.UserOrganizations on uad.UserId equals uo.UserId
					 join p in dc.Profiles on uo.UserId equals p.UserId
					 where uo.OrganizationId == new Guid(organizationId)
					 select new { uad, p }).ToList();
		//{
		// title = p.Firstname + " " + p.Lastname,
		// start = uad.DateAvailable,
		// allDay = "true",
		// url = "/V1/Profile/AvailableDates.aspx?userId=" + p.UserId
		//};

		var eventAvail = events.Select(e => new { title = e.p.Firstname + " " + e.p.Lastname, start = e.uad.DateAvailable.ToString("yyyy-MM-ddTHH:mm:ss"), allDay = "true", url = "/V1/Profile/AvailableDates.aspx?userId=" + e.p.UserId }).ToList();


		return JsonConvert.SerializeObject(eventAvail);
	}
}