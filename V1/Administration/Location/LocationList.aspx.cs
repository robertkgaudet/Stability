using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_Location_LocationList : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.PageTitle			= "Stability - Locations";
		this.Master.PageDescription		= "";
		this.Master.FbDescription		= "";
		this.Master.FbImage				= "Images/HurricaneMichael.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbSite_name			= "Stability - Locations";
		this.Master.FbURL				= Request.Url.AbsoluteUri;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var locations = from lp in dc.LocationProfiles
							join a in dc.Addresses on lp.AddressId equals a.AddressId
							join lpt in dc.LocationParentTypes on lp.LocationParentTypeId equals lpt.LocationParentTypeId
                            orderby lp.CreatedOn descending
                            where a.State == "Florida" && a.County == "Lee County"
						select new { locationTypeName = lpt.Name, a.State, lp.LocationProfileId, lp.IsActive, lp.IsOnMap, lp.Name, a.AddressId, lp.PointOfContactName, lp.PointOfContactPhoneNumber, a.FormattedAddress };

		rpLocations.DataSource = locations;//.Take(250);
		rpLocations.DataBind();
	}

	protected void dlLocationTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litAddress = (Literal)e.Item.FindControl("litAddress");
			HyperLink hypPhoneNumber = (HyperLink)e.Item.FindControl("hypPhoneNumber");
			HyperLink hypEdit = (HyperLink)e.Item.FindControl("hypEdit"); 
			Literal litPointOfContact = (Literal)e.Item.FindControl("litPointOfContact");
			Literal litType = (Literal)e.Item.FindControl("litType"); 
			Literal litOnMapIsActive = (Literal)e.Item.FindControl("litOnMapIsActive");
            Literal litState = (Literal)e.Item.FindControl("litState"); 

             //String locationName = (String)DataBinder.Eval(dataItem.DataItem, "Name");
             String formattedAddress = (String)DataBinder.Eval(dataItem.DataItem, "formattedAddress");
			String pointOfContactName = (String)DataBinder.Eval(dataItem.DataItem, "PointOfContactName");
			String pointOfContactPhoneNumber = (String)DataBinder.Eval(dataItem.DataItem, "PointOfContactPhoneNumber");
			Guid addressId = (Guid)DataBinder.Eval(dataItem.DataItem, "AddressId");
			String locationTypeName = (String)DataBinder.Eval(dataItem.DataItem, "locationTypeName");
            String state = (String)DataBinder.Eval(dataItem.DataItem, "State");
            Boolean isOnMap = (Boolean)DataBinder.Eval(dataItem.DataItem, "isOnMap");
			Boolean isActive = (Boolean)DataBinder.Eval(dataItem.DataItem, "isActive");

            litState.Text = state;


            litAddress.Text = formattedAddress;
			litPointOfContact.Text = pointOfContactName;
			hypPhoneNumber.Text = pointOfContactPhoneNumber;
			hypPhoneNumber.NavigateUrl = "tel:" + pointOfContactPhoneNumber;
			hypPhoneNumber.Style.Add("text-decoration", "underline");

			litOnMapIsActive.Text = (isActive ? "IsActive" : "InActive") + " / " + (isOnMap ? "IsMapped" : "UnMapped");

			litType.Text = locationTypeName;

			if (User.IsInRole("Administrator"))
			{ 
				hypEdit.Text = "Edit";
				hypEdit.NavigateUrl = "/V1/Administration/Location/EditLocation.aspx?addressId=" + addressId;
			}
			else
			{
				hypEdit.Visible = false;
			}
		}
	}
	
}