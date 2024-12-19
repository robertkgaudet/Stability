using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public partial class V1_Administration_NewDisaster : BaseOrganizationWebForm
{
	public string eventId = string.Empty;
	public string disasterTypeDropDown = string.Empty;
	public string preselectedStates = string.Empty;
	public string preselectedDisasterType = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.PageTitle = "Stability - Add New Community Portal";
		this.Master.PageDescription = "";
		this.Master.FbDescription = "";
		this.Master.FbSite_name = "Stability - Add New Community Portal";
		this.Master.FbURL = Request.Url.AbsoluteUri;


		eventId = Request.QueryString["eventId"];
		if (!IsPostBack && !String.IsNullOrEmpty(eventId))
		{
			//Edit mode
			btnSubmit.Text = "Update Disaster/Event";
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			//Load event information.
			var disasterEvent = (from ev in dc.Events
								 where ev.EventId == new Guid(eventId)
								 select ev).SingleOrDefault();

			txtBeginDate.Value = disasterEvent.BeginDate.ToString();
			if (disasterEvent.EndDate != null)
			{
				txtEndDate.Value = disasterEvent.EndDate.ToString();
			}
			if (disasterEvent.FEMADeclarationDate != null)
			{
				txtFEMADate.Value = disasterEvent.FEMADeclarationDate.ToString();
			}
			txtFEMAID.Value = disasterEvent.FEMADeclarationID;
			txtDisasterName.Value = disasterEvent.Name;
			txtDescription.Value = disasterEvent.Description;
			if (!String.IsNullOrEmpty(disasterEvent.Latitude))
			{
				txtLatitude.Value = disasterEvent.Latitude.ToString();
				txtLongitude.Value = disasterEvent.Longitude.ToString();
			}
			txtMapZoomLevel.Value = disasterEvent.Zoom.ToString();
			txtURLFriendlyName.Value = disasterEvent.URLFriendlyName;
			chkActive.Checked = disasterEvent.IsActive;
			chkSimulation.Checked = disasterEvent.IsSimulation == null ? false : (bool)disasterEvent.IsSimulation;

			//Select the various states.

			if (!String.IsNullOrEmpty(eventId))
			{
				//Select any states that are already selected.
				var eventStates = from es in dc.EventStates
								  where es.EventId == new Guid(eventId)
								  select es;

				if (eventStates != null)
				{
					//Load the statelist and select the correct state.
					foreach (var eventStateRecord in eventStates)
					{
						preselectedStates += "'" + eventStateRecord.StatesId + "',";
					}
				}
			}

			if (disasterEvent.EventTypeId != null)
			{
				var eventType = (from et in dc.EventTypes
								 where et.EventTypeId == disasterEvent.EventTypeId
								 select et).SingleOrDefault();

				if (eventType != null)
				{
					preselectedDisasterType = "$(\"#btn-dropdown.disasterEvent\").html('" + eventType.Name + "');";
					hidEventTypeId.Value = eventType.EventTypeId.ToString();
				}
			}
		}
		if (!IsPostBack)
		{
			LoadStates(eventId);
			LoadDisasters();
		}
	}

	protected void LoadStates(string eventId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var states = from s in dc.USStates
					 orderby s.Name
					 select s;

		ddlStates.DataSource = states;
		ddlStates.DataBind();
		ddlStates.Attributes.Add("multiple", "");
		ddlStates.Attributes.Add("style", "width:100%;");
	}

	public void LoadDisasters()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasterTypes = from et in dc.EventTypes
							orderby et.Name
							select new { et };

		int idNumber = 0;
		foreach (var disasterType in disasterTypes)
		{
			disasterTypeDropDown = disasterTypeDropDown + "<li id=\"" + disasterType.et.EventTypeId + "\"><a href=\"#\">" + disasterType.et.Name + "</a></li>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		//Insert EVENT
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		string disasterName = txtDisasterName.Value;
		string description = txtDescription.Value;
		string URLFriendlyName = txtURLFriendlyName.Value;
		string beginDate = txtBeginDate.Value;
		string endDate = txtEndDate.Value;
		string latitude = txtLatitude.Value;
		string longitude = txtLongitude.Value;
		int mapZoomLevel = int.Parse(txtMapZoomLevel.Value);
		string redirectURL = "/Maps/" + URLFriendlyName;
		string FEMADeclarationDATE = txtFEMADate.Value;
		string FEMADeclarationID = txtFEMAID.Value;

		if (eventId == null)
		{
			//Insert into event table.
			string GEOJSONFilePath = string.Empty;
			if(!String.IsNullOrEmpty(FEMADeclarationID) && fileGEOJson.HasFile)
			{
				GEOJSONFilePath = "/Uploads/" + Guid.NewGuid().ToString() + "_FEMA_" + fileGEOJson.FileName;
				//Upload the GEOJSONE FILE
				fileGEOJson.SaveAs(Server.MapPath(GEOJSONFilePath));
			}

			Guid newEventId = Guid.NewGuid();
			Event disasterEvent = new Event();

			if (!String.IsNullOrEmpty(beginDate))
			{
				disasterEvent.BeginDate = DateTime.Parse(beginDate);
			}

			if (!String.IsNullOrEmpty(endDate))
			{
				disasterEvent.EndDate = DateTime.Parse(endDate);
			}
			if (!String.IsNullOrEmpty(FEMADeclarationDATE))
			{
				disasterEvent.FEMADeclarationDate = DateTime.Parse(FEMADeclarationDATE);
			}
			if(!String.IsNullOrEmpty(GEOJSONFilePath))
			{
				disasterEvent.DeclaredGEOJSON = GEOJSONFilePath;
			}
			disasterEvent.FEMADeclarationID = FEMADeclarationID;
			disasterEvent.Name = disasterName;
			disasterEvent.Description = description;
			disasterEvent.Latitude = latitude; ;
			disasterEvent.Longitude = longitude;
			disasterEvent.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			disasterEvent.CreatedOn = DateTime.Now;
			disasterEvent.EventId = newEventId;
			disasterEvent.EventTypeId = new Guid(hidEventTypeId.Value);
			disasterEvent.Zoom = mapZoomLevel;
			disasterEvent.URLFriendlyName = URLFriendlyName;
			disasterEvent.IsActive = chkActive.Checked;
			disasterEvent.IsDisaster = true;
			disasterEvent.IsSimulation = chkSimulation.Checked;
			dc.Events.InsertOnSubmit(disasterEvent);
			dc.SubmitChanges();

			//Add impacted states 
			foreach (ListItem listItem in ddlStates.Items)
			{
				if (listItem.Selected == true)
				{

					Guid stateId = new Guid(listItem.Value);

					EventState eventState = new EventState();
					eventState.StatesId = stateId;
					eventState.EventId = newEventId;
					eventState.EventStateId = Guid.NewGuid();
					dc.EventStates.InsertOnSubmit(eventState);
					dc.SubmitChanges();
				}
			}
			//New entry, redirect so they choose counties.
			redirectURL = "DisasterCounty.aspx?eventId=" + newEventId.ToString();
		}
		else
		{

			//EDIT
			string GEOJSONFilePath = string.Empty;
			if (!String.IsNullOrEmpty(FEMADeclarationID) && fileGEOJson.HasFile)
			{
				GEOJSONFilePath = "/Uploads/" + Guid.NewGuid().ToString() + "_FEMA_" + fileGEOJson.FileName;
				//Upload the GEOJSONE FILE
				fileGEOJson.SaveAs(Server.MapPath(GEOJSONFilePath));
			}
			eventId = Request.QueryString["eventId"];
			var disasterEvent = (from de in dc.Events
								 where de.EventId == new Guid(eventId)
								 select de).SingleOrDefault();

			if (!String.IsNullOrEmpty(beginDate))
			{
				disasterEvent.BeginDate = DateTime.Parse(beginDate);
			}

			if (!String.IsNullOrEmpty(endDate))
			{
				disasterEvent.EndDate = DateTime.Parse(endDate);
			}
			if (!String.IsNullOrEmpty(FEMADeclarationDATE))
			{
				disasterEvent.FEMADeclarationDate = DateTime.Parse(FEMADeclarationDATE);
			}
			if (!String.IsNullOrEmpty(GEOJSONFilePath))
			{
				disasterEvent.DeclaredGEOJSON = GEOJSONFilePath;
			}
			disasterEvent.FEMADeclarationID = FEMADeclarationID;
			disasterEvent.Name = disasterName;
			disasterEvent.Description = description;
			disasterEvent.Latitude = latitude; ;
			disasterEvent.Longitude = longitude;
			//disasterEvent.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			//disasterEvent.CreatedOn = DateTime.Now;
			disasterEvent.EventId = new Guid(eventId);
			disasterEvent.EventTypeId = new Guid(hidEventTypeId.Value);
			disasterEvent.Zoom = mapZoomLevel;
			disasterEvent.URLFriendlyName = URLFriendlyName;
			disasterEvent.IsActive = chkActive.Checked;
			disasterEvent.IsSimulation = chkSimulation.Checked;
			dc.SubmitChanges();

			foreach (ListItem listItem in ddlStates.Items)
			{
				//If item is selected, make sure it remains selected.
				var eventStates = from es in dc.EventStates
								  where es.EventId == new Guid(eventId)
								  && es.StatesId == new Guid(listItem.Value)
								  select es;

				if (listItem.Selected)
				{
					if (eventStates.Count() == 0)
					{
						//Adding new states so send to the county selector.
						redirectURL = "DisasterCounty.aspx?eventId=" + eventId.ToString();

						//Does not exist, add it
						Guid stateId = new Guid(listItem.Value);
						EventState eventState = new EventState();
						eventState.StatesId = stateId;
						eventState.EventId = new Guid(eventId);
						eventState.EventStateId = Guid.NewGuid();
						dc.EventStates.InsertOnSubmit(eventState);
						dc.SubmitChanges();
					}
				}
				else
				{
					if (eventStates.Count() > 0)
					{
						//Delete any checked records
						//Removing so send to the county selector.
						//redirectURL = "DisasterCounty.aspx?eventId=" + eventId.ToString();

						//Item is selected.
						foreach (var eventState in eventStates)
						{
							//Join on cityextended and delete where countyid matches.
							//TODO REMOVE ANY CHECKED COUNTIES FOR THIS STATE FROM THE EVENTCOUNTY TABLE AS WELL WHERE THIS STATE MATCHES
							var eventCounties = from co in dc.Counties
												join ec in dc.EventCounties on co.CountyId equals ec.CountyId
												join s in dc.USStates on co.StateId equals s.StatesId
												where ec.EventId == new Guid(eventId) && co.StateId == new Guid(listItem.Value)
												select ec;

							//Item is selected.
							foreach (var eventCounty in eventCounties)
							{
								dc.EventCounties.DeleteOnSubmit(eventCounty);
								dc.SubmitChanges();
							}
							dc.EventStates.DeleteOnSubmit(eventState);
							dc.SubmitChanges();
						}
					}
				}
			}
		}

		//Redirect to county association page.
		Response.Redirect(redirectURL);

	}

	private string ConvertKMZtoGEOJSON(FileUpload fileGEOJson, string FEMADeclarationID)
	{
		string GEOJsonGuid = Guid.NewGuid().ToString();
		string GEOJsonFilePath = string.Empty;
		string filePath = Server.MapPath("/App_Data/FEMA_" + fileGEOJson.FileName);
		fileGEOJson.SaveAs(filePath);

		// Call the async method without using async/await, as the button click can't be async in C# v5.
		string geoJson = ConvertKmzToGeoJson(filePath);
		if (geoJson != null)
		{
			string FilePath = "~/App_Data/" + FEMADeclarationID + "_FEMA_" + GEOJsonGuid + ".geojson";
			GEOJsonFilePath = Server.MapPath(FilePath);
			File.WriteAllText(GEOJsonFilePath, geoJson);
		}
		else
		{
		}
		return GEOJsonFilePath;
	}

	private string ConvertKmzToGeoJson(string kmzFilePath)
	{
		string apiKey = "cONDCvUaaxVgsLY"; // Replace with your actual API key
		string apiUrl = "https://mygeodata.cloud/api/converter";

		using (var client = new HttpClient())
		{
			var formData = new MultipartFormDataContent();
			formData.Add(new StringContent(apiKey), "key");
			formData.Add(new StringContent("geojson"), "output_format");
			formData.Add(new ByteArrayContent(File.ReadAllBytes(kmzFilePath)), "file", Path.GetFileName(kmzFilePath));

			// Synchronous Post request
			HttpResponseMessage response = client.PostAsync(apiUrl, formData).Result;

			if (response.IsSuccessStatusCode)
			{
				// Synchronously read the response content
				string jsonResponse = response.Content.ReadAsStringAsync().Result;
				return jsonResponse; // Return GeoJSON content
			}
			else
			{
				string errorContent = response.Content.ReadAsStringAsync().Result;
				string errorMessage = "Error " + response.StatusCode + " " + errorContent; // Display error info
				return null; // Indicate failure
			}
		}
	}
}