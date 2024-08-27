using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_TeamAvailabilityCalendar : BaseWebForm
{
	public string _logo;
	public string _teamName;
	public string _teamSquareLogo;
	public string _teamDescription;
	public string _pageName;
	public string _organizationId;
	public string _nonProfitDropDown;
	public string _coverImage;
	public string organizationId = string.Empty;
	public string jsonEvents = string.Empty;
	public string availableDates = string.Empty;
	public string teamCounts = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamFooter.PageName = "teamCalendarPage";
		ucTeamHeader.PageName = "Team Availability Calendar";

		#region HEADER PROPERTIES
		////////////////////////
		//BEGIN HEADER PROPERTIES
		////////////////////////

		organizationId = Request.QueryString["organizationId"];
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "businesscoverimage.png";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName }).SingleOrDefault();

		string squareLogo = string.Empty;
		if (organization != null)
		{
			if (organization.CoverImage != null)
			{
				//	_coverImage = causePhotoFolder + organization.CoverImage;
			}

			ucTeamHeader.CoverImage = _coverImage;
			ucTeamHeader.TeamDescription = organization.Description;
			ucTeamHeader._teamTitle = organization.Name;

			if (!String.IsNullOrEmpty(organization.LogoSquare))
			{
				squareLogo = "/Impactoid/Images/Logos/" + organization.LogoSquare;
			}
			else
			{
				squareLogo = "/V1/Images/Logo-Placeholder.png";
			}

			Master.PageTitle = organization.Name + " Programs on Stability";
			Master.PageDescription = organization.Description;
			Master.FbDescription = organization.Description;
			Master.FbImage = _coverImage;
			Master.FbSite_name = organization.Name + " Programs on Stability";
			ucTeamHeader.URLFriendlyPageName = organization.URLFriendlyName;
		}

		ucTeamFooter.TeamName = organization.Name;
		ucTeamFooter.OrganizationId = organizationId;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.TeamLogo = squareLogo;
		Master.FbImageType = "image/jpg";
		Master.FbURL = Request.Url.AbsoluteUri;

		//ucTeamHeader.Logo = logo;
		//ucTeamHeader.OrganizationId = organizationId;
		//ucTeamHeader.PageName = "Programs";
		//ucTeamHeader.TeamDescription = organization.Description;
		//ucTeamHeader.TeamName = organization.Name;
		//ucTeamHeader.TeamSquareLogo = squareLogo;

		bool isOwner = false;
		if (User.Identity.IsAuthenticated == true)
		{
			var userOrganizationOwner = (from uo in dc.UserOrganizations
										 join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
										 where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
										 && uo.OrganizationId == new Guid(organizationId)
										 select o).Take(1).SingleOrDefault();

			if (userOrganizationOwner != null)
			{
				if ((userOrganizationOwner.OwnerId != userId))
				{
					isOwner = true;
				}
			}
		}

		////////////////////////
		//END HEADER PROPERTIES
		////////////////////////
		#endregion

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

		var eventAvail = events.Select(e => new { title = e.p.Firstname + " " + e.p.Lastname, start = e.uad.DateAvailable.ToString("yyyy-MM-ddTHH:mm:ss"), allDay = "true", url = "/V1/Profile/AvailableDates.aspx?userId=" + e.p.UserId }).ToList();


		return JsonConvert.SerializeObject(eventAvail);
	}
}