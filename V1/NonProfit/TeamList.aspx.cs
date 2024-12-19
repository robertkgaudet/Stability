using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_TeamList : BaseWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		Guid prioritizedId = new Guid("79305f85-3816-46a8-911f-0d7e3e227c32");

		var teams = from t in dc.Organizations
					where t.IsActive == true
					orderby t.CreatedOn
					select new { t.Name, t.Description, t.LogoSquare, t.OrganizationId, t.URLFriendlyName, t.CreatedOn };

		rptTeams.DataSource = teams.OrderByDescending(d => d.OrganizationId == prioritizedId).ThenByDescending(o => o.CreatedOn).ToList();
		rptTeams.DataBind();

		Master.PageName = "Stability Teams";
		litCount.Text = teams.Count().ToString() + " Teams";
		if (User.Identity.IsAuthenticated )
		{ 
			ucMemberNavigation.UserId = userId.ToString();
		}
		litPageName.Text = "Non-Profits, Groups and Teams";

		string team = Request.QueryString["team"];
		if(!String.IsNullOrEmpty(team))
		{
			//Show a alert to pick a team.
			divJoinTeamMessage.Visible = true;
		}
	}

	protected void rptTeams_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			string campaignImageFolder = System.Configuration.ConfigurationManager.AppSettings["CampaignImageFolder"].ToString();

			Image imgLogo = (Image)e.Item.FindControl("imgLogo");
			HyperLink hypTeamName = (HyperLink)e.Item.FindControl("hypTeamName");
			Label lblDescription = (Label)e.Item.FindControl("lblDescription");

			RepeaterItem dataItem = (RepeaterItem)e.Item;
			//Total count of items and total cost.
			string teamName = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			string description = (string)DataBinder.Eval(dataItem.DataItem, "Description");
			string logoSquare = (string)DataBinder.Eval(dataItem.DataItem, "LogoSquare");
			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId");

			if (!String.IsNullOrEmpty(logoSquare))
			{
				logoSquare = "/Impactoid/Images/Logos/" + logoSquare;
				imgLogo.Width = 50;
			}
			else
			{
				logoSquare = "/V1/Images/Logo-Placeholder.png";
				imgLogo.Width = 50;
			}
			imgLogo.ImageUrl = logoSquare;
			hypTeamName.Text = teamName;
			hypTeamName.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId.ToString();
			//lblDescription.Text = description;
		}
	}
}