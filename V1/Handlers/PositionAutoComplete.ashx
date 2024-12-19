<%@ WebHandler Language="C#" Class="PositionAutoComplete" %>

using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.SessionState;
using System.Collections.Generic;
using Newtonsoft.Json;


[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class PositionAutoComplete : IHttpHandler, IReadOnlySessionState
{

	CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

    public void ProcessRequest(HttpContext context)
    {
       context.Response.ContentType = "application/json";

        string method = context.Request["method"];
        string userId = context.Request["userId"];

        if (method == "GetSuggestions")
        {
            GetSuggestions(context, userId);
        }
        else if (method == "SaveNewValues")
        {
            SaveNewValues(context, userId);
        }
    }

    private void GetSuggestions(HttpContext context, string userId)
    {
        string term = context.Request["term"];
        var suggestions = dc.Positions
            .Where(p => p.Name.Contains(term))
            .OrderBy(p => p.Name)
            .Select(p => p.Name)
            .ToList();

        string json = JsonConvert.SerializeObject(suggestions);
        context.Response.ContentType = "application/json";
		context.Response.Write(json);
    }
			
	private void SaveNewValues(HttpContext context, string userId)
    {
        string newValuesJson = context.Request["newValues"];
		//string userId = context.Request["userId"];
        List<string> newValues = new JavaScriptSerializer().Deserialize<List<string>>(newValuesJson);

        foreach (var newValue in newValues)
        {
            var existingPosition = dc.Positions.FirstOrDefault(p => p.Name == newValue);

            if (existingPosition == null)
            {
                Position newPosition = new Position
                {
                    Name = newValue,
					CreatedBy = new System.Guid(userId)
                };
                dc.Positions.InsertOnSubmit(newPosition);
            }
        }

        dc.SubmitChanges();
        context.Response.Write("{\"status\": \"success\"}");
    }

	public bool IsReusable
	{
		get { return false; }
	}
}