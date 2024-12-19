<%@ WebHandler Language="C#" Class="GetGeoJsonByDisaster" %>

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
public class GetGeoJsonByDisaster: IHttpHandler, IReadOnlySessionState
{
	public void ProcessRequest (HttpContext context)
	{
		string keyId = context.Request.QueryString["keyId"];
		string mapFilterType = context.Request.QueryString["mapFilterType"];
		string results = string.Empty;
		try
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			switch (mapFilterType)
			{
				case "All":
					var allLocationJson = dc.GetGeoJsonByDisaster(new Guid(keyId));

					results = (new JavaScriptSerializer().Serialize(allLocationJson));
					results = results.Remove(0, 13);
					results = results.Remove(results.Length - 3, 3);
					results = results.Replace("\\", "");
                break;

				case "Community":
					var communityLocationJson = dc.MapStabilityLocations(new Guid(keyId));

					results = (new JavaScriptSerializer().Serialize(communityLocationJson));
					results = results.Remove(0, 13);
					results = results.Remove(results.Length - 3, 3);
					results = results.Replace("\\", "");
                break;
				

				case "Critical":
					var criticalLocationJson = dc.GetGeoJsonByDisaster(new Guid(keyId));

					results = (new JavaScriptSerializer().Serialize(criticalLocationJson));
					results = results.Remove(0, 13);
					results = results.Remove(results.Length - 3, 3);
					results = results.Replace("\\", "");
                break;
				

				case "VOAD":
					var VOADLocationJson = dc.GetGeoJsonByDisaster(new Guid(keyId));

					results = (new JavaScriptSerializer().Serialize(VOADLocationJson));
					results = results.Remove(0, 13);
					results = results.Remove(results.Length - 3, 3);
					results = results.Replace("\\", "");
                break;
				

				case "Professional":
					var professionalLocationJson = dc.GetGeoJsonByDisaster(new Guid(keyId));

					results = (new JavaScriptSerializer().Serialize(professionalLocationJson));
					results = results.Remove(0, 13);
					results = results.Remove(results.Length - 3, 3);
					results = results.Replace("\\", "");
                break;


				default:
					var defaultLocationJson = dc.GetGeoJsonByDisaster(new Guid(keyId));

					results = (new JavaScriptSerializer().Serialize(defaultLocationJson));
					results = results.Remove(0, 13);
					results = results.Remove(results.Length - 3, 3);
					results = results.Replace("\\", "");
                break;
			}
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