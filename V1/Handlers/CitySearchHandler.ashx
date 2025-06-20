<%@ WebHandler Language="C#" Class="CitySearchHandler" %>

using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

public class CitySearchHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string query = context.Request["q"] ?? "";

        context.Response.ContentType = "application/json";

        using (var db = new CrowdReliefDBDataContext())
        {
            var cities = (from c in db.Cities
                          join s in db.USStates on c.Code equals s.Code
                          select new
                          {
                              City = c.City1,
                              State = s.Name,
                              StateCode = s.Code
                          }).ToList(); // pull into memory

            // Normalize query
            string cleaned = new string(query.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
            string normalized = ReplaceIgnoreCase(cleaned, "Saint", "St").Trim().ToLower();
            var words = normalized.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			var results = cities
				.Select(x =>
				{
					string cityName = ReplaceIgnoreCase(x.City, "Saint", "St").ToLower();
					string stateName = x.State.ToLower();
					string stateCode = x.StateCode.ToLower();

					bool allWordsMatch = words.All(w =>
						cityName.Contains(w) || stateName.Contains(w) || stateCode.Contains(w));

					int score = 0;

					// Add relevance score
					if (cityName == normalized) score += 100;
					if (stateName == normalized || stateCode == normalized) score += 100;

					if (words.Length == 2)
					{
						if (cityName == words[0] && (stateName == words[1] || stateCode == words[1]))
							score += 200; // perfect match: "new york" "ny"
						else if (cityName == words[1] && (stateName == words[0] || stateCode == words[0]))
							score += 200; // "ny new york"
					}

					return new
					{
						id = x.City + ", " + x.StateCode,
						text = x.City + ", " + x.State,
						city = x.City,
						state = x.State,
						score = allWordsMatch ? score : -1
					};
				})
				.Where(x => x.score >= 0)
				.OrderByDescending(x => x.score)
				.ThenBy(x => x.city)
				.Take(10)
				.ToList();


            var serializer = new JavaScriptSerializer();
            context.Response.Write(serializer.Serialize(results));
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }

    // ✅ This must be outside ProcessRequest, inside the class
    public static string ReplaceIgnoreCase(string input, string search, string replacement)
    {
        return Regex.Replace(
            input,
            Regex.Escape(search),
            replacement.Replace("$", "$$"),
            RegexOptions.IgnoreCase);
    }
}
