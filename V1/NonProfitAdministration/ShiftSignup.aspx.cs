using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_ShiftSignup : BaseWebForm
{
    string organizationEventId = string.Empty;
	string pageName = "ShiftSignup";

    /// <summary>
    /// This page loads a deployment's available shifts
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //TODO: DI this 
        var dbContext = new CrowdReliefDBDataContext();
        var deploymentsRepository = new DeploymentsRepository(dbContext);
        var positionsRepository = new PositionsRepository(dbContext);

        organizationEventId = Request.QueryString["organizationEventId"];
		ucPostionNavigation.PageName = pageName;
		ucPostionNavigation.OrganizationEventId = organizationEventId;
        
        var deployment = repository.GetDeployment(Guid.parse(organizationEventId))

        //display available shifts to user
        var availableShifts = positionsRepository.GetAvailableShifts(organizationEventId);

        //TODO: serialize to UI
    }
}