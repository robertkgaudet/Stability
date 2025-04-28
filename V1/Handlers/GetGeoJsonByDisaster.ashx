<%@ WebHandler Language="C#" Class="GetGeoJsonByDisaster" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GetGeoJsonByDisaster: IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest (HttpContext context)
    {
        string keyId = context.Request.QueryString["keyId"];
        string mapFilterType = context.Request.QueryString["mapFilterType"];
        string locationTypeId = context.Request.QueryString["locationTypeId"];
        string parentTypeId = context.Request.QueryString["parentTypeId"];
        string statusId = context.Request.QueryString["statusId"];

        string results = string.Empty;
        try
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();     
            Guid? locTypeId = !string.IsNullOrEmpty(locationTypeId) ? new Guid(locationTypeId) : (Guid?)null;
            Guid? parTypeId = !string.IsNullOrEmpty(parentTypeId) ? new Guid(parentTypeId) : (Guid?)null;
            Guid? statId = !string.IsNullOrEmpty(statusId) ? new Guid(statusId) : (Guid?)null;

            switch (mapFilterType)
            {
                case "All":
                    var allLocationJson = dc.GetGeoJsonByDisaster(
                        new Guid(keyId),
                        locTypeId,
                        parTypeId,
                        statId);
                    results = ProcessJsonResults(allLocationJson);
                    break;
                case "Community":
                    var communityLocationJson = dc.MapStabilityLocations(
                        new Guid(keyId),
                        locTypeId,
                        parTypeId,
                        statId);
                    results = ProcessJsonResults(communityLocationJson);
                    break;                
                case "Critical":
                    var criticalLocationJson = dc.GetGeoJsonByDisaster(
                        new Guid(keyId),
                        locTypeId,
                        parTypeId,
                        statId);
                    results = ProcessJsonResults(criticalLocationJson);
                    break;
                case "VOAD":
                    var VOADLocationJson = dc.GetGeoJsonByDisaster(
                        new Guid(keyId),
                        locTypeId,
                        parTypeId,
                        statId);
                    results = ProcessJsonResults(VOADLocationJson);
                    break;
                case "Professional":
                    var professionalLocationJson = dc.GetGeoJsonByDisaster(
                        new Guid(keyId),
                        locTypeId,
                        parTypeId,
                        statId);
                    results = ProcessJsonResults(professionalLocationJson);
                    break;
                default:
                    var defaultLocationJson = dc.GetGeoJsonByDisaster(
                        new Guid(keyId),
                        locTypeId,
                        parTypeId,
                        statId);
                    results = ProcessJsonResults(defaultLocationJson);
                    break;
            }
        }
        catch (Exception ex)
        {
            results = "Error: " + ex.Message;
            // Log the full error details if needed
            // System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
        
        context.Response.ContentType = "application/json";
        context.Response.Write(results);
    }

    private string ProcessJsonResults(object jsonData)
    {
        string results = (new JavaScriptSerializer().Serialize(jsonData));
        // Clean up the JSON string (remove wrapper properties if needed)
        results = results.Remove(0, 13);  // Remove initial wrapper
        results = results.Remove(results.Length - 3, 3);  // Remove trailing wrapper
        results = results.Replace("\\", "");  // Remove escape characters
        return results;
    }

    public bool IsReusable {
        get {
            return false;
        }
    }
}