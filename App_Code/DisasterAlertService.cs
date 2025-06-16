using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public class DisasterAlertService
{
	private string GetCountyZoneFromLatLon(double latitude, double longitude)
	{
		string url = "https://api.weather.gov/points/" + latitude.ToString("0.####") + "," + longitude.ToString("0.####");
		var request = (HttpWebRequest)WebRequest.Create(url);
		request.UserAgent = "StabilityDisasterBot/1.0";

		using (var response = (HttpWebResponse)request.GetResponse())
		using (var reader = new StreamReader(response.GetResponseStream()))
		{
			string json = reader.ReadToEnd();
			JObject obj = JObject.Parse(json);
			string countyZoneUrl = (string)obj["properties"]["county"];

			if (!string.IsNullOrEmpty(countyZoneUrl))
			{
				return countyZoneUrl.Substring(countyZoneUrl.LastIndexOf("/") + 1);
			}
		}

		return null;
	}
	private List<JToken> GetNwsAlertsByZone(string zoneId)
	{
		var list = new List<JToken>();
		string url = "https://api.weather.gov/alerts/active?zone=" + zoneId;

		var request = (HttpWebRequest)WebRequest.Create(url);
		request.UserAgent = "StabilityDisasterBot/1.0";

		using (var response = (HttpWebResponse)request.GetResponse())
		using (var reader = new StreamReader(response.GetResponseStream()))
		{
			string json = reader.ReadToEnd();
			JObject obj = JObject.Parse(json);
			JArray features = (JArray)obj["features"];
			foreach (var f in features)
				list.Add(f);
		}

		return list;
	}

	public List<NWSCommunityDisasterAlertCard> GetDisasterCardsWithGuidance(double latitude, double longitude)
	{
		string zoneId = GetCountyZoneFromLatLon(latitude, longitude);
		var cards = new List<NWSCommunityDisasterAlertCard>();
		var gpt = new GPTDisasterAlertNarrarator();

		if (string.IsNullOrEmpty(zoneId))
			return cards;

		List<JToken> alerts = GetNwsAlertsByZone(zoneId);

		if (alerts.Count == 0)
		{
			string message = "✅ There are currently no immediate weather alerts for your area. " +
							 "Use this time to check on your emergency kit, review your household disaster plan, " +
							 "and consider joining or forming a local response team on Stability.org to be ready when it matters.";

			cards.Add(new NWSCommunityDisasterAlertCard
			{
				GptMessage = message,
				Event = "No Active Weather Alerts",
				Severity = "None",
				Certainty = "N/A",
				Urgency = "N/A",
				Description = "There are currently no active weather alerts in your area.",
				Headline = "Calm Conditions – Stay Ready",
				SenderName = "National Weather Service",
				AreaDesc = "Your Local Zone",
				Url = "https://www.weather.gov/",
				Sent = DateTime.UtcNow,
				Expires = DateTime.UtcNow.AddHours(6),
				Ends = DateTime.UtcNow.AddHours(6),
				Status = "None",
				MessageType = "None",
				Category = "General"
			});

			return cards;
		}

		foreach (var feature in alerts)
		{
			var props = feature["properties"];
			var card = new NWSCommunityDisasterAlertCard
			{
				Event = (string)props["event"],
				Severity = (string)props["severity"],
				Certainty = (string)props["certainty"],
				Urgency = (string)props["urgency"],
				Description = (string)props["description"],
				Headline = (string)props["headline"],
				SenderName = (string)props["senderName"],
				AreaDesc = (string)props["areaDesc"],
				Url = (string)props["uri"],
				Sent = ParseDate((string)props["sent"]),
				Expires = ParseDate((string)props["expires"]),
				Ends = ParseDate((string)props["ends"]),
				Status = (string)props["status"],
				MessageType = (string)props["messageType"],
				Category = (string)props["category"]
			};

			string prompt = BuildPromptFromCard(card);
			card.GptMessage = gpt.GetGptResponse(prompt);

			cards.Add(card);
		}

		return cards;
	}





	public List<NWSCommunityDisasterAlertCard> GetDisasterCardsWithGuidance(string stateCode)
	{
		var cards = new List<NWSCommunityDisasterAlertCard>();
		string url = "https://api.weather.gov/alerts/active?area=" + stateCode.ToUpper();

		var request = (HttpWebRequest)WebRequest.Create(url);
		request.UserAgent = "StabilityDisasterBot/1.0";

		var response = (HttpWebResponse)request.GetResponse();
		using (var reader = new StreamReader(response.GetResponseStream()))
		{
			string json = reader.ReadToEnd();
			JObject obj = JObject.Parse(json);
			JArray features = (JArray)obj["features"];

			foreach (JToken feature in features)
			{
				var props = feature["properties"];
				var card = new NWSCommunityDisasterAlertCard
				{
					Event = (string)props["event"],
					Severity = (string)props["severity"],
					Certainty = (string)props["certainty"],
					Urgency = (string)props["urgency"],
					Description = (string)props["description"],
					Headline = (string)props["headline"],
					SenderName = (string)props["senderName"],
					AreaDesc = (string)props["areaDesc"],
					Url = (string)props["uri"],
					Sent = ParseDate((string)props["sent"]),
					Expires = ParseDate((string)props["expires"]),
					Ends = ParseDate((string)props["ends"]),
					Status = (string)props["status"],
					MessageType = (string)props["messageType"],
					Category = (string)props["category"]
				};

				string prompt = BuildPromptFromCard(card);
				var gpt = new GPTDisasterAlertNarrarator();
				card.GptMessage = gpt.GetGptResponse(prompt);

				cards.Add(card);
			}
		}

		return cards;
	}
	private DateTime ParseDate(string dateStr)
	{
		DateTime result;
		if (!string.IsNullOrWhiteSpace(dateStr) &&
			DateTime.TryParse(dateStr, out result))
		{
			return result;
		}

		return DateTime.UtcNow; // fallback if parsing fails
	}

	private string BuildPromptFromCard(NWSCommunityDisasterAlertCard card)
	{
		var sb = new StringBuilder();
		sb.AppendLine("You are a helpful assistant writing disaster safety guidance.");
		sb.AppendLine("Here is the official alert:");
		sb.AppendLine("- Event: " + card.Event);
		sb.AppendLine("- Headline: " + card.Headline);
		sb.AppendLine("- Description: " + card.Description);
		sb.AppendLine("- Severity: " + card.Severity);
		sb.AppendLine("- Certainty: " + card.Certainty);
		sb.AppendLine("- Urgency: " + card.Urgency);
		sb.AppendLine("- Affected Areas: " + card.AreaDesc);
		sb.AppendLine("- Issued by: " + card.SenderName);
		sb.AppendLine();
		sb.AppendLine("Based on this, provide actionable guidance to community members reading this alert. Advise them how to prepare, follow local instructions, and how to help others through community support or Stability.org. Keep the tone urgent but calm, and use accessible language.");
		return sb.ToString();
	}

	private string BuildGptPrompt(List<JToken> alerts)
	{
		var sb = new StringBuilder();
		sb.AppendLine("Write a clear community update based on the following weather alerts. For each one, briefly summarize the situation and advise citizens on safety steps, following local guidance, and offering help via Stability.org.\n");

		for (int i = 0; i < alerts.Count; i++)
		{
			var props = alerts[i]["properties"];
			if (props == null) continue;

			sb.AppendLine("Alert #" + (i + 1));
			sb.AppendLine("- Event: " + (string)props["event"]);
			sb.AppendLine("- Severity: " + (string)props["severity"]);
			sb.AppendLine("- Certainty: " + (string)props["certainty"]);
			sb.AppendLine("- Urgency: " + (string)props["urgency"]);
			sb.AppendLine("- Issued by: " + (string)props["senderName"]);
			sb.AppendLine("- Affected Areas: " + (string)props["areaDesc"]);
			sb.AppendLine("- Instructions: " + (string)props["instruction"]);
			sb.AppendLine("- Description: " + (string)props["description"]);
			sb.AppendLine();
		}

		sb.AppendLine("Create a single message that the community can understand and act on calmly. Focus on clarity, safety, and encouragement to help others.");
		return sb.ToString();
	}

	private string CallGpt(string prompt)
	{
		var gpt = new GPTDisasterAlertNarrarator();
		return gpt.GetGptResponse(prompt);
	}
}
