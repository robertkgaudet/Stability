using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Administration_Location_EditLocationRelationships : BaseOrganizationWebForm
{
	public string locationStatusDropDown = string.Empty;
	public string preselectedLocationStatusJQuery = string.Empty;

	public string locationTypeDropDown = string.Empty;
	public string preselectedLocationTypeJQuery = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		string locationParentProfileTypeId = string.Empty;
		if (!IsPostBack)
		{
			LoadDisasters();
			LoadLocationTypes();
			LoadLocationStatuses();

			string locationProfileId = Request.QueryString["locationProfileId"];
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var locationProfile = (from lp in dc.LocationProfiles
									where lp.LocationProfileId == new Guid(locationProfileId)
									select new { lp.Name, lp.LocationParentTypeId }).SingleOrDefault();
			hypLocation.NavigateUrl = "/V1/Location.aspx?locationProfileId=" + locationProfileId;
			hypLocation.Text = "Back to " + locationProfile.Name;
			locationParentProfileTypeId = locationProfile.LocationParentTypeId.ToString();
			
			LoadLocationParentTypes(locationParentProfileTypeId);
		}
	}

	protected void LoadLocationParentTypes(string locationParentTypeId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if (!String.IsNullOrEmpty(locationParentTypeId))
		{
			var currentLocationParentType = (from lpt in dc.LocationParentTypes
										 where lpt.LocationParentTypeId == new Guid(locationParentTypeId)
										 select new { lpt.Name }).SingleOrDefault();

			if (currentLocationParentType != null)
			{
				preselectedLocationTypeJQuery = "$(\"#btn-dropdown-type.locationType\").html('" + currentLocationParentType.Name + "');";
				hidLocationParentTypeId.Value = locationParentTypeId.ToString();
			}
		}

		var locationParentTypes = from lpt in dc.LocationParentTypes
							   orderby lpt.Name
							   select new { lpt };
	
		foreach (var locationParentType in locationParentTypes)
		{
			locationTypeDropDown = locationTypeDropDown + "<li id=\"" + locationParentType.lpt.LocationParentTypeId + "\"><a href=\"#\">" + locationParentType.lpt.Name + "</a></li>" + Environment.NewLine;
		}
	}

	protected void LoadLocationStatuses()
	{
		string addressId = Request.QueryString["addressId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//Show the latest location status.

		var currentLocationStatus = (from lls in dc.LocationLocationStatus
									 join ls in dc.LocationStatus on lls.LocationStatusId equals ls.LocationStatusId
									 where lls.AddressId == new Guid(addressId)
									 orderby lls.UpdatedOn descending
									 select new { ls.Name, ls.LocationStatusId }).Take(1).SingleOrDefault();

		if(currentLocationStatus != null)
		{ 
			preselectedLocationStatusJQuery = "$(\"#btn-dropdown.locationStatus\").html('" + currentLocationStatus.Name + "');";
			hidLocationStatusId.Value = currentLocationStatus.LocationStatusId.ToString();
		}

		var locationStatuses = from ls in dc.LocationStatus
						orderby ls.Name 
						select new { ls };

		int idNumber = 0;
		foreach (var locationStatuse in locationStatuses)
		{
			locationStatusDropDown = locationStatusDropDown + "<li id=\"" + locationStatuse.ls.LocationStatusId + "\"><a href=\"#\">" + locationStatuse.ls.Name + "</a></li>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
	}

	protected void LoadLocationTypes()
	{
		string locationProfileId = Request.QueryString["locationProfileId"];
		string addressId = Request.QueryString["addressId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var events = from lt in dc.LocationTypes
					 orderby lt.Name
					 select new { name = " - " + lt.Name, lt.LocationTypeId };

		chkBoxLocationTypes.DataSource = events;
		chkBoxLocationTypes.DataBind();


		var locationLocationTypes = from llt in dc.LocationLocationTypes
									where llt.AddressId == new Guid(addressId)
									select llt;

		//Preselect the orgs for this user
		if (locationLocationTypes.Count() > 0)
		{
			foreach (var locationLocationType in locationLocationTypes)
			{
				for (int i = 0; i < chkBoxLocationTypes.Items.Count; i++)
				{
					if (locationLocationType.LocationTypeId.ToString() == chkBoxLocationTypes.Items[i].Value)
					{
						chkBoxLocationTypes.Items[i].Selected = true;
					}
				}
			}
		}
	}

	protected void LoadDisasters()
	{
		string locationProfileId = Request.QueryString["locationProfileId"];
		string addressId = Request.QueryString["addressId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var events = from c in dc.Events
					 where c.IsDisaster == true
					 orderby c.BeginDate descending
					 select new { name = " - " + c.Name, c.EventId };

		chkBoxListDisasters.DataSource = events;
		chkBoxListDisasters.DataBind();

		var locationProfileEvents = from lpe in dc.LocationProfileEvents
									where lpe.LocationProfileId == new Guid(locationProfileId)
									select lpe;

		//Preselect the orgs for this user
		if (locationProfileEvents.Count() > 0)
		{
			foreach (var userEvent in locationProfileEvents)
			{
				for (int i = 0; i < chkBoxListDisasters.Items.Count; i++)
				{
					if (userEvent.EventId.ToString() == chkBoxListDisasters.Items[i].Value)
					{
						chkBoxListDisasters.Items[i].Selected = true;
					}
				}
			}
		}
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		string locationProfileId = Request.QueryString["locationProfileId"];
		Response.Redirect("/V1/Location.aspx?locationProfileId=" + locationProfileId);
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string locationStatusId = hidLocationStatusId.Value;
		string locationParentTypeId = hidLocationParentTypeId.Value;
		string locationProfileId = Request.QueryString["locationProfileId"];
		string addressId = Request.QueryString["addressId"];
		divMessage.Visible = true;
		lblMessage.Text = "Your location information has been updated.";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (!string.IsNullOrEmpty(locationParentTypeId))
		{
			//Update location Profile.

			var locationProfile = (from lp in dc.LocationProfiles
								  where lp.AddressId == new Guid(addressId)
								  select lp).SingleOrDefault();
			locationProfile.LocationParentTypeId = new Guid(locationParentTypeId);
			dc.SubmitChanges();
		}

		if (!string.IsNullOrEmpty(locationStatusId))
		{ 
			LocationLocationStatus locationLocationStatus = new LocationLocationStatus();
			locationLocationStatus.AddressId = new Guid(addressId);
			locationLocationStatus.CreatedBy = userId;
			locationLocationStatus.CreatedOn = DateTime.Now;
			locationLocationStatus.LocationLocationStatusId = Guid.NewGuid();
			locationLocationStatus.LocationStatusId = new Guid(locationStatusId);
			locationLocationStatus.UpdatedBy = userId;
			locationLocationStatus.UpdatedOn = DateTime.Now;
			dc.LocationLocationStatus.InsertOnSubmit(locationLocationStatus);
			dc.SubmitChanges();
		}

		//UPDATE LOCATION TYPES------------------------------------------
		foreach (ListItem item in chkBoxLocationTypes.Items)
		{
			var locationLocationTypes = from llt in dc.LocationLocationTypes
										where llt.AddressId == new Guid(addressId)
										&& llt.LocationTypeId == new Guid(item.Value)
										select llt;
			if (item.Selected)
			{
				if (locationLocationTypes.Count() == 0)
				{
					LocationLocationType locationLocationType = new LocationLocationType();
					locationLocationType.AddressId = new Guid(addressId);
					locationLocationType.LocationLocationTypeId = Guid.NewGuid();
					locationLocationType.LocationTypeId = new Guid(item.Value);
					dc.LocationLocationTypes.InsertOnSubmit(locationLocationType);
					dc.SubmitChanges();
				}
			}
			else
			{
				//Delete any checked records
				if (locationLocationTypes.Count() > 0)
				{
					//Item is selected.
					foreach (var locationLocationType in locationLocationTypes)
					{
						dc.LocationLocationTypes.DeleteOnSubmit(locationLocationType);
						dc.SubmitChanges();
					}
				}
			}
		}










		//UPDATE DISASTERS------------------------------------------
		foreach (ListItem item in chkBoxListDisasters.Items)
		{
			var locationProfileEvents = from lpe in dc.LocationProfileEvents
										where lpe.LocationProfileId == new Guid(locationProfileId)
										&& lpe.EventId == new Guid(item.Value)
										select lpe;
			if (item.Selected)
			{
				if (locationProfileEvents.Count() == 0)
				{
					LocationProfileEvent locationProfileEvent = new LocationProfileEvent();
					locationProfileEvent.LocationProfileEventId = Guid.NewGuid();
					locationProfileEvent.AddressId = new Guid(addressId);
					locationProfileEvent.EventId = new Guid(item.Value);
					locationProfileEvent.LocationProfileId = new Guid(locationProfileId);
					dc.LocationProfileEvents.InsertOnSubmit(locationProfileEvent);
					dc.SubmitChanges();
				}
			}
			else
			{
				//Delete any checked records
				if (locationProfileEvents.Count() > 0)
				{
					//Item is selected.
					foreach (var locationProfileEventCheck in locationProfileEvents)
					{
						dc.LocationProfileEvents.DeleteOnSubmit(locationProfileEventCheck);
						dc.SubmitChanges();
					}
				}
			}
		}
		LoadLocationStatuses();
		LoadLocationParentTypes(locationParentTypeId);
		Response.Redirect("/V1/Location.aspx?locationProfileId=" + locationProfileId);
	}
}