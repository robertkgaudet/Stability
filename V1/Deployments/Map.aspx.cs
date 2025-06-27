using System;
using System.Configuration;
using System.Linq;
using System.Web;

public partial class V1_Deployments_Map : BaseWebForm
{
    public string disasterDropDown = string.Empty;
    public string _eventName = string.Empty;
    public string _latitude = string.Empty;
    public string _longitude = string.Empty;
    public string _zoom = string.Empty;
    public string _eventId = string.Empty;
    public string host = HttpContext.Current.Request.Url.Host;
    public string mapApiKey = ConfigurationManager.AppSettings["mapApiKey"].ToString();
    public string _mapGEOJsonPath = String.Empty;
    public string liCases = string.Empty;
    public string locationTypeDropDown = string.Empty;
    public string locationParentTypeDropDown = string.Empty;
    public string locationStatusDropDown = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (host == "localhost")
        {
            host = "http://" + host + ":" + HttpContext.Current.Request.Url.Port;
        }
        else
        {
            host = "https://" + host;
        }

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["eventName"]))
        {
            //we have the event name...
            string eventName = HttpContext.Current.Request.QueryString["eventName"];

            //Get the eventId from the eventname
            var portal = (from ev in dc.Events
                          where ev.URLFriendlyName == eventName
                          select ev).SingleOrDefault();

            if (portal != null)
            {
                string disasterSimulation = portal.IsSimulation == null ? string.Empty : ((bool)portal.IsSimulation ? " <i class=\"fa fa-binoculars\"></i> " : "");
                _eventId = portal.EventId.ToString();
                Master._eventName = disasterSimulation + portal.Name;
                Master.EventId = _eventId;
                _eventName = disasterSimulation + portal.Name;

                _mapGEOJsonPath = portal.DeclaredGEOJSON;

                if (!string.IsNullOrEmpty(portal.Latitude))
                {
                    _latitude = portal.Latitude;
                    _longitude = portal.Longitude;
                    _zoom = portal.Zoom.ToString();
                }
            }
        }
        else
        {
            //we don't have any identifier so pick the most recent disaster to load first.
            var portal = (from ev in dc.Events
                          where ev.IsActive == true
                          orderby ev.CreatedOn descending
                          select ev).Take(1).SingleOrDefault();

            if (portal != null)
            {
                string disasterSimulation = portal.IsSimulation == null ? string.Empty : ((bool)portal.IsSimulation ? " <i class=\"fa fa-binoculars\"></i> " : "");
                _eventId = portal.EventId.ToString();
                Master._eventName = disasterSimulation + portal.Name;
                Master.EventId = _eventId;
                _eventName = disasterSimulation + portal.Name;

                _mapGEOJsonPath = portal.DeclaredGEOJSON;

                if (!string.IsNullOrEmpty(portal.Latitude))
                {
                    _latitude = portal.Latitude;
                    _longitude = portal.Longitude;
                    _zoom = portal.Zoom.ToString();
                }
            }
        }

        string createDeploymentURL = string.Empty;
        string createDeploymentTEXT = "";
        if (User.Identity.IsAuthenticated)
        {
            hypCreateDeployment.Text = "Add Your Deployment";

            //What if user is not a group owner?
            //Group owners can create their own deployments.
            var isOwner = (from o in dc.Organizations
                           where o.OwnerId == userId
                           select o).Take(1).SingleOrDefault();

            if (isOwner != null)
            {
                //Show cases in drop down list.
                liCases = "<li id=\"Cases\"><a href=\"#\">Cases</a></li>";
                if (!String.IsNullOrEmpty(_eventId))
                {
                    //Has event Id
                    createDeploymentURL = "/V1/NonProfitAdministration/RespondToEvent.aspx?eventId=" + _eventId + "&organizationId=" + isOwner.OrganizationId;
                }
                else
                {
                    //No event Id
                    createDeploymentURL = "/V1/NonProfitAdministration/RespondToEvent.aspx?organizationId=" + isOwner.OrganizationId;
                }
                //User has a group
                createDeploymentTEXT = "Add Your Deployment";
            }
            else
            {
                if (User.IsInRole("CaseManager") || User.IsInRole("Administrator"))
                {
                    //Show cases in drop down list.
                    liCases = "<li id=\"Cases\"><a href=\"#\">Cases</a></li>";
                }
                createDeploymentTEXT = "Add Your Group To Create Your Own Deployment";
                createDeploymentURL = "/V1/NonProfit/NonProfitNew.aspx";
            }
        }
        else
        {
            //User is not signed in, send them to sign in page with URL redirect attached.
            createDeploymentTEXT = "Register";
            createDeploymentURL = "/Register";
            hypSignIn.Visible = true;
        }
        hypCreateDeployment.NavigateUrl = createDeploymentURL;
        hypCreateDeployment.Text = createDeploymentTEXT;

        LoadDisasters();       
    }
    
    public void LoadDisasters()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var disasters = from d in dc.Events
                        orderby d.BeginDate descending
                        where d.IsActive == true
                        select new { d };

        int idNumber = 0;
        foreach (var disaster in disasters)
        {
            string disasterSimulation = disaster.d.IsSimulation == null ? string.Empty : ((bool)disaster.d.IsSimulation ? "<i class='fa fa-binoculars'></i>" : "");
            string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
            string disasterUrlFriendlyName = String.IsNullOrEmpty(disaster.d.URLFriendlyName) ? "HurricaneIan" : disaster.d.URLFriendlyName;
            disasterDropDown = disasterDropDown + "<li name=\"" + disasterUrlFriendlyName + "\"><a href=\"#\">" + disasterSimulation + " " + disaster.d.Name + " - " + disasterDate + "</a></li>" + Environment.NewLine;
            idNumber = idNumber + 1;
        }
    }
}