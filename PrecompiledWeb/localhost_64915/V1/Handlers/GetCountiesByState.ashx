<%@ WebHandler Language="C#" Class="GetCountiesByState" %>

using System;
using System.Web;
using System.Linq;
using System.Web.Services;
using System.Web.SessionState;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GetCountiesByState : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string eventId = (string)context.Request.QueryString["eventId"];
        string stateId = (string)context.Request.QueryString["stateId"];
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var counties = from c in dc.Counties
                       join ec in dc.EventCounties on c.CountyId equals ec.CountyId
                        where ec.EventId == new Guid(eventId) 
                        && c.StateId == new Guid(stateId)
                        orderby c.Name
                        select new {text = c.Name, id = c.CountyId};

        var serializer = new JavaScriptSerializer();
        var json = serializer.Serialize(counties);

        //context.Response.ContentType = "text/plain";
        context.Response.ContentType = "application/json";
        context.Response.Write(json);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }
}