using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

public class LinkPreviewController : ApiController
{
	private readonly LinkPreviewService _linkPreviewService = new LinkPreviewService();

	[HttpPost]
	[Route("api/previewlink")]
	public Task<IHttpActionResult> PreviewLink([FromBody] string url)
	{
		var task = _linkPreviewService.ExtractTextAndImageFromLink(url);

		return task.ContinueWith<IHttpActionResult>(t =>
		{
			var result = t.Result;
			return Ok(new { title = result.Item1, description = result.Item2, imageUrl = result.Item3 });
		});
	}
}
