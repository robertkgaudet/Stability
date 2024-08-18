using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class LinkPreviewService
{
	public Task<Tuple<string, string, string>> ExtractTextAndImageFromLink(string url)
	{
		var tcs = new TaskCompletionSource<Tuple<string, string, string>>();

		try
		{
			HttpClient client = new HttpClient();
			client.GetStringAsync(url).ContinueWith(responseTask =>
			{
				if (responseTask.IsFaulted)
				{
					string exMessage = responseTask.Exception.Message;
					tcs.SetResult(Tuple.Create(string.Empty, string.Empty, string.Empty));
					return;
				}

				var response = responseTask.Result;
				var document = new HtmlDocument();
				document.LoadHtml(response);

				// Extract meta description
				var metaDescription = string.Empty;

				//metaDescription = document.DocumentNode.SelectSingleNode("//meta[@name='description' or @property='og:description']") .GetAttributeValue("content", string.Empty);

				var descriptionNode = document.DocumentNode.SelectSingleNode("//meta[@name='description' or @property='og:description']");
				if (descriptionNode != null)
				{
					metaDescription = descriptionNode.GetAttributeValue("content", string.Empty);
				}

				// Fallback to first paragraph if no meta description
				if (string.IsNullOrEmpty(metaDescription))
				{
					if (document.DocumentNode.SelectSingleNode("//p") != null)
					{
						metaDescription = document.DocumentNode.InnerText ?? string.Empty;
					}
				}

				// Extract title
				string title = null;
				var titleNode = document.DocumentNode.SelectSingleNode("//title");
				if (titleNode != null)
				{
					title = titleNode.InnerText.Trim();
				}


				var imageUrl = string.Empty;

				var imageNode = document.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
				if (imageNode != null)
				{
					imageUrl = imageNode.GetAttributeValue("content", string.Empty);
				}

				if (String.IsNullOrEmpty(imageUrl))
				{
					imageUrl = document.DocumentNode.GetAttributeValue("src", string.Empty);
				}

				// Ensure the image URL is absolute if necessary
				if (!string.IsNullOrEmpty(imageUrl) && !Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
				{
					var baseUri = new Uri(url);
					imageUrl = new Uri(baseUri, imageUrl).ToString();
				}

				tcs.SetResult(Tuple.Create(title, metaDescription, imageUrl));
			});
		}
		catch (Exception ex)
		{
			string exMessage = ex.Message;
			tcs.SetResult(Tuple.Create(string.Empty, string.Empty, string.Empty));
		}

		return tcs.Task;
	}
}
