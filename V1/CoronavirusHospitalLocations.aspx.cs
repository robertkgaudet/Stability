using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_CoronavirusHospitalLocations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.FbImage = "V1/Images/neworleanscovid.jpg";
		this.Master.FbImageType = "image/jpg";
		this.Master.PageTitle = "Stability - Covid-19 Pandemic Response Locations in the New Orleans Area";
		this.Master.FbSite_name = "Stability - Covid-19 Pandemic Response Locations in the New Orleans Area";
		this.Master.PageDescription = "List of drive by test sites, and other health locations in the New Orleans area responding to the Coronavirus, COVID-19.";
		this.Master.FbDescription = "List of drive by test sites, and other health locations in the New Orleans area responding to the Coronavirus, COVID-19.";
		this.Master.FbURL = Request.Url.AbsoluteUri;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var locations = from lp in dc.LocationProfiles
						join a in dc.Addresses on lp.AddressId equals a.AddressId
						join lpt in dc.LocationParentTypes on lp.LocationParentTypeId equals lpt.LocationParentTypeId
						join llt in dc.LocationLocationTypes on lp.AddressId equals llt.AddressId
						join lt in dc.LocationTypes on llt.LocationTypeId equals lt.LocationTypeId
						orderby lp.CreatedOn descending
						where  lt.Name == "Pandemic Response" // && a.State == "Louisiana" && a.County == "Orleans Parish"
						select new { locationTypeName = lpt.Name, a.City, a.State, lp.LocationProfileId, lp.IsActive, lp.IsOnMap, lp.Name, a.AddressId, lp.PointOfContactName, lp.PointOfContactPhoneNumber, a.FormattedAddress };

		rpHealthFacilities.DataSource = locations;
		rpHealthFacilities.DataBind();

		lblAddLocation.Visible = false;
		if (User.IsInRole("Administrator") | User.IsInRole("Location Administrator") | User.IsInRole("Location Manager"))
		{
			lblAddLocation.Visible = true;
		}
	}

	protected void rpHealthFacilities_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litAddress = (Literal)e.Item.FindControl("litAddress");
			Literal litServices = (Literal)e.Item.FindControl("litServices");
			Literal litType = (Literal)e.Item.FindControl("litType");
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

			//Get a list of services.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var services = from lt in dc.LocationTypes
						   join llt in dc.LocationLocationTypes on lt.LocationTypeId equals llt.LocationTypeId
						   where llt.AddressId == addressId
						   select new { lt.Name };

			string servicesList = string.Empty;

			foreach (var service in services)
			{
				servicesList += service.Name + ", ";
			}

			litServices.Text = servicesList.Substring(0, servicesList.Length-2);

			litAddress.Text = formattedAddress;
			litType.Text = locationTypeName;
		}
	}
}