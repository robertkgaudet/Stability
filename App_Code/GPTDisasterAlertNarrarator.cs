using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

public class GPTDisasterAlertNarrarator
{
	private static string openAIApiKey = ConfigurationManager.AppSettings["OpenAIKey"].ToString();
	private const string endpoint = "https://api.openai.com/v1/chat/completions";

	public string GetGptResponse(string prompt)
	{
		try
		{
			var request = (HttpWebRequest)WebRequest.Create(endpoint);
			request.Method = "POST";
			request.ContentType = "application/json";
			request.Headers.Add("Authorization", "Bearer " + openAIApiKey);

			var requestBody = new
			{
				model = "gpt-4",
				messages = new object[]
				{
					new { role = "system", content = "You are a helpful assistant writing disaster alert messages for the public." },
					new { role = "user", content = prompt }
				},
				temperature = 0.7
			};

			string json = JsonConvert.SerializeObject(requestBody);
			byte[] data = Encoding.UTF8.GetBytes(json);
			request.ContentLength = data.Length;

			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}

			string result = "";
			using (var response = (HttpWebResponse)request.GetResponse())
			{
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					result = reader.ReadToEnd();
				}
			}

			JObject parsed = JObject.Parse(result);
			return (string)parsed["choices"][0]["message"]["content"];
		}
		catch (WebException webEx)
		{
			string errorMessage = "Network error: Unable to retrieve GPT response.";
			try
			{
				using (var reader = new StreamReader(webEx.Response.GetResponseStream()))
				{
					string errorDetail = reader.ReadToEnd();
					errorMessage += " Details: " + errorDetail;
				}
			}
			catch { }

			return errorMessage;
		}
		catch (Exception ex)
		{
			return "An error occurred while generating the disaster alert message: " + ex.Message;
		}
	}
}
