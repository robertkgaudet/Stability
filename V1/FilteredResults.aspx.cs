using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Configuration;

public partial class V1_FilteredResults : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string searchTerm = Request.QueryString["searchTerm"];
            string searchType = Request.QueryString["searchType"];

            if (!string.IsNullOrEmpty(searchTerm))
            {
                BindResults(searchType, searchTerm);
            }
        }
    }

    private void BindResults(string searchType, string searchTerm)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        List<string> proceduresToRun = new List<string>();

        if (string.IsNullOrEmpty(searchType))
        {
            proceduresToRun.AddRange(new string[] {
                "Search_Posts", "Search_Teams", "Search_Skills",
                "Search_Resources", "Search_Deployments", "Search_Portals",
                "Search_VolunteerOpportunities"
            });
        }
        else
        {
            proceduresToRun.Add(GetStoredProcedureName(searchType));
        }

        List<dynamic> allResults = new List<dynamic>();

        foreach (var spName in proceduresToRun)
        {
            string query = string.Format("EXEC {0} {{0}}", spName);
            var result = dc.ExecuteQuery<dynamic>(query, searchTerm).ToList();

            foreach (var item in result)
            {
                allResults.Add(item);
            }
            ucPeopleSearch.Visible = true;
            ucPeopleSearch.BindData(allResults);
        }

    }

    private string GetStoredProcedureName(string searchType)
    {
        switch (searchType)
        {
            case "TeamMembers": return "Search_Profiles";
            case "Teams": return "Search_Teams";
            case "Skills": return "Search_Skills";
            case "Resources": return "Search_Resources";
            case "Deployments": return "Search_Deployments";
            case "Portals": return "Search_Portals";
            default: return "";
        }
    }
}
