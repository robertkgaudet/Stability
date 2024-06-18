using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administration_NonProfitList : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = from o in dc.Organizations
						   where o.IsActive == true
						   orderby o.CreatedOn descending
						   select o;

		rpNonProfit.DataSource = organization;
		rpNonProfit.DataBind();
	}

	protected void dlOrganizationTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litPointOfContactName = (Literal)e.Item.FindControl("litPointOfContactName"); 
			HyperLink hypPhoneNumber = (HyperLink)e.Item.FindControl("hypPhoneNumber");
			//HyperLink hypImpactoidWebsite = (HyperLink)e.Item.FindControl("hypImpactoidWebsite"); 
			Literal litCityState = (Literal)e.Item.FindControl("litCityState"); 
			Literal litPurposeMission = (Literal)e.Item.FindControl("litPurposeMission"); 

			String pointOfContactName = (String)DataBinder.Eval(dataItem.DataItem, "PointOfContactName");
			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "organizationId");
			String pointOfContactPhoneNumber = (String)DataBinder.Eval(dataItem.DataItem, "PointOfContactPhoneNumber");
			String cityState = (String)DataBinder.Eval(dataItem.DataItem, "City") + ", " + (String)DataBinder.Eval(dataItem.DataItem, "State");
			String purposeMission = (String)DataBinder.Eval(dataItem.DataItem, "PurposeMission");
			String URLFriendlyName = (String)DataBinder.Eval(dataItem.DataItem, "URLFriendlyName");
			string url = !String.IsNullOrEmpty(URLFriendlyName) ? "/Impactoid/CommunityPage.aspx?organizationName=" + URLFriendlyName : "/Impactoid/CommunityPage.aspx?organizationId=" + organizationId;
			//hypImpactoidWebsite.NavigateUrl = url;
			//hypImpactoidWebsite.Text = "Impactoid Website";
			litPointOfContactName.Text  = pointOfContactName;
			hypPhoneNumber.Text         = pointOfContactPhoneNumber;
			hypPhoneNumber.NavigateUrl	= "tel:" + pointOfContactPhoneNumber;
			hypPhoneNumber.Style.Add("text-decoration","underline");
			litCityState.Text			= cityState;
			litPurposeMission.Text		= purposeMission;
		}
	}
}