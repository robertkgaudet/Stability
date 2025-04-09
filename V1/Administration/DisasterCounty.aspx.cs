using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
public partial class V1_Administration_DisasterCounty : BaseOrganizationWebForm
{
	public string disasterName = string.Empty;
	public string disasterURLFriendlyName = string.Empty;
	public Guid eventId;
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		this.Master.PageTitle = "Stability - Select Impacted Counties";
		this.Master.PageDescription = "";
		this.Master.FbDescription = "";
		this.Master.FbSite_name = "Stability - Select Impacted Counties";
		this.Master.FbURL = Request.Url.AbsoluteUri;
		eventId = new Guid(Request.QueryString["eventId"]);
		var eventName = (from c in dc.Events
						where c.EventId == eventId
						select new { c.Name, c.URLFriendlyName }).SingleOrDefault();
		disasterName = eventName.Name;
		disasterURLFriendlyName = eventName.URLFriendlyName;
		if (!IsPostBack)
		{
			LoadCounties(eventId);
		}
	}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
	protected void LoadCounties(Guid eventId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var states =	from st in dc.USStates
						join es in dc.EventStates on st.StatesId equals es.StatesId
						join co in dc.Counties on st.StatesId equals co.StateId
						where es.EventId == eventId
						orderby st.Name
						group co by new { stateName = st.Name, county = co.Name, co.CountyId } into grp
						select new
						{
							displayName = "  " + grp.Key.stateName + " - " + grp.Key.county,
							grp.Key.CountyId
						};
		states = states.OrderBy(x => x.displayName).GroupBy(x => x.displayName).Select(x => x.FirstOrDefault());

		cblCounties.DataSource = states;

		cblCounties.DataBind();

		var countyEvents = from ce in dc.EventCounties
						   where ce.EventId == eventId
						  // && ce.CountyId == new Guid(item.Value)
						   select ce;

		//Preselect the counties
		if (countyEvents.Count() > 0)
		{
			foreach (var countyEvent in countyEvents)
			{
				for (int i = 0; i < cblCounties.Items.Count; i++)
				{
					if (countyEvent.CountyId.ToString() == cblCounties.Items[i].Value)
					{
						cblCounties.Items[i].Selected = true;
					}
				}
			}
		}
	}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Guid eventId = new Guid(Request.QueryString["eventId"]);
        Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
        divMessage.Visible = true;

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            foreach (ListItem item in cblCounties.Items)
            {
                var countyEvents = from ce in dc.EventCounties
                                   where ce.EventId == eventId && ce.CountyId == new Guid(item.Value)
                                   select ce;

                if (item.Selected)
                {
                    if (countyEvents.Count() == 0)
                    {
                        var county = dc.Counties.FirstOrDefault(c => c.CountyId == new Guid(item.Value));

                        EventCounty eventCounty = new EventCounty();
                        eventCounty.EventId = eventId;
                        eventCounty.EventCountyId = Guid.NewGuid();
                        eventCounty.CountyId = county.CountyId;
                        eventCounty.CreatedBy = userId;
                        eventCounty.CreatedOn = DateTime.Now;
                        eventCounty.IsActive = true;
                        dc.EventCounties.InsertOnSubmit(eventCounty);
                    }
                }
                else
                {
                    foreach (var countyEvent in countyEvents)
                    {
                        dc.EventCounties.DeleteOnSubmit(countyEvent);
                    }
                }
            }
            dc.SubmitChanges();
            // Fetch all location types ONCE (reduces DB queries)
            var allLocationTypes = dc.LocationTypes.Select(lt => lt.Name).ToList();

            foreach (ListItem item in cblCounties.Items)
            {
                if (item.Selected)
                {
                    var county = dc.Counties.FirstOrDefault(c => c.CountyId == new Guid(item.Value));
                    if (county != null)
                    {
                       string address = string.Format("{0}, {1}", county.Name, county.State);
                      
                        // Process each location type
                        foreach (string locationType in allLocationTypes)
                        {
                            try
                            {
                                CallGetLatLngHandler(userId.ToString(), address, locationType);
                            }
                            catch (Exception ex)
                            {
                                // Log error (e.g., to a file or database)
                              
                            }
                        }
                    }
                }
            }

        }
        lblMessage.Text = "County information has been updated with Google Places data.";
        Response.Redirect("/Disaster/" + disasterURLFriendlyName);
    }
    private void CallGetLatLngHandler(string userId, string address, string locationType)
    {
        try
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string handlerUrl = baseUrl + "/V1/Handlers/GetLatitudeLongitude.ashx?userId=" +
                                HttpUtility.UrlEncode(userId) +
                                "&address=" + HttpUtility.UrlEncode(address) +
                                "&locationType=" + HttpUtility.UrlEncode(locationType);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(handlerUrl);
            request.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error calling GetLatitudeLongitude.ashx: " + ex.Message);
        }
    }



}