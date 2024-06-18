using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_People : BaseOrganizationWebForm
{
	public string organizationId = string.Empty;
	public string signedInUserFullName = string.Empty;
	public bool isUserOnTeam = false;
	public Guid organizationOwnerId = Guid.Empty;
	public string teamName = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamNavigation.PageName = "peoplePage";
		organizationId = Request.QueryString["organizationId"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

		ucTeamNavigation.TeamName = organization.Name;

		Master.PageTitle = organization.Name + " Team Members on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = organization.CoverImage;
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " Team Members on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;


		string logo = string.Empty;
		if (!String.IsNullOrEmpty(organization.Logo))
		{
			logo = "/Impactoid/Images/Logos/" + organization.Logo;
		}
		else
		{
			//Use placeholder image.imgLogo.Visible = true;
			logo = "/V1/Images/Logo-Placeholder.png";
		}

		ucTeamHeader.Logo = logo;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.PageName = "People";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;

		if (User.Identity.IsAuthenticated)
		{
			//Is logged in user on this team?

			var userCheck = (from uo in dc.UserOrganizations
							 where uo.UserId == userId
							 && uo.OrganizationId == new Guid(organizationId)
							 select uo).Take(1).SingleOrDefault();

			Profile profile = GetUserProfileByUserId(userId);
			signedInUserFullName = profile.Firstname + " " + profile.Lastname;

			if (userCheck != null)
			{
				//User is on this team.
				isUserOnTeam = true;
				hpanelJoin.Visible = false;
				hpanelMembers.Visible = true;
				hypInviteTeamMembers.Visible = true;
				hypInviteTeamMembers.NavigateUrl = "/V1/NonProfitAdministration/InviteTeam.aspx?organizationId=" + organizationId;
			}

			bool userIsOwner = false;
			if (organization != null)
			{
				teamName = organization.Name;
				organizationOwnerId = organization.OwnerId != null ? (Guid)organization.OwnerId : Guid.Empty;
				userIsOwner = organization.OwnerId == userId ? true : false;
				string contactInfo = string.Empty;
				if (User.IsInRole("Administrator") || organization.OwnerId == userId)
				{
					//User is the group owner.
					contactInfo = "<br/>EM:" + organization.PointOfContactEmail + "<br/>PH:" + organization.PointOfContactPhoneNumber;
				}
			}

			if (isUserOnTeam && !User.IsInRole("Administrator") && !userIsOwner)
			{
				var peopleList = from uo in dc.UserOrganizations
								 join p in dc.Profiles on uo.UserId equals p.UserId
								 join net in dc.aspnet_Memberships on p.UserId equals net.UserId
								 join u in dc.aspnet_Users on p.UserId equals u.UserId
								 where uo.OrganizationId == new Guid(organizationId)
								 && net.IsApproved == true && p.PassedVetting == true
								 orderby net.LastLoginDate descending
								 select new { p.Firstname, net.CreateDate, p.Description, net.LoweredEmail, p.Lastname, p.UserId, p.DateVettingCompleted, p.DateVettingStarted, p.VettingNotes, p.VettingActive, p.VettingComplete, p.PassedVetting, p.Title, p.ZelloName, LastLoginDate = u.LastActivityDate };

				hpanelMembers.Visible = true;
				rptVolunteers.DataSource = peopleList;
				rptVolunteers.DataBind();
			}
			else if (User.IsInRole("Administrator") || userIsOwner)
			{
				//ADMIN SEES ALL USERS
				var peopleList = from uo in dc.UserOrganizations
								 join p in dc.Profiles on uo.UserId equals p.UserId
								 join net in dc.aspnet_Memberships on p.UserId equals net.UserId
								 join u in dc.aspnet_Users on p.UserId equals u.UserId
								 where uo.OrganizationId == new Guid(organizationId)
								 orderby net.LastLoginDate descending
								 select new { p.Firstname, net.CreateDate, p.Description, net.LoweredEmail, p.Lastname, p.UserId, p.DateVettingCompleted, p.DateVettingStarted, p.VettingNotes, p.VettingActive, p.VettingComplete, p.PassedVetting, p.Title, p.ZelloName, LastLoginDate = u.LastActivityDate, net.IsLockedOut };

				rptVolunteers.DataSource = peopleList;
				rptVolunteers.DataBind();

				hpanelMembers.Visible = true;

				//Show message recommending user join this team.
			}
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
			String description = (String)DataBinder.Eval(dataItem.DataItem, "Description");
			bool? vettingComplete = (bool?)DataBinder.Eval(dataItem.DataItem, "VettingComplete");
			bool? passedVetting = (bool?)DataBinder.Eval(dataItem.DataItem, "PassedVetting");
			bool? vettingActive = (bool?)DataBinder.Eval(dataItem.DataItem, "VettingActive");
			//bool? isLockedOut = (bool?)DataBinder.Eval(dataItem.DataItem, "IsLockedOut");
			String vettingNotes = (String)DataBinder.Eval(dataItem.DataItem, "VettingNotes");
			DateTime? dateVettingCompleted = (DateTime?)DataBinder.Eval(dataItem.DataItem, "DateVettingCompleted");
			DateTime? dateVettingStarted = (DateTime?)DataBinder.Eval(dataItem.DataItem, "DateVettingStarted");
			DateTime? lastActivityDate = (DateTime?)DataBinder.Eval(dataItem.DataItem, "LastLoginDate");

			MembershipUser profileUser = Membership.GetUser(userId);

			bool isLockedOut = !profileUser.IsApproved;
			//isLockedOut
			vettingComplete = vettingComplete == null ? false : vettingComplete;
			passedVetting = passedVetting == null ? false : passedVetting;
			vettingActive = vettingActive == null ? false : vettingActive;
			DateTime dateVettingCompletedString = dateVettingCompleted == null ? DateTime.MinValue : (DateTime)dateVettingCompleted;
			DateTime dateVettingStartedString = dateVettingStarted == null ? DateTime.MinValue : (DateTime)dateVettingStarted;
			DateTime lastActivityDateString = lastActivityDate == null ? DateTime.MinValue : (DateTime)lastActivityDate;

			String dateVettingComplete = dateVettingCompletedString == DateTime.MinValue ? "Not Complete" : dateVettingCompletedString.ToLongDateString();
			String dateVettingStarts = dateVettingStartedString == DateTime.MinValue ? "Not Started" : dateVettingStartedString.ToLongDateString();
			String lastActivitysDate = lastActivityDateString == DateTime.MinValue ? "Not Started" : lastActivityDateString.ToShortDateString() + " " + lastActivityDateString.ToLongDateString() + " at " + lastActivityDateString.ToLongTimeString();

			string vettingCompleted = (bool)vettingComplete ? "VETTING COMPLETE: " + ((bool)passedVetting ? "<span style='color:yellowgreen'>PASSED</span>" : "<span style='color:orange'>FAILED</span>") : (bool)vettingActive ? "VETTING: PENDING" : "VETTING: NO ACTION TAKEN";
			vettingCompleted += ((bool)isLockedOut ? "<br><span style='color:orange'>LOCKED OUT</span>" : "<br><span style='color:yellowgreen'>HAS ACCESS</span>") + ("<br>Notes:" + vettingNotes + "<br>Date Started: " + dateVettingStarts + "<br>Date Completed: " + dateVettingComplete + "<br>Last Activity Date: " + lastActivitysDate);

			HtmlGenericControl divFooter = (HtmlGenericControl)e.Item.FindControl("divFooter");
			HyperLink hypName = (HyperLink)e.Item.FindControl("hypName");
			Literal litMemberInfo = (Literal)e.Item.FindControl("litMemberInfo");
			Literal litDescription = (Literal)e.Item.FindControl("litDescription");
			Literal litSkills = (Literal)e.Item.FindControl("litSkills");
			Literal litActiveDate = (Literal)e.Item.FindControl("litActiveDate");
			Literal litVettingInfo = (Literal)e.Item.FindControl("litVettingInfo");
			Button btnContact = (Button)e.Item.FindControl("btnContact");
			hypName.Text = firstname + " " + lastname;
			hypName.NavigateUrl = "/V1/Profile/Profile.aspx?userId=" + userId;

			btnContact.Attributes.Add("data-name", firstname + " " + lastname);
			btnContact.Attributes.Add("data-email", loweredEmail);
			btnContact.Attributes.Add("onclick", "return btnClick(this);");

			title = !String.IsNullOrEmpty(title) ? " TITLE: " + title + "</br>" : "";
			zelloName = !String.IsNullOrEmpty(zelloName) ? " ZELLO: " + zelloName + "</br>" : "";
			description = !String.IsNullOrEmpty(description) ? " ABOUT ME: " + description + "</br>" : "";

			litVettingInfo.Text = vettingCompleted;

			litMemberInfo.Text = title + " " + zelloName;
			string skills = GetSkills(userId);
			litSkills.Text = !String.IsNullOrEmpty(skills) ? "SKILLS: " + skills + "</br>" : "";
			litDescription.Text = description;

			if (User.IsInRole("Administrator") || organizationOwnerId == userId)
			{
				divFooter.Visible = true;
				//Show the message users button.
				btnContact.Visible = true;
			}
		}
	}
	protected string GetSkills(Guid userId)
	{
		string skillList = string.Empty;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var skills = from us in dc.UserSkills
					 join s in dc.Skills on us.SkillId equals s.SkillId
					 where us.UserId == userId
					 select s;

		skillList = string.Join(" ", skills.Select(p => "<button type=\"button\" class=\"btn btn-default\">" + p.Name.ToString() + "</button>"));

		return skillList;
	}

}