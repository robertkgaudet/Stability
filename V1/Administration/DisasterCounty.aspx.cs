using Braintree;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_DisasterCounty : BaseOrganizationWebForm
{
	public string disasterName = string.Empty;
	public string disasterURLFriendlyName = string.Empty;
	public Guid eventId;
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		this.Master.PageTitle = "Stability - Select Impacted Counties";
		this.Master.PageDescription = "";
		this.Master.FbDescription = "";
		this.Master.FbSite_name = "Stability - Select Impacted Counties";
		this.Master.FbURL = Request.Url.AbsoluteUri;

		eventId = new Guid(Request.QueryString["eventId"]);

		var eventName = (from c in dc.Events
						where c.EventId == eventId
						select new { c.Name, c.URLFriendlyName }).SingleOrDefault();

		disasterName = eventName.Name;
		disasterURLFriendlyName = eventName.URLFriendlyName;

		if (!IsPostBack)
		{
			LoadCounties(eventId);
		}
	}

	protected void LoadCounties(Guid eventId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//Get a list of states for this disaster and use it to load counties
		//State id, code and name for this event.
		//var stateEvent = from es in dc.EventStates
		//				 join s in dc.USStates on es.StatesId equals s.StatesId
		//				  where es.EventId == eventId
		//				  orderby s.Name
		//				  select new { es.StatesId, s.Code, s.Name };

		var states =	from st in dc.USStates
						join es in dc.EventStates on st.StatesId equals es.StatesId
						join co in dc.Counties on st.StatesId equals co.StateId
						where es.EventId == eventId
						orderby st.Name
						group co by new { stateName = st.Name, county = co.Name, co.CountyId } into grp
						select new
						{
							displayName = "  " + grp.Key.stateName + " - " + grp.Key.county,
							grp.Key.CountyId
						};

		states = states.OrderBy(x => x.displayName).GroupBy(x => x.displayName).Select(x => x.FirstOrDefault());

		cblCounties.DataSource = states;

		cblCounties.DataBind();

		var countyEvents = from ce in dc.EventCounties
						   where ce.EventId == eventId
						  // && ce.CountyId == new Guid(item.Value)
						   select ce;

		//Preselect the counties
		if (countyEvents.Count() > 0)
		{
			foreach (var countyEvent in countyEvents)
			{
				for (int i = 0; i < cblCounties.Items.Count; i++)
				{
					if (countyEvent.CountyId.ToString() == cblCounties.Items[i].Value)
					{
						cblCounties.Items[i].Selected = true;
					}
				}
			}
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		Guid eventId = new Guid(Request.QueryString["eventId"]);

		divMessage.Visible = true;
		lblMessage.Text = "County information has been updated.";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//UPDATE LOCATION TYPES------------------------------------------
		foreach (ListItem item in cblCounties.Items)
		{
			//Are county event already connected?
			var countyEvents = from ce in dc.EventCounties
								where ce.EventId == eventId
								&& ce.CountyId == new Guid(item.Value)
								select ce;


			if (item.Selected)
			{

				if (countyEvents.Count() == 0)
				{
					//Does not exist, add it
					EventCounty eventCounty = new EventCounty();
					eventCounty.EventId = eventId;
					eventCounty.EventCountyId = Guid.NewGuid();
					eventCounty.CountyId = new Guid(item.Value);
					eventCounty.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
					eventCounty.CreatedOn = DateTime.Now;
					eventCounty.IsActive = true;
					dc.EventCounties.InsertOnSubmit(eventCounty);
					dc.SubmitChanges();
				}
			}
			else
			{
				//Delete any checked records
				if (countyEvents.Count() > 0)
				{
					//Item is selected.
					foreach (var countyEvent in countyEvents)
					{
						dc.EventCounties.DeleteOnSubmit(countyEvent);
						dc.SubmitChanges();
					}
				}
			}
		}
		//LoadCounties();
		Response.Redirect("/Disaster/" + disasterURLFriendlyName);
	}
}