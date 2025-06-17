using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class StabilityCommunityFeed
{
	private static string OpenWeatherMapApiKey = ConfigurationManager.AppSettings["OpenWeatherMapApiKey"].ToString();
	private static string EventbriteToken = ConfigurationManager.AppSettings["EventbriteToken"].ToString();
	private static string GoogleAPI = ConfigurationManager.AppSettings["mapApiKey"].ToString();
	public class CommunitySnapshot
	{
		public WeatherInfo Weather { get; set; }
	}

	public class WeatherInfo
	{
		public Guid WeatherInfoId { get; set; }
		public string City { get; set; }
		public float Temperature { get; set; }
		public string Condition { get; set; }
		public DateTime RetrievedAt { get; set; }
	}
	public class ForecastDay
	{
		public string Day { get; set; }
		public string Temp { get; set; }
		public string Icon { get; set; }
		public string Condition { get; set; }
	}

	public static async Task<CommunitySnapshot> GetCommunitySnapshotAsync(string city)
	{
		CommunitySnapshot snapshot = new CommunitySnapshot();
		snapshot.Weather = await GetWeatherAsync(city);
		return snapshot;
	}

	private static async Task<WeatherInfo> GetWeatherAsync(string city)
	{
		string url = "https://api.openweathermap.org/data/2.5/weather?q=" + Uri.EscapeDataString(city) + "&appid=" + OpenWeatherMapApiKey + "&units=imperial";

		using (HttpClient client = new HttpClient())
		{
			HttpResponseMessage response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				string json = await response.Content.ReadAsStringAsync();
				dynamic data = JsonConvert.DeserializeObject(json);
				WeatherInfo weather = new WeatherInfo();
				weather.WeatherInfoId = Guid.NewGuid();
				weather.City = city;
				weather.Temperature = data.main.temp;
				weather.Condition = data.weather[0].main;
				weather.RetrievedAt = DateTime.Now;
				return weather;
			}
		}
		return null;
	}
	private static string GetWeatherIcon(string condition)
	{
		condition = condition.ToLower();
		if (condition.Contains("clear")) return "☀️";
		if (condition.Contains("cloud")) return "⛅";
		if (condition.Contains("rain")) return "🌧️";
		if (condition.Contains("thunder")) return "⛈️";
		if (condition.Contains("snow")) return "❄️";
		if (condition.Contains("fog") || condition.Contains("mist")) return "🌫️";
		return "🌡️";
	}


	public static async Task<List<ForecastDay>> GetFiveDayForecastAsync(string city)
	{
		List<ForecastDay> forecastList = new List<ForecastDay>();
		string url = "https://api.openweathermap.org/data/2.5/forecast?q=" + Uri.EscapeDataString(city) + "&appid=" + OpenWeatherMapApiKey + "&units=imperial";

		using (HttpClient client = new HttpClient())
		{
			HttpResponseMessage response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				string json = await response.Content.ReadAsStringAsync();
				dynamic data = JsonConvert.DeserializeObject(json);
				var items = data.list;

				Dictionary<string, ForecastDay> seen = new Dictionary<string, ForecastDay>();

				foreach (var item in items)
				{
					string dateText = item.dt_txt;
					DateTime date = DateTime.Parse(dateText);
					string day = date.ToString("ddd");

					if (!seen.ContainsKey(day))
					{
						string temp = Math.Round((double)item.main.temp).ToString();
						string condition = item.weather[0].main.ToString();
						string icon = GetWeatherIcon(condition);

						seen[day] = new ForecastDay
						{
							Day = day,
							Temp = temp,
							Icon = icon,
							Condition = condition
						};
					}

					if (seen.Count >= 5) break;
				}

				forecastList.AddRange(seen.Values);
			}
		}

		return forecastList;
	}


	public class ElderlyDemographics
	{
		public string Name { get; set; }
		public int ElderlyCount { get; set; }
		public int TotalPopulation { get; set; }
		public double ElderlyPercentage { get; set; } // ← new
		public string ZCTA { get; set; }
	}

	public static async Task<ElderlyDemographics> GetElderlyByZipAsync(string zip)
	{
		// Includes total population + 60+ age buckets
		string variables = string.Join(",", new string[]
		{
		"B01001_001E", // total population
        "B01001_020E", "B01001_021E", "B01001_022E", "B01001_023E", "B01001_024E", "B01001_025E", // Male 60+
        "B01001_044E", "B01001_045E", "B01001_046E", "B01001_047E", "B01001_048E", "B01001_049E"  // Female 60+
		});

		string url = "https://api.census.gov/data/2023/acs/acs5"
			+ "?get=NAME," + variables
			+ "&for=zip%20code%20tabulation%20area:" + Uri.EscapeDataString(zip);

		using (HttpClient client = new HttpClient())
		{
			string json = await client.GetStringAsync(url);
			var arr = JsonConvert.DeserializeObject<List<List<string>>>(json);

			if (arr.Count < 2)
				return null;

			var values = arr[1];

			int totalPopulation = 0;
			int elderlyTotal = 0;

			// Parse total population (index 1)
			int.TryParse(values[1], out totalPopulation);

			// Parse all 60+ buckets (index 2 to 13)
			for (int i = 2; i <= 13; i++)
			{
				int value;
				if (int.TryParse(values[i], out value))
				{
					elderlyTotal += value;
				}
			}

			double percentage = 0;
			if (totalPopulation > 0)
			{
				percentage = (double)elderlyTotal / totalPopulation * 100;
			}

			return new ElderlyDemographics
			{
				Name = values[0],
				TotalPopulation = totalPopulation,
				ElderlyCount = elderlyTotal,
				ElderlyPercentage = Math.Round(percentage, 2),
				ZCTA = values[values.Count - 1]
			};
		}
	}
}