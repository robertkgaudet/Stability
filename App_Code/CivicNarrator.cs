using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class CivicNarrator
{
	private static string OpenAIApiKey = ConfigurationManager.AppSettings["OpenAIKey"].ToString();
	private const string OpenAIEndpoint = "https://api.openai.com/v1/chat/completions";


	public static Dictionary<string, string> CommunityDemonyms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{ "Boston", "Bostonians" },
		{ "New York", "New Yorkers" },
		{ "Chicago", "Chicagoans" },
		{ "Houston", "Houstonians" },
		{ "Dallas", "Dallasites" },
		{ "Los Angeles", "Angelenos" },
		{ "San Francisco", "San Franciscans" },
		{ "Lafayette", "Cajuns" },
		{ "New Orleans", "New Orleanians" },
		{ "Atlanta", "Atlantans" },
		{ "Philadelphia", "Philadelphians" },
		{ "Miami", "Miamians" },
		{ "Seattle", "Seattleites" },
		{ "Detroit", "Detroiters" },
		{ "Minneapolis", "Minneapolitans" },
		{ "St. Louis", "St. Louisans" },
		{ "Nashville", "Nashvillians" },
		{ "Phoenix", "Phoenicians" },
		{ "Portland", "Portlanders" },
		{ "San Antonio", "San Antonians" },
		{ "Honolulu", "Honolulans" }
	};

	public class WeatherInfo
	{
		public Guid WeatherInfoId { get; set; }
		public string City { get; set; }
		public float Temperature { get; set; }
		public string Condition { get; set; }
		public DateTime RetrievedAt { get; set; }
	}

	public class ElderlyDemographics
	{
		public string Name { get; set; }
		public int ElderlyCount { get; set; }
		public int TotalPopulation { get; set; }
		public double ElderlyPercentage { get; set; } // ← new
		public string ZCTA { get; set; }
	}

	private static string GetTimeOfDay()
	{
		int hour = DateTime.Now.Hour;
		if (hour < 12) return "Morning";
		if (hour < 17) return "Afternoon";
		return "Evening";
	}

	private static string GetSeason()
	{
		int month = DateTime.Now.Month;
		if (month >= 3 && month <= 5) return "Spring";
		if (month >= 6 && month <= 8) return "Summer";
		if (month >= 9 && month <= 11) return "Fall";
		return "Winter";
	}

	public static Task<string> GenerateWeatherAndDemographicsNarrativeAsync(WeatherInfo weather, ElderlyDemographics demographics, List<ForecastDay> forecast, string demonym)
	{
		string prompt = BuildWeatherAndDemographicsPrompt(weather, demographics, forecast, demonym);
		return CallChatGptWithPrompt(prompt);
	}

	public class ForecastDay
	{
		public string Day { get; set; }
		public string Temp { get; set; }
		public string Icon { get; set; }
		public string Condition { get; set; }
	}
	private static string BuildWeatherAndDemographicsPrompt(WeatherInfo weather, ElderlyDemographics demographics, List<ForecastDay> forecast, string demonym)
	{
		var now = DateTime.Now;
		string timeOfDay = now.Hour < 12 ? "morning" :
						   now.Hour < 18 ? "afternoon" : "evening";
		string season = GetSeason(now);
		string greeting = "Good " + timeOfDay + ".";
		string month = now.ToString("MMMM");

		var sb = new StringBuilder();

		sb.AppendLine("You are a friendly and thoughtful civic assistant, like a trusted neighbor or quiet butler.");
		sb.AppendLine("Your tone should be relaxed, warm, and human — without exclamation points.");
		sb.AppendLine();
		sb.AppendLine("Speak to the resident as if you're starting a small community check-in.");
		sb.AppendLine("Include cultural tone hints based on the local region (e.g., Southern Black, Midwest small town, etc.).");
		sb.AppendLine();
		sb.AppendLine("Start with a soft greeting like \"" + greeting + "\" — make it sound personal and welcoming, but get creative and change the verbiage to something similar with each request to keep it fresh.");
		sb.AppendLine("Mention the current weather and demographic info in just a few relaxed sentences.");
		sb.AppendLine("Then offer a short 5-day forecast in the voice of a calm local weatherman — grounded, non-dramatic, helpful.");
		sb.AppendLine("Close with something soft and human, like \"Might be a good time to check on neighbors.\"");
		sb.AppendLine();

		sb.AppendLine("Weather:");
		sb.AppendLine("City: " + weather.City);
		sb.AppendLine("Temperature: " + Math.Round(weather.Temperature) + "°F");
		sb.AppendLine("Condition: " + weather.Condition);
		sb.AppendLine();

		sb.AppendLine("Demographics:");
		sb.AppendLine("In " + demographics.Name + ", " +
			demographics.ElderlyCount.ToString("N0") + " out of " +
			demographics.TotalPopulation.ToString("N0") + " residents are aged 60 or older — " +
			demographics.ElderlyPercentage.ToString("0.0") + "% of the population.");

		sb.AppendLine();
		sb.AppendLine("Five-Day Forecast:");
		foreach (var f in forecast)
		{
			sb.AppendLine(f.Day + ": " + f.Temp + "°F, " + f.Condition);
		}

		//sb.AppendLine();
		//sb.AppendLine("Now use this information to write a friendly, short summary.");
		//sb.AppendLine("Begin with the greeting and end with a gentle, encouraging phrase.");
		//sb.AppendLine("Make the whole response no more than 5 sentences.");
		//sb.AppendLine("Don't be overly expressive. Keep the mood neighborly, steady, and kind.");
		sb.AppendLine();
		if (!string.IsNullOrEmpty(demonym))
		{
			sb.AppendLine("Refer to residents as \"" + demonym + "\" in a natural way.");
		}
		else
		{
			sb.AppendLine("If the area has a well-known cultural identity or demonym (e.g. Cajuns, Bostonians, Houstonians), refer to them naturally.");
		}

		sb.AppendLine("Now write a short civic-minded summary using this information.");
		sb.AppendLine("Use only the city or community name (never the ZIP code).");
		sb.AppendLine("Tone: warm, humble, calm — like a neighborhood butler or local weatherman.");
		sb.AppendLine("Avoid exclamation points. Keep it under 5 sentences.");
		sb.AppendLine("Begin with a personal greeting (e.g., 'Good afternoon, neighbors in Boston').");
		sb.AppendLine("Close with something gently caring like 'Might be a good time to check on neighbors.'");

		return sb.ToString();
	}

	private static string GetSeason(DateTime date)
	{
		int dayOfYear = date.DayOfYear;

		if (dayOfYear < 80 || dayOfYear > 355) return "winter";
		if (dayOfYear < 172) return "spring";
		if (dayOfYear < 265) return "summer";
		return "autumn";
	}


	private static Task<string> CallChatGptWithPrompt(string prompt)
	{
		var tcs = new TaskCompletionSource<string>();

		try
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + OpenAIApiKey);

			var requestData = new
			{
				model = "gpt-3.5-turbo",
				messages = new object[]
				{
					new { role = "system", content = "You are a civic-focused assistant writing friendly updates." },
					new { role = "user", content = prompt }
				},
				temperature = 0.7
			};

			string json = JsonConvert.SerializeObject(requestData);
			HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

			client.PostAsync(OpenAIEndpoint, content).ContinueWith(responseTask =>
			{
				if (responseTask.IsCompleted && !responseTask.IsFaulted)
				{
					HttpResponseMessage response = responseTask.Result;

					response.Content.ReadAsStringAsync().ContinueWith(readTask =>
					{
						if (readTask.IsCompleted && !readTask.IsFaulted)
						{
							string resultJson = readTask.Result;
							dynamic result = JsonConvert.DeserializeObject(resultJson);
							string text = result.choices[0].message.content.ToString();
							tcs.SetResult(text);
						}
						else
						{
							tcs.SetResult("Unable to read GPT response.");
						}
					});
				}
				else
				{
					tcs.SetResult("Unable to contact GPT service.");
				}
			});
		}
		catch (Exception ex)
		{
			tcs.SetResult("An error occurred: " + ex.Message);
		}

		return tcs.Task;
	}
}