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
        string locationTypeIdStr = context.Request.QueryString["locationType"];
        string parentTypeIdStr = context.Request.QueryString["parentType"];
        string statusIdStr = context.Request.QueryString["status"];
        string results = string.Empty;
        try
        {
            Guid disasterGuid;
            if (!Guid.TryParse(disasterId, out disasterGuid))
            {
                throw new ArgumentException("Invalid or missing Disaster ID.");
            }
            Guid? locationTypeId = TryParseGuid(locationTypeIdStr);
            Guid? parentTypeId = TryParseGuid(parentTypeIdStr);
            Guid? statusId = TryParseGuid(statusIdStr);
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                results = GetFilteredGeoJson(dc, disasterGuid, mapFilterType, locationTypeId, parentTypeId, statusId);
            }
        }
        catch (Exception ex)
        {
            results = new JavaScriptSerializer().Serialize(new { error = ex.Message });
        }

        context.Response.ContentType = "application/json";
        context.Response.Write(results);
    }

    private string GetFilteredGeoJson(CrowdReliefDBDataContext dc, Guid disasterGuid, string mapFilterType, Guid? locationTypeId, Guid? parentTypeId, Guid? statusId)
    {
        object data = null;

        switch (mapFilterType)
        {
            case "All":
                data = dc.GetGeoJsonByDisaster(disasterGuid, locationTypeId, parentTypeId, statusId);
                break;
            case "Community":
                data = dc.MapStabilityLocations(disasterGuid, locationTypeId, parentTypeId, statusId);
                break;

            case "Critical":
                data = dc.MapStabilityLocations(disasterGuid, locationTypeId, parentTypeId, statusId);
                break;

            case "VOAD":
            case "Professional":
            default:
                data = dc.GetGeoJsonByDisaster(disasterGuid, locationTypeId, parentTypeId, statusId);
                break;
        }
        return SerializeResults(data);
    }
    private string SerializeResults(object data)
    {
        return new JavaScriptSerializer().Serialize(data);
    }
    private Guid? TryParseGuid(string guidStr)
    {
        Guid result;
        return Guid.TryParse(guidStr, out result) ? (Guid?)result : null;
    }
    public bool IsReusable
    {
        get { return false; }
    }
}
