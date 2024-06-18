using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

//public class LinkCategoryParentCategory	
//{
//	public String Link { get; set; }
//	public String Title { get; set; }
//	public String Description { get; set; }
//	public String parentCateogry { get; set; }
//	public String Category { get; set; }
//}

public partial class V1_UserControls_ImpactedCommunities : System.Web.UI.UserControl
{
	Guid	_eventId			= Guid.Empty;

	public Guid eventId
	{
		get { return _eventId; }
		set { _eventId = value; }
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			LoadStates();
		}
	}

	public void LoadStates()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		IEnumerable<USState> states =	from s in dc.USStates
										join es in dc.EventStates on s.StatesId equals es.StatesId
										where es.EventId == eventId
										orderby s.Name
										select s;

		rptStates.DataSource = states;
		rptStates.DataBind();
	}

	protected void rptStates_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid statesId = (Guid)DataBinder.Eval(dataItem.DataItem, "StatesId");
			String code = (String)DataBinder.Eval(dataItem.DataItem, "Code");
			String stateName = (String)DataBinder.Eval(dataItem.DataItem, "Name");
			Literal litDivControl = e.Item.FindControl("litDivControl") as Literal;

			HyperLink hypState = e.Item.FindControl("hypState") as HyperLink;
			HyperLink hypEditCounties = e.Item.FindControl("hypEditCounties") as HyperLink;
			Literal lblEOCName = e.Item.FindControl("lblEOCName") as Literal;
			Literal lblEMName = e.Item.FindControl("lblEMName") as Literal;
			HyperLink hypPhoneNumber = e.Item.FindControl("hypPhoneNumber") as HyperLink;
			Literal lblWebsite = e.Item.FindControl("lblWebsite") as Literal;

			Literal litActiveNonprofitCount = e.Item.FindControl("litActiveNonprofitCount") as Literal;
			Literal litActiveShelterCount = e.Item.FindControl("litActiveShelterCount") as Literal;
			Literal litActiveFoodSites = e.Item.FindControl("litActiveFoodSites") as Literal;
			Literal litActiveDistributionLocations = e.Item.FindControl("litActiveDistributionLocations") as Literal;
			


			hypState.Text = stateName;
			hypState.Attributes.Add("href", "#" + code);
			HyperLink hypEditState = e.Item.FindControl("hypEditState") as HyperLink;
			hypEditState.NavigateUrl = "/V1/Administration/EmergencyManagement.aspx?eventId=" + eventId + "&stateCode=" + code;
			litDivControl.Text  = "<div id=\"" + code + "\" class=\"panel-body panel-collapse collapse m-b-lg\">";

			if (HttpContext.Current.User.IsInRole("Administrator"))
			{
				hypEditState.Visible = true;
				hypEditCounties.Visible = true;
				hypEditCounties.NavigateUrl = "/V1/Administration/DisasterCounty.aspx?eventId=" + eventId;
			}

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			//Count nonprofits, shelters, food sites and distribution locations per state.
			var organizationCount = from oe in dc.OrganizationEvents
									where oe.EventId == eventId
									&& oe.StagingStateId == statesId
									select oe;


			litActiveNonprofitCount.Text = organizationCount.Count().ToString();

			//Get shelter, food site and dist site id's
			var shelterType = (from lt in dc.LocationParentTypes
							where lt.Name == "Shelter"
							select new {lt.LocationParentTypeId }).SingleOrDefault();
			//Get shelter, food site and dist site id's
			var distribtionType = (from lt in dc.LocationParentTypes
								   where lt.Name == "Distribution Center"
								   select new { lt.LocationParentTypeId }).SingleOrDefault();
			//Get shelter, food site and dist site id's
			var preparedFoodType = (from lt in dc.LocationParentTypes
									where lt.Name == "Feeding Location"
							   select new { lt.LocationParentTypeId }).SingleOrDefault();

			var shelterCount = from lp in dc.LocationProfiles
							   join lpe in dc.LocationProfileEvents on lp.LocationProfileId equals lpe.LocationProfileId
							   join a in dc.Addresses on lp.AddressId equals a.AddressId
							   where lp.LocationParentTypeId == shelterType.LocationParentTypeId
							   && lp.IsActive == true //Location is set as active.
							   && lpe.EventId == eventId
							   && a.State == stateName
							   select lp;

			litActiveShelterCount.Text = shelterCount.Distinct().Count().ToString();

			var distributionCenterCount = from lp in dc.LocationProfiles
							   join lpe in dc.LocationProfileEvents on lp.LocationProfileId equals lpe.LocationProfileId
							   join a in dc.Addresses on lp.AddressId equals a.AddressId
							   where lp.LocationParentTypeId == distribtionType.LocationParentTypeId
							   && lp.IsActive == true //Location is set as active.
							   && lpe.EventId == eventId
							   && a.State == stateName
							   select lp;

			litActiveDistributionLocations.Text = distributionCenterCount.Distinct().Count().ToString();

			var preparedFoodCount = from lp in dc.LocationProfiles
							   join lpe in dc.LocationProfileEvents on lp.LocationProfileId equals lpe.LocationProfileId
							   join a in dc.Addresses on lp.AddressId equals a.AddressId
							   where lp.LocationParentTypeId == preparedFoodType.LocationParentTypeId
							   && lp.IsActive == true //Location is set as active.
							   && lpe.EventId == eventId
							   && a.State == stateName
							   select lp;

			litActiveFoodSites.Text = preparedFoodCount.Distinct().Count().ToString();

			//Get EM Info per state.
			var emergencyManagementInformation = (from em in dc.EmergencyManagments
												 where em.StatesId == statesId
												 && em.IsStateEOC == true
												 select em).SingleOrDefault();

			if(emergencyManagementInformation != null)
			{ 
				lblEOCName.Text = emergencyManagementInformation.EOCName;
				lblEMName.Text = emergencyManagementInformation.EmergencyManagerName;
				hypPhoneNumber.Text = Regex.Replace(emergencyManagementInformation.PhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypPhoneNumber.NavigateUrl = "tel:" + emergencyManagementInformation.PhoneNumber;
				lblWebsite.Text = emergencyManagementInformation.Website;
			}
			
			var counties = from co in dc.Counties
						   join ec in dc.EventCounties on co.CountyId equals ec.CountyId
						   where co.Code == code && ec.EventId == eventId
						   orderby co.Name
						   select new { ec.CountyId, co.Name, co.Code };

			Repeater rptCounties = e.Item.FindControl("rptCounties") as Repeater;

			rptCounties.DataSource = counties;
			rptCounties.DataBind();
		}
	}

	protected void rptCounties_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			String county = (String)DataBinder.Eval(dataItem.DataItem, "Name");
			Guid countyId = (Guid)DataBinder.Eval(dataItem.DataItem, "CountyId");
			string stateCode = (string)DataBinder.Eval(dataItem.DataItem, "Code");

			HyperLink hypCounty = (HyperLink)e.Item.FindControl("hypCounty");

			hypCounty.NavigateUrl = "/V1/CountyInfo.aspx?eventId=" + eventId + "&countyId=" + countyId.ToString() + "&stateCode=" + stateCode;
			hypCounty.Text = county;
		}
	}
}