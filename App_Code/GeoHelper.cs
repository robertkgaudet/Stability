using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

public static class GeoHelper
{
	public class GeoResult
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Zip { get; set; }
	}

	public static GeoResult GetLatLonZipFromCityState(string city, string state)
	{
		using (var client = new HttpClient())
		{
			client.DefaultRequestHeaders.Add("User-Agent", "Stability/1.0 (robgaudet@Stability.org)");

			string url = string.Format(
			"https://nominatim.openstreetmap.org/search?q=city+hall+{0}+{1}&format=json&addressdetails=1&limit=1",
			Uri.EscapeDataString(city),
			Uri.EscapeDataString(state));

			HttpResponseMessage response = client.GetAsync(url).Result;
			if (!response.IsSuccessStatusCode)
				return null;

			string json = response.Content.ReadAsStringAsync().Result;
			JArray results = JArray.Parse(json);

			if (results.Count == 0)
				return null;

			JObject result = (JObject)results[0];
			double lat = double.Parse(result["lat"].ToString());
			double lon = double.Parse(result["lon"].ToString());

			string zip = "";
			JObject address = (JObject)result["address"];
			if (address != null && address["postcode"] != null)
			{
				zip = address["postcode"].ToString();
			}

			return new GeoResult
			{
				Latitude = lat,
				Longitude = lon,
				Zip = zip
			};
		}
	}

}