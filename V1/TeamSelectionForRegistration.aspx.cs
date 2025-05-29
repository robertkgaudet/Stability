using System;
using System.Linq;
using System.Web.Security;
using CrowdRelief;
using System.Web.UI.WebControls;
using System.IdentityModel.Metadata;
using System.Collections.Specialized;
using System.Web.Routing;
using Twilio.TwiML.Voice;
using System.Web.Services;

public partial class V1_TeamSelectionForRegistration : System.Web.UI.Page
{
	public string disasterDropDown = string.Empty;
	public string nonProfitDropDown = string.Empty;
	public string preselectedDisasterJQuery = string.Empty;
	public string preselectedNonProfitJQuery = string.Empty;
	public string eventId = string.Empty;
	public string organizationId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{

		//Only use this if the person selected Volunteer.
		//If they have chosen an event filter down the volunteer orgs working on the event otherwise show all events.
		//If a nonprofitid is sent on the QS filter down to just that nonprofit.

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (!IsPostBack)
		{
			Guid parentOrganizationId;

			// Replace this with your actual logic to determine parentId
			string parentOrganizationIdString = Request.QueryString["parentOrganizationId"];
			if (!Guid.TryParse(parentOrganizationIdString, out parentOrganizationId))
			{
				// Handle missing or invalid parentId
				return;
			}

			if (User.Identity.IsAuthenticated)
			{
				Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=" + parentOrganizationIdString);
			}
			else
			{
				//Get the parent team name.

				var parentTeam = (from o in dc.Organizations
								  where o.OrganizationId == parentOrganizationId
								  select new { o.Name }).SingleOrDefault();

				litParentTeam.Text = parentTeam.Name;

				var orgs = from o in dc.Organizations
						   where o.ParentOrganizationId == parentOrganizationId
						   && o.IsActive == true
						   orderby o.Name
						   select new
						   {
							   o.OrganizationId,
							   o.Name
						   };

				ddlOrganizations.DataSource = orgs.ToList();
				ddlOrganizations.DataTextField = "Name";
				ddlOrganizations.DataValueField = "OrganizationId";
				ddlOrganizations.DataBind();

				ddlOrganizations.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Your Club--", ""));

			}
		}

	}

	protected void ddlOrganizations_SelectedIndexChanged(object sender, EventArgs e)
	{
		string selectedId = ddlOrganizations.SelectedValue;
		if (!string.IsNullOrEmpty(selectedId))
		{
			Response.Redirect("/register.aspx?organizationId=" + selectedId);
		}
	}
}