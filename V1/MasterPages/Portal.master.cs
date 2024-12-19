using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_MasterPages_Portal : System.Web.UI.MasterPage
{
	public string _eventName = string.Empty;
	public string _eventId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if (!String.IsNullOrEmpty(_eventId))
		{
			var portal = (from ev in dc.Events
									 where ev.EventId == new Guid(_eventId)
									 select ev).SingleOrDefault();

			if (portal != null)
			{
				string isSimulation = portal.IsSimulation == null ? "" : ((bool)portal.IsSimulation ? " (<i class='fa fa-binoculars'></i> Simulation Only)" : "");

				_eventName = portal.Name + isSimulation;
				uc1EventHeader.TeamCount = "0";// organizationEventCount.Count().ToString();
				uc1EventHeader.CauseCount = Convert.ToString(Session["deploymentCount"]);

				uc1EventHeader.PageTitle = portal.Name + isSimulation;
				uc1EventHeader.EventName = portal.Name + isSimulation;

				uc1EventHeader.PageDescription = "Organize a disaster relief team for " + portal.Description;
				uc1EventHeader.TeamName = portal.Name + isSimulation;
				//uc1EventHeader.OrganizationId = organizationEvent.o.OrganizationId.ToString();

				Master.PageTitle = portal.Name;
			}
		}
	}

	public string EventId
	{
		get
		{
			return _eventId;
		}
		set
		{
			_eventId = value;
		}
	}
}
