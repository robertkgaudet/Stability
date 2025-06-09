using iTextSharp.text;
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
    private int pageSize = 50;
    private int pageNumber = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucMemberNavigation.UserId = userId.ToString();
            litPageName.Text = "Find Open Positions";
            organizationId = Request.QueryString["organizationId"];
            string searchTerm = Request.QueryString["searchTerm"];
            currentPageValue.Value = currentPageValue.Value == "" ? "1" : currentPageValue.Value;

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var deploymentTeams = new List<SearchResponse.VolunteerOpportunityInfo>();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                deploymentTeams = dc.ExecuteQuery<SearchResponse.VolunteerOpportunityInfo>(
                "EXEC NavSearchFilter {0}, {1},{2},{3}",
                "VolunteerOpportunities",
                string.IsNullOrWhiteSpace(searchTerm) ? "" : searchTerm, pageNumber, pageSize).ToList();
            }
            else
            {
                deploymentTeams = dc.ExecuteQuery<SearchResponse.VolunteerOpportunityInfo>(
                 "EXEC SearchDeploymentTeam {0}, {1},{2},{3}",
                 "VolunteerOpportunities",
                 string.IsNullOrWhiteSpace(searchTerm) ? "" : searchTerm, pageNumber, pageSize).ToList();
            }
            var totalCount = deploymentTeams.Any() ? deploymentTeams.First().TotalCount : 0;
            totalPageValue.Value = Convert.ToString(Math.Ceiling((double)totalCount / 50));
            rptDeploymentTeams.DataSource = deploymentTeams.ToList();
            rptDeploymentTeams.DataBind();
            hpanelMembers.Visible = true;
            ucMemberNavigation.UserId = userId.ToString();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenCollapse", "$('#searchFilters').collapse('show');", true);
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

    protected void SearchButton_Click(object sender, EventArgs e)
    {
      

        string positionTerm = position.Text.Trim().ToLower();
        string locationTerm = location.Text.Trim().ToLower();
        string teamTerm = team.Text.Trim().ToLower();
        DateTime? startDateParam = null;
        DateTime parsedStartDate;
        if (!string.IsNullOrWhiteSpace(StartDate.Text) && DateTime.TryParse(StartDate.Text, out parsedStartDate))
        {
            startDateParam = parsedStartDate;
        }
        DateTime? endDateParam = null;
        DateTime parsedEndDate;
        if (!string.IsNullOrWhiteSpace(EndDate.Text) && DateTime.TryParse(EndDate.Text, out parsedEndDate))
        {
            endDateParam = parsedEndDate;
        }
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        string startDateValue = startDateParam.HasValue ? startDateParam.Value.ToString("yyyy-MM-dd") : "";
        string endDateValue = endDateParam.HasValue ? endDateParam.Value.ToString("yyyy-MM-dd") : "";

        var deploymentTeams = dc.ExecuteQuery<SearchResponse.VolunteerOpportunityInfo>(
            "EXEC SearchDeploymentTeam {0}, {1}, {2}, {3}, {4}, {5}, {6}",
            locationTerm,
            string.IsNullOrWhiteSpace(teamTerm) ? "" : teamTerm,
            startDateValue,   
            endDateValue,
            positionTerm,
            currentPageValue.Value,
            pageSize
        ).ToList();

        var totalCount = deploymentTeams.Any() ? deploymentTeams.First().TotalCount : 0;
        totalPageValue.Value = Convert.ToString(Math.Ceiling((double)totalCount / 50));

        hpanelMembers.Visible = false;

        if (deploymentTeams.Count == 0)
        {
            rptDeploymentTeams.DataSource = null;
        }
        else
        {
            rptDeploymentTeams.DataSource = deploymentTeams;
        }
        currentPageValue.Value = currentPageValue.Value == "" ? "1" : currentPageValue.Value;

        rptDeploymentTeams.DataBind();
        hpanelMembers.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "updatePagination", "updatePagination();", true);
    }

}