using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_VolunteerByVettingStatus : System.Web.UI.Page
{
	public string vettingStatusList = string.Empty;
	public string preselectedRoleJQuery;

	protected void Page_Load(object sender, EventArgs e)
	{

		string roleId = Request.QueryString["roleId"];

		CreateVolunteerList(roleId);
		
		LoadRoles();
	}
	public class VettingProfile
	{
		public Guid UserId { get; set; }
		public int ProfileNumber { get; set; }
		public string UserName { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public Guid RoleId { get; set; }
		public string PhoneNumber { get; set; }
		public string LoweredEmail { get; set; }
	}
	public void CreateVolunteerList(string roleId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		Guid parsedRoleId;
		bool filterByRole = Guid.TryParse(roleId, out parsedRoleId);

		var profiles = from s in dc.Profiles
					   join m in dc.aspnet_Memberships on s.UserId equals m.UserId
					   join u in dc.aspnet_Users on s.UserId equals u.UserId
					   join r in dc.aspnet_UsersInRoles on s.UserId equals r.UserId
					   where !filterByRole || r.RoleId == parsedRoleId
					   select new VettingProfile
					   {
						   UserId = s.UserId,
						   ProfileNumber = s.ProfileNumber,
						   UserName = u.UserName,
						   Firstname = s.Firstname,
						   Lastname = s.Lastname,
						   RoleId = r.RoleId,
						   PhoneNumber = s.PhoneNumber,
						   LoweredEmail = m.LoweredEmail
					   };

		var distinctProfiles = profiles
			.GroupBy(p => p.UserId)
			.Select(g => g
				.OrderByDescending(p => p.ProfileNumber)
				.FirstOrDefault()) // pick one per user
			.OrderByDescending(p => p.ProfileNumber)
			.Take(1000)
			.ToList();

		dlVettingStatusList.DataSource = distinctProfiles;
		dlVettingStatusList.DataBind();

		if (!String.IsNullOrEmpty(roleId))
		{
			var roles = (from r in dc.aspnet_Roles
						where r.RoleId == new Guid(roleId)
						select new { r.RoleName }).SingleOrDefault();

			string roleName = string.Empty;
			roleName = roles.RoleName;

			if(!String.IsNullOrEmpty(roleName))
			{
				preselectedRoleJQuery = "$(\"#btn-dropdown.roleList\").html('" + roleName + "');";
			}
		}
	}
	
	protected void LoadRoles()
	{
		vettingStatusList = "<li id='" + VolunteerStatus.VettingStarted.Value + "'><a href=\"#\">Start Vetting</a></li>";
		vettingStatusList += "<li id='" + VolunteerStatus.VettingComplete_Passed.Value + "'><a href=\"#\">Passed Vetting</a></li>";
		vettingStatusList += "<li id='" + VolunteerStatus.VettingComplete_Failed.Value + "'><a href=\"#\">Failed Vetting</a></li>";
	}

	protected string GetSkills(Guid userId)
	{
		string skillList = string.Empty;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var skills = from us in dc.UserSkills
					 join s in dc.Skills on us.SkillId equals s.SkillId
					 where us.UserId == userId
					 select s;

		skillList = string.Join(", ", skills.Select(p => p.Name.ToString()));

		return skillList;
	}

	protected string GetEvents(Guid userId)
	{
		string eventList = string.Empty;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var events = from us in dc.UserEvents
					 join s in dc.Events on us.EventId equals s.EventId
					 where us.UserId == userId
					 select s;

		eventList = string.Join(", ", events.Select(p => p.Name.ToString()));

		return eventList;
	}

	string FormatPhoneNumber(string number)
	{
		// Handle null or empty strings
		if (number == null || number.Trim() == "")
		{
			return "Invalid phone number";
		}

		// Handle non-numeric values
		long numericPhoneNumber;
		if (!long.TryParse(number, out numericPhoneNumber))
		{
			return "Invalid phone number";
		}

		// Format the valid numeric phone number
		string formattedPhoneNumber = String.Format("{0:(###) ###-####}", numericPhoneNumber);

		// Return the formatted phone number as a clickable link
		return "<a href=\"tel:" +  number + "\">" + formattedPhoneNumber + "</a>";
	}

	string FormatEmail(string email)
	{
		// Handle null or empty strings
		if (string.IsNullOrEmpty(email) || !email.Contains("@"))
		{
			return "Invalid email";
		}

		// Return the formatted email as a clickable link
		return "<a href=\"mailto:" + email + "\">" + email + "</a>";
	}

	protected void dlVolunteers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			string campaignImageFolder = System.Configuration.ConfigurationManager.AppSettings["CampaignImageFolder"].ToString();

			RepeaterItem dataItem = (RepeaterItem)e.Item;
			HyperLink hypID = (HyperLink)e.Item.FindControl("hypID");
			Literal litEmail = (Literal)e.Item.FindControl("litEmail");
			Literal litPhone = (Literal)e.Item.FindControl("litPhone");

			//Total count of items and total cost.
			Guid userId			= (Guid)DataBinder.Eval(dataItem.DataItem, "userId");
			int profileNumber	= (int)DataBinder.Eval(dataItem.DataItem, "ProfileNumber");
			string firstname	= (string)DataBinder.Eval(dataItem.DataItem, "firstname");
			string lastname		= (string)DataBinder.Eval(dataItem.DataItem, "lastname");
            string userName     = (string)DataBinder.Eval(dataItem.DataItem, "UserName");
			string phoneNumber = (string)DataBinder.Eval(dataItem.DataItem, "PhoneNumber");
			string loweredEmail = (string)DataBinder.Eval(dataItem.DataItem, "LoweredEmail");

			litEmail.Text	= FormatEmail(loweredEmail);
			litPhone.Text = FormatPhoneNumber(phoneNumber);

			hypID.Text				= "Id: " + profileNumber + "  (" + userName + ") - " + firstname + " " + lastname;
			hypID.NavigateUrl		= "/V1/Profile/Profile.aspx?userId=" + userId;
			
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		}
	}
}