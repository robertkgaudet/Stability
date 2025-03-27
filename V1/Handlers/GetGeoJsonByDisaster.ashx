<%@ WebHandler Language="C#" Class="GetGeoJsonByDisaster" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GetGeoJsonByDisaster : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string disasterId = context.Request.QueryString["keyId"];
        string mapFilterType = context.Request.QueryString["mapFilterType"];
        string locationType = context.Request.QueryString["locationType"];
        string parentType = context.Request.QueryString["parentType"];
        string status = context.Request.QueryString["status"];
        string results = string.Empty;

        try
        {
            // Validate disasterId
            if (string.IsNullOrEmpty(disasterId))
            {
                throw new ArgumentException("Disaster ID is required");
            }

            Guid disasterGuid;
            if (!Guid.TryParse(disasterId, out disasterGuid))
            {
                throw new ArgumentException("Invalid Disaster ID format");
            }

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            // Default values if filters are not provided
            if (string.IsNullOrEmpty(locationType)) locationType = null;
            if (string.IsNullOrEmpty(parentType)) parentType = null;
            if (string.IsNullOrEmpty(status)) status = null;

            switch (mapFilterType)
            {
                case "All":
                    var allLocationJson = dc.GetGeoJsonByDisaster(disasterGuid, locationType, parentType, status);
                    results = SerializeResults(allLocationJson);
                    break;

                case "Community":
                    var communityLocationJson = dc.MapStabilityLocations(disasterGuid);
                    results = SerializeResults(communityLocationJson);
                    break;

                case "Critical":
                    var criticalLocationJson = dc.GetGeoJsonByDisaster(disasterGuid, locationType, parentType, status);
                    results = SerializeResults(criticalLocationJson);
                    break;

                case "VOAD":
                    var VOADLocationJson = dc.GetGeoJsonByDisaster(disasterGuid, locationType, parentType, status);
                    results = SerializeResults(VOADLocationJson);
                    break;

                case "Professional":
                    var professionalLocationJson = dc.GetGeoJsonByDisaster(disasterGuid, locationType, parentType, status);
                    results = SerializeResults(professionalLocationJson);
                    break;

                default:
                    var defaultLocationJson = dc.GetGeoJsonByDisaster(disasterGuid, locationType, parentType, status);
                    results = SerializeResults(defaultLocationJson);
                    break;
            }
        }
        catch (Exception ex)
        {
            results = "Error: " + ex.Message;
        }

        context.Response.ContentType = "application/json";
        context.Response.Write(results);
    }

    private string SerializeResults(object data)
    {
        string json = new JavaScriptSerializer().Serialize(data);
        // Remove the wrapper if present (adjust based on your actual data structure)
        if (json.StartsWith("{\"d\":") && json.EndsWith("}"))
        {
            json = json.Substring(5, json.Length - 6);
        }
        return json;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}