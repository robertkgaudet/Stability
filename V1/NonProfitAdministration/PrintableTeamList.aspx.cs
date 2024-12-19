using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_PrintableTeamList : BaseWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string organizationId	= Request.QueryString["organizationId"];
		string resourceId		= Request.QueryString["resourceId"];
		string skillId			= Request.QueryString["skillId"];

		CreateVolunteerList(organizationId, resourceId, skillId);
	}

	public void CreateVolunteerList(string organizationId, string resourceId, string skillId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var peopleList = from s in dc.Profiles
					   join m in dc.aspnet_Memberships on s.UserId equals m.UserId
					   join u in dc.aspnet_Users on s.UserId equals u.UserId
					   join uo in dc.UserOrganizations on s.UserId equals uo.UserId

					   orderby m.CreateDate ascending
					   select new { u.UserName, uo.OrganizationId, s.ProfileNumber, s.UserId, s.Firstname, s.Lastname, s.PhoneNumber, m.LoweredEmail };

		if (!String.IsNullOrEmpty(organizationId))
		{
			peopleList = peopleList.Where(c => (c.OrganizationId == new Guid(organizationId)));
		}

		if (!String.IsNullOrEmpty(skillId))
		{
			var skillName = (from s in dc.Skills
							 where s.SkillId == new Guid(skillId)
							 select new { s.Name }).SingleOrDefault();

			peopleList = from pl in peopleList
						 join spl in dc.UserSkills on pl.UserId equals spl.UserId
						 where spl.SkillId == new Guid(skillId)
						 select pl;

			divFilterMessage.Visible = true;
			litFilterMessage.Text = "<i class=\"fa fa-2x fa-hand-pointer-o\"></i><hr>Showing team members with the '" + skillName.Name + "' skillset.";
		}
		if (!String.IsNullOrEmpty(resourceId))
		{
			var resouceName = (from r in dc.Resources
							   where r.ResourceId == new Guid(resourceId)
							   select new { r.Name }).SingleOrDefault();

			peopleList = from pl in peopleList
						 join rpl in dc.UserResources on pl.UserId equals rpl.UserId
						 where rpl.ResourceId == new Guid(resourceId)
						 select pl;
			divFilterMessage.Visible = true;
			litFilterMessage.Text = "<i class=\"fa fa-2x fa-hand-pointer-o\"></i><hr>Showing team members with the '" + resouceName.Name + "' skillset.";

		}

		dlVolunteers.DataSource = peopleList.Distinct();
		dlVolunteers.DataBind();
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
		return "<a href=\"tel:" + number + "\">" + formattedPhoneNumber + "</a>";
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
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "userId");
			int profileNumber = (int)DataBinder.Eval(dataItem.DataItem, "ProfileNumber");
			string firstname = (string)DataBinder.Eval(dataItem.DataItem, "firstname");
			string lastname = (string)DataBinder.Eval(dataItem.DataItem, "lastname");
			string userName = (string)DataBinder.Eval(dataItem.DataItem, "UserName");
			string phoneNumber = (string)DataBinder.Eval(dataItem.DataItem, "PhoneNumber");
			string loweredEmail = (string)DataBinder.Eval(dataItem.DataItem, "LoweredEmail");

			litEmail.Text = FormatEmail(loweredEmail);
			litPhone.Text = FormatPhoneNumber(phoneNumber);

			hypID.Text = "Id: " + profileNumber + "  (" + userName + ") - " + firstname + " " + lastname;
			hypID.NavigateUrl = "/V1/Profile/Profile.aspx?userId=" + userId;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		}
	}

}