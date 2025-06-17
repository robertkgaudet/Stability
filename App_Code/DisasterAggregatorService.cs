using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using Newtonsoft.Json.Linq;
public class DisasterEvent
{
	public string Source { get; set; }
	public string Title { get; set; }
	public string Summary { get; set; }
	public string Url { get; set; }
	public string Location { get; set; }
	public DateTime Date { get; set; }
}

public class DisasterAggregatorService
{

	private static DateTime _lastFetch = DateTime.MinValue;
	private static List<DisasterEvent> _cachedData = new List<DisasterEvent>();
	private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

	public List<DisasterEvent> GetLatestDisasters(string stateCode)
	{
		var events = new List<DisasterEvent>();


		//NWS
		try { events.AddRange(GetNwsAlerts(stateCode)); } 
		catch { }


		//// Optional: include global alerts
		//try
		//{
		//	var gdacs = GetGdacsEvents();
		//	foreach (var e in gdacs)
		//	{
		//		e.Location += " (Global)";
		//		events.Add(e);
		//	}
		//}
		//catch { }


		//// Optional: include NASA EONET global events
		//try
		//{
		//	var eonet = GetNasaEonetEvents();
		//	foreach (var e in eonet)
		//	{
		//		e.Location += " (Satellite-tracked)";
		//		events.Add(e);
		//	}
		//}
		//catch { }

		return events;
	}



	private List<DisasterEvent> GetGdacsEvents()
	{
		var list = new List<DisasterEvent>();
		var url = "https://www.gdacs.org/xml/rss.xml";

		var request = (HttpWebRequest)WebRequest.Create(url);
		var response = (HttpWebResponse)request.GetResponse();

		using (var stream = response.GetResponseStream())
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(stream);
			var items = xmlDoc.GetElementsByTagName("item");

			foreach (XmlNode item in items)
			{
				string lat = "";
				string lon = "";

				var geoLat = item["geo:lat"];
				var geoLong = item["geo:long"];

				if (geoLat != null) lat = geoLat.InnerText;
				if (geoLong != null) lon = geoLong.InnerText;

				var evt = new DisasterEvent
				{
					Source = "GDACS",
					Title = item["title"] != null ? item["title"].InnerText : "",
					Summary = item["description"] != null ? item["description"].InnerText : "",
					Url = item["link"] != null ? item["link"].InnerText : "",
					Location = lat + "," + lon,
					Date = item["pubDate"] != null ? ParseDate(item["pubDate"].InnerText) : DateTime.UtcNow
				};
				list.Add(evt);
			}
		}

		return list;
	}

	private List<DisasterEvent> GetNasaEonetEvents()
	{
		var list = new List<DisasterEvent>();
		var url = "https://eonet.gsfc.nasa.gov/api/v3/events";

		var request = (HttpWebRequest)WebRequest.Create(url);
		var response = (HttpWebResponse)request.GetResponse();

		using (var reader = new StreamReader(response.GetResponseStream()))
		{
			string json = reader.ReadToEnd();
			JObject obj = JObject.Parse(json);
			JArray events = (JArray)obj["events"];

			foreach (JToken e in events)
			{
				string title = (string)e["title"];
				string description = e["description"] != null ? (string)e["description"] : "";
				string urlLink = "";
				string coordinates = "";
				DateTime date = DateTime.UtcNow;

				JArray sources = e["sources"] as JArray;
				if (sources != null && sources.Count > 0)
				{
					urlLink = (string)sources[0]["url"];
				}

				JArray geometry = e["geometry"] as JArray;
				if (geometry != null && geometry.Count > 0)
				{
					var coords = geometry[0]["coordinates"];
					coordinates = coords != null ? coords.ToString() : "";

					string dateStr = (string)geometry[0]["date"];
					date = ParseDate(dateStr);
				}

				list.Add(new DisasterEvent
				{
					Source = "NASA EONET",
					Title = title,
					Summary = description,
					Url = urlLink,
					Location = coordinates,
					Date = date
				});
			}
		}

		return list;
	}

	private List<DisasterEvent> GetNwsAlerts(string stateCode)
	{
		var list = new List<DisasterEvent>();
		string url = "https://api.weather.gov/alerts/active?area=" + stateCode.ToUpper();

		var request = (HttpWebRequest)WebRequest.Create(url);
		request.UserAgent = "StabilityDisasterBot/1.0";

		var response = (HttpWebResponse)request.GetResponse();
		using (var reader = new StreamReader(response.GetResponseStream()))
		{
			string json = reader.ReadToEnd();
			JObject obj = JObject.Parse(json);
			JArray features = (JArray)obj["features"];

			foreach (JToken f in features)
			{
				var props = f["properties"];
				list.Add(new DisasterEvent
				{
					Source = "NWS",
					Title = (string)props["event"],
					Summary = (string)props["description"],
					Url = (string)props["uri"],
					Location = (string)props["areaDesc"],
					Date = ParseDate((string)props["sent"])
				});
			}
		}

		return list;
	}


	private DateTime ParseDate(string dateStr)
	{
		DateTime dt;
		if (DateTime.TryParse(dateStr, out dt))
			return dt;
		return DateTime.UtcNow;
	}
}
