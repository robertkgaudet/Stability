using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_BriefTodaysVolunteer : BaseWebForm
{
	public string _logo;
	public string _teamName;
	public string _teamSquareLogo;
	public string _teamDescription;
	public string _pageName;
	public string _organizationId;
	public string _nonProfitDropDown;
	public string _coverImage;
	public string urlFriendlyName = string.Empty;
	public string organizationId = string.Empty;
	public string volunteerLink = string.Empty;
	public string donateLink = string.Empty;
	public string impactoidLink = string.Empty;
	public string activityPageLink = string.Empty;
	public string nonProfitDropDown = string.Empty;
	public string createChapterLink = string.Empty;
	public string editLink = string.Empty;
	public string DefaultCampaignId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{

		#region HEADER PROPERTIES
		////////////////////////
		//BEGIN HEADER PROPERTIES
		////////////////////////

		urlFriendlyName = Request.QueryString["urlFriendlyName"];
		organizationId = Request.QueryString["organizationId"];

		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "businesscoverimage.png";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (!String.IsNullOrEmpty(urlFriendlyName))
		{
			//Get and set the org id.
			var organizationIdCheck = (from o in dc.Organizations
									   where o.URLFriendlyName == urlFriendlyName
									   select new { o.OrganizationId }).SingleOrDefault();

			if (organizationIdCheck.OrganizationId != Guid.Empty)
			{
				organizationId = organizationIdCheck.OrganizationId.ToString();
			}
		}

		ucTeamHeader.OrganizationId = organizationId;

		if (String.IsNullOrEmpty(organizationId))
		{
			if (!User.Identity.IsAuthenticated)
			{
				//Have the user signin
				Response.Redirect("/SignIn");
			}
			else
			{
				//Get this users team, no team? Send them to pick a team.
				var userPrimaryOrganization = (from uo in dc.UserOrganizations
											   where uo.UserId == userId && uo.IsPrimary == true
											   select new { uo.OrganizationId }).Take(1).SingleOrDefault();

				if (userPrimaryOrganization == null)
				{
					var userOrganization = (from uo in dc.UserOrganizations
											where uo.UserId == userId
											select new { uo.OrganizationId }).Take(1).SingleOrDefault();

					if (userOrganization == null)
					{
						Response.Redirect("/V1/NonProfit/TeamList.aspx?team=false");
					}
					else
					{
						organizationId = userOrganization.OrganizationId.ToString();
					}
				}
				else
				{
					organizationId = userPrimaryOrganization.OrganizationId.ToString();
				}
			}
		}

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId) && o.IsActive == true
							select new
							{
								o.PurposeMission,
								o.YearFounded,
								o.City,
								o.State,
								o._501c3Status,
								o.Address,
								o.Zip,
								o.PointOfContactName,
								o.PointOfContactPhoneNumber,
								o.PointOfContactEmail,
								o.FacebookGroupURL,
								o.FacebookURL,
								o.TwitterURL,
								o.InstagramURL,
								o.YouTubeURL,
								o.EIN,
								o.PrimaryPhone,
								o.SecondaryPhone,
								o.PublicEmail,
								o.PublicPhoneNumber,
								o.Website,
								o.BlogURL,
								o.DonationURL,
								o.IsActive,
								o.Name,
								o.LogoSquare,
								o.Description,
								o.Logo,
								o.CoverImage,
								o.URLFriendlyName,
								o.ParentOrganizationId
							}).SingleOrDefault();

		string squareLogo = "/V1/Images/Logo-Placeholder.png";
		if (organization != null)
		{
			if (!string.IsNullOrEmpty(organization.CoverImage))
			{
				//Let's the user change the cover image.
				_coverImage = causePhotoFolder + organization.CoverImage;
			}

			ucTeamHeader.CoverImage = _coverImage;
			ucTeamHeader.TeamDescription = organization.Description;
			ucTeamHeader._teamTitle = organization.Name;

			if (!string.IsNullOrEmpty(organization.LogoSquare))
			{
				string virtualPath_square = "/Impactoid/Images/Logos/" + organization.LogoSquare;
				string physicalPath_square = Server.MapPath(virtualPath_square);

				if (System.IO.File.Exists(physicalPath_square))
				{
					squareLogo = virtualPath_square;
				}
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

		bool isOwner = false;
		if (User.Identity.IsAuthenticated == true)
		{
			var userOrganizationOwners = (from uo in dc.UserOrganizations
										  join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
										  where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString()) && (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
										  && uo.OrganizationId == new Guid(organizationId)
										  select o).Take(1).SingleOrDefault();

			if (userOrganizationOwners != null)
			{

				if ((userOrganizationOwners.OwnerId == userId))
				{
					isOwner = true;
				}
			}
		}

		if (!IsPostBack)
		{
			string today = DateTime.Today.ToString("yyyy-MM-dd");
			hfStartDate.Value = today;
			hfEndDate.Value = today;

			// Set textbox to today's date and trigger initial load
			LoadNewSignups(DateTime.Today, DateTime.Today, Request.QueryString["organizationId"]);
		}

		////////////////////////
		//END HEADER PROPERTIES
		////////////////////////
		#endregion HEADER PROPERTIES
	}

	protected void btnLoad_Click(object sender, EventArgs e)
	{

		DateTime startDate, endDate;
		if (!DateTime.TryParse(hfStartDate.Value.Trim(), out startDate) || !DateTime.TryParse(hfEndDate.Value.Trim(), out endDate))
		{
			lblCount.Text = "Please select a valid date range.";
			//gvNewUsers.DataSource = null;
			//gvNewUsers.DataBind();
			gvOrganizations.DataSource = null;
			gvOrganizations.DataBind();
			return;
		}

		LoadNewSignups(startDate, endDate, Request.QueryString["organizationId"]);
	}

	private void LoadNewSignups(DateTime startDate, DateTime endDate, String organizationId)
	{
		using (var dc = new CrowdReliefDBDataContext())
		{
			var query = from s in dc.Profiles
						join m in dc.aspnet_Memberships on s.UserId equals m.UserId
						where m.CreateDate >= startDate && m.CreateDate <= endDate
						join uo in dc.UserOrganizations on s.UserId equals uo.UserId
						join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
						join r in dc.OrganizationUserRankings on s.UserId equals r.UserId into rankingGroup
						from r in rankingGroup.DefaultIfEmpty() // left join
						where m.IsApproved == true
						let rank = r != null ? r.RankPosition : 0
						select new
						{
							s.Firstname,
							s.Lastname,
							m.CreateDate,
							OrganizationName = o.Name,
							o.OrganizationId,
							s.UserId,
							RankPosition = r != null ? r.RankPosition : 0
						};

			if (!String.IsNullOrEmpty(organizationId))
			{
				query = query.Where(x => x.OrganizationId == new Guid(organizationId));
			}

			var results = query
							.OrderBy(x => x.RankPosition)
							.ThenByDescending(x => x.CreateDate)
							.ToList();

			var groupedResults = results
							.GroupBy(r => r.CreateDate.Date)
							.OrderByDescending(g => g.Key)
							.ToList();

			rptGroupedByDate.DataSource = groupedResults;
			rptGroupedByDate.DataBind();

			litNewCount.Text = "Today's Registration Count " + results.Count.ToString();
			int orgCount = results.Select(r => r.OrganizationId).Distinct().Count();
			lblCount.Text = "New Signups from " + startDate.ToString("yyyy-MM-dd") + " to " + endDate.ToString("yyyy-MM-dd") + ": " + results.Count;
			lblOrgCount.Text = "Represented Organizations: " + orgCount;

			//gvNewUsers.DataSource = results;
			//gvNewUsers.DataBind();


			// Group by Organization
			var orgSummary = (from r in results
							  group r by new { r.OrganizationId, r.OrganizationName } into g
							  orderby g.Count() descending
							  select new
							  {
								  OrganizationId = g.Key.OrganizationId,
								  OrganizationName = g.Key.OrganizationName,
								  SignupCount = g.Count(),
								  TotalMembers = dc.UserOrganizations.Count(uo => uo.OrganizationId == g.Key.OrganizationId)
							  }).ToList();


			// Update UI
			lblOrgCount.Text = "Represented Organizations: " + orgCount;


			gvOrganizations.DataSource = orgSummary.ToList();
			gvOrganizations.DataBind();
		}
	}
}