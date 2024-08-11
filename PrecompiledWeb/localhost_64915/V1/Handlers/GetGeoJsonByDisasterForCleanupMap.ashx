<%@ WebHandler Language="C#" Class="GetGeoJsonByDisasterForCleanupMap" %>

using System;
using System.Web;
using System.Linq;
using System.Web.Services;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GetGeoJsonByDisasterForCleanupMap: IHttpHandler, IReadOnlySessionState
{
	public void ProcessRequest (HttpContext context)
	{
		string eventId = context.Request.QueryString["eventId"];
		string results = string.Empty;
		try
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var locationJson = dc.GetGeoJsonByDisasterForCleanupMap(new Guid(eventId));
			//var UserDetails = JsonConvert.DeserializeObject<string>(locationJson);

			results = (new JavaScriptSerializer().Serialize(locationJson));
			results = results.Remove(0, 13);
			results = results.Remove(results.Length - 3, 3);
			results = results.Replace("\\", "");
			//return results;
		}
		catch (Exception ex)
		{
			results += results + ex.Data + "<br> " + ex.HelpLink + "<br> " + ex.HResult + "<br> " + ex.InnerException + "<br> " + ex.Message + "<br> " + ex.Source + "<br>" + ex.StackTrace + "<br>" + ex.TargetSite;
		}
		context.Response.ContentType = "text/plain";
		context.Response.Write(results);
	}

	public bool IsReusable {
		get {
			return false;
		}
	}

}