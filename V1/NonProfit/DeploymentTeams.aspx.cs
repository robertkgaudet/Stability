using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_DeploymentTeams : BaseWebForm
{
    public string organizationId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        ucMemberNavigation.UserId = userId.ToString();
        litPageName.Text = "Find Open Positions";
        organizationId = Request.QueryString["organizationId"];

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var deploymentTeams = (from oep in dc.OrganizationEventPositions
                               join oe in dc.OrganizationEvents on oep.OrganizationEventId equals oe.OrganizationEventId
                               join p in dc.Positions on oep.PositionId equals p.PositionId
                               join county in dc.Counties on oe.StagingCountyId equals county.CountyId
                               join city in dc.Cities on county.Code equals city.Code
                               where oe.IsActive == true
                               && oe.OrganizationId == new Guid(organizationId)
                               && oep.DeploymentDate >= DateTime.Now
                               select new
                               {
                                   oe.OrganizationEventId,
                                   CampaignName = oe.CampaignName,
                                   oep.DeploymentDate,
                                   oe.URLFriendlyCampaignName,
                                   County = county.Name,
                                   State = county.State,
                                   City = city.City1,
                                   EarliestDeploymentDate = (from innerOep in dc.OrganizationEventPositions
                                                             where innerOep.OrganizationEventId == oe.OrganizationEventId
                                                             select innerOep.DeploymentDate).Min(),
                                   LatestDeploymentDate = (from innerOep in dc.OrganizationEventPositions
                                                           where innerOep.OrganizationEventId == oe.OrganizationEventId
                                                           select innerOep.DeploymentDate).Max(),
                                   PositionCount = (from innerOep in dc.OrganizationEventPositions
                                                    where innerOep.OrganizationEventId == oe.OrganizationEventId
                                                    select innerOep.PositionId).Distinct().Count(),
                                   RowNum = (from innerOep in dc.OrganizationEventPositions
                                             where innerOep.OrganizationEventId == oe.OrganizationEventId
                                             orderby innerOep.DeploymentDate ascending
                                             select innerOep.DeploymentDate).FirstOrDefault()
                               }).ToList()
                     .GroupBy(x => x.OrganizationEventId)
                     .Select(g => g.OrderBy(x => x.RowNum).First())
                     .OrderByDescending(x => x.DeploymentDate);


        rptDeploymentTeams.DataSource = deploymentTeams.ToList();
        rptDeploymentTeams.DataBind();

        ucMemberNavigation.UserId = userId.ToString();
    }

    protected void rptDeploymentTeams_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //Get the positions.
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RepeaterItem dataItem = (RepeaterItem)e.Item;

            HyperLink hypCampaignName = (HyperLink)e.Item.FindControl("hypCampaignName");
            Label lblEarliestDeploymentDate = (Label)e.Item.FindControl("lblEarliestDeploymentDate");
            Label lbLatestDeploymentDate = (Label)e.Item.FindControl("lbLatestDeploymentDate");
            Label lblDeploymentLocation = (Label)e.Item.FindControl("lblDeploymentLocation");
            HyperLink hypPositions = (HyperLink)e.Item.FindControl("hypPositions");
            Label lblPositionCount = (Label)e.Item.FindControl("lblPositionCount");

            Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");
            String campaignName = (String)DataBinder.Eval(dataItem.DataItem, "CampaignName");
            DateTime earliestDeploymentDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "EarliestDeploymentDate");
            DateTime latestDeploymentDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "LatestDeploymentDate");
            String URLFriendlyCampaignName = (String)DataBinder.Eval(dataItem.DataItem, "URLFriendlyCampaignName");
            String county = (String)DataBinder.Eval(dataItem.DataItem, "County");
            String state = (String)DataBinder.Eval(dataItem.DataItem, "State");
            String city = (String)DataBinder.Eval(dataItem.DataItem, "City");
            int positionCount = (int)DataBinder.Eval(dataItem.DataItem, "PositionCount");

            lblEarliestDeploymentDate.Text = earliestDeploymentDate.ToString("ddd, MMM dd yyyy");
            lbLatestDeploymentDate.Text = latestDeploymentDate.ToString("ddd, MMM dd yyyy");
            lblDeploymentLocation.Text = city + ", " + state + "<br>(" + county + " County)";
            hypCampaignName.Text = campaignName;
            hypCampaignName.NavigateUrl = "/Cause/" + URLFriendlyCampaignName;
            hypPositions.NavigateUrl = "/SignUp/" + URLFriendlyCampaignName;
            lblPositionCount.Text = positionCount.ToString();
        }
    }
}