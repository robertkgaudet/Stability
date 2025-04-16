using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_TeamName : BaseWebForm
{
	string organizationId = string.Empty;
	public string nonProfitDropDown = string.Empty;
	public string preselectedNonProfitJQuery = string.Empty;
	string parentOrganizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		parentOrganizationId = Request.QueryString["parentOrganizationId"]; //IF IT'S CLICKED INTO
		if (!IsPostBack)
		{
			LoadParentNonProfits(parentOrganizationId);
		}
	}

	[WebMethod]
	public static bool IsFriendlyNameUnique(string urlFriendlyName)
	{
		using (var dc = new CrowdReliefDBDataContext())
		{
			// Check if the URLFriendlyCampaignName already exists
			return !dc.Organizations.Any(o => o.URLFriendlyName == urlFriendlyName);
		}
	}
	public void LoadParentNonProfits(string parentOrganizationId)
	{

		//Only use this if the person selected Volunteer.
		//If they have chosen an event filter down the volunteer orgs working on the event otherwise show all events.
		//If a nonprofitid is sent on the QS filter down to just that nonprofit.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		//Load all available non-profits
		//If they chose an event, filter to orgs that are added to the event.
		var nonProfits = from o in dc.Organizations
						 where o.IsActive == true
						 orderby o.Name
						 select new { o };

		nonProfitDropDown = nonProfitDropDown + "<li><a href=\"#\">Choose A Parent Organization (Optional)</a></li>" + Environment.NewLine;
		nonProfitDropDown = nonProfitDropDown + "<li><a href=\"#\">------ None ------</a></li>" + Environment.NewLine;
		foreach (var nonProfit in nonProfits)
		{
			nonProfitDropDown = nonProfitDropDown + "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;

		}

		if (parentOrganizationId != null)
		{
			//Get the parent.
			var nonProfitParent = (from o in dc.Organizations
								   where o.OrganizationId == new Guid(parentOrganizationId)
								   orderby o.Name
								   select new { o.Name }).Take(1).SingleOrDefault();

			if (nonProfitParent != null)
			{
				//If this is the parent non-profit, pre-select the item in the dropdown list and pre-set the hidden field value.
				preselectedNonProfitJQuery = "$(\"#btn-NonProfitDropdown.nonProfit\").html('" + nonProfitParent.Name + "');";
			}

		}
	}
	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		parentOrganizationId = hidParentOrganizationId.Value;
		string organizationName = txtOrganization.Value;
		string urlFriendlyName = txtURLFriendlyName.Value;

		Organization organization = new Organization();

		organization.Name = organizationName;
		organization.URLFriendlyName = urlFriendlyName;

		if(!String.IsNullOrEmpty(parentOrganizationId))
			organization.ParentOrganizationId = new Guid(parentOrganizationId);

		organization.CreatedBy = userId;
		organization.CreatedOn = DateTime.Now;
		organization.IsActive = true;
		organization.OwnerId = userId;
		organization.OrganizationId = Guid.NewGuid();

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		dc.Organizations.InsertOnSubmit(organization);
		dc.SubmitChanges();

		//Add this user as their primary org.
		UserOrganization userOrganization = new UserOrganization();
		userOrganization.UserOrganizationId = Guid.NewGuid();
		userOrganization.OrganizationId = organization.OrganizationId;
		userOrganization.UserId = userId;
		userOrganization.IsPrimary = true;
		dc.UserOrganizations.InsertOnSubmit(userOrganization);
		dc.SubmitChanges();

		//var profile = ProfileBase.Create(User.Identity.Name);
		//string email = profile.GetPropertyValue("Email") as string;
		//string firstName = profile.GetPropertyValue("Firstname") as string;

		var profile = (from p in dc.Profiles
					   join u in dc.aspnet_Memberships on p.UserId equals u.UserId
					  where p.UserId == userId
					  select new { p.Firstname, u.Email }).SingleOrDefault();

		//Send an email.
		ListDictionary ldEmailBodyReplacements = new ListDictionary();
		ldEmailBodyReplacements.Add("<% OrganizationName %>", organizationName);
		ldEmailBodyReplacements.Add("<% RecipientsName %>", profile.Firstname);
		ldEmailBodyReplacements.Add("<% OrganizationId %>", organization.OrganizationId.ToString());

		string error = string.Empty;
		Tools.SendEmail(
		string.Empty,
		"Your Disaster Relief Team " + organizationName,
		ldEmailBodyReplacements,
		profile.Email,
		profile.Firstname,
		string.Empty,
		string.Empty,
		"~\\EmailTemplates\\TeamName.html",
		out error);

		Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=" + organization.OrganizationId);
	}
	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/Default.aspx");
	}
}