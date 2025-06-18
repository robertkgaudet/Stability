<%@ WebHandler Language="C#" Class="CitySearchHandler" %>

using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

public class CitySearchHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
		string query = context.Request["q"] ?? "";

		// Remove punctuation except spaces
		string cleaned = new string(query.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

		// Normalize "Saint" and "St"
		string sanitized = ReplaceIgnoreCase(cleaned, "Saint", "St").Trim().ToLower();
    
        context.Response.ContentType = "application/json";

        using (var db = new CrowdReliefDBDataContext()) // Replace with your actual DataContext
        {
            var results = (from c in db.Cities
                           join s in db.USStates on c.Code equals s.Code
						   where (c.City1 + " " + s.Name + " " + s.Code).Contains(sanitized)
                           orderby c.City1
                           select new
                           {
                               id = c.City1 + ", " + s.Code, // Select2's internal ID
                               text = c.City1 + ", " + s.Name, // Displayed text
                               city = c.City1,
                               state = s.Name
                           }).Take(10).ToList();


            var serializer = new JavaScriptSerializer();
            context.Response.Write(serializer.Serialize(results));
        }
    }
	public static string ReplaceIgnoreCase(string input, string search, string replacement)
	{
		return System.Text.RegularExpressions.Regex.Replace(input, 
			System.Text.RegularExpressions.Regex.Escape(search), 
			replacement.Replace("$", "$$"), 
			System.Text.RegularExpressions.RegexOptions.IgnoreCase);
	}
    public bool IsReusable
	{
		get { return false; }
	}
}