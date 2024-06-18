using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administration_BusinessList : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var business = from o in dc.Businesses
						   where o.IsActive == true
						   orderby o.Name
						   select o;

		rpBusiness.DataSource = business;
		rpBusiness.DataBind();
	}

	protected void dlBusinessTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litPointOfContactName = (Literal)e.Item.FindControl("litPointOfContactName"); 
			HyperLink hypPhoneNumber = (HyperLink)e.Item.FindControl("hypPhoneNumber");
			Literal litCityState = (Literal)e.Item.FindControl("litCityState"); 
			Literal litPurposeMission = (Literal)e.Item.FindControl("litPurposeMission"); 

			String pointOfContactName = (String)DataBinder.Eval(dataItem.DataItem, "PointOfContactName");
			String pointOfContactPhoneNumber = (String)DataBinder.Eval(dataItem.DataItem, "PointOfContactPhoneNumber");
			String cityState = (String)DataBinder.Eval(dataItem.DataItem, "City") + ", " + (String)DataBinder.Eval(dataItem.DataItem, "State");
			String purposeMission = (String)DataBinder.Eval(dataItem.DataItem, "PurposeMission");

			litPointOfContactName.Text	= pointOfContactName;
			hypPhoneNumber.Text			= pointOfContactPhoneNumber;
			hypPhoneNumber.NavigateUrl	= "tel:" + pointOfContactPhoneNumber;
			hypPhoneNumber.Style.Add("text-decoration","underline");
			litCityState.Text			= cityState;
			litPurposeMission.Text		= purposeMission;
		}
	}
}