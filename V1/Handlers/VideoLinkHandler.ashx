<%@ WebHandler Language="C#" Class="VideoLinkHandler" %>
using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

	public class VideoLinkHandler : IHttpHandler

	{public void ProcessRequest(HttpContext context)
	{
		string method = context.Request.HttpMethod;
        string programId = context.Request["programId"];
        using (var db = new CrowdReliefDBDataContext()) // Replace with your DataContext name.
        {

			if (method == "POST" && context.Request["method"] == "DELETE")
			{
				HandleDeleteRequest(context, db, programId);
			}
			else if (method == "GET")
			{
				HandleGetRequest(context, db, programId);
			}
			else if (method == "POST")
			{
				HandlePostRequest(context, db, programId);
			}
			else
			{
				context.Response.StatusCode = 405; // Method Not Allowed
				context.Response.Write("Method not allowed.");
			}
		}
	}

    private void HandleGetRequest(HttpContext context, CrowdReliefDBDataContext db, string programId)
    {
        var links = db.ProgramVideoLinks
            .Where(pvl => pvl.ProgramId == new Guid(programId))
            .Select(pvl => new
            {
                pvl.VideoLink.VideoLinkId,
                pvl.VideoLink.URL,
				Title = pvl.VideoLink.Title
            }).ToList();

        var serializer = new JavaScriptSerializer();
        string json = serializer.Serialize(links);

        context.Response.ContentType = "application/json";
        context.Response.Write(json);
    }

    private void HandlePostRequest(HttpContext context, CrowdReliefDBDataContext db, string programId)
    {
        string url = context.Request["url"];
        string title = context.Request["title"];
		if (string.IsNullOrEmpty(title))
		{
			title = "Untitled"; // Default title if none is provided
		}
        if (string.IsNullOrEmpty(url))
        {
            context.Response.StatusCode = 400; // Bad Request
            context.Response.Write("URL cannot be empty.");
            return;
        }
		
        var videoLink = new VideoLink { Title = title, URL = url, CreatedOn = DateTime.Now, VideoLinkId = Guid.NewGuid(), ViewCount = 0, IsActive = true };
        db.VideoLinks.InsertOnSubmit(videoLink);
        db.SubmitChanges();

        var programVideoLink = new ProgramVideoLink
        {
            ProgramId = new Guid(programId),
            VideoLinkId = videoLink.VideoLinkId,
			IsActive = true,
			ViewCount = 0,
			ProgramVideoLinkId = Guid.NewGuid()
        };

        db.ProgramVideoLinks.InsertOnSubmit(programVideoLink);
        db.SubmitChanges();

        var serializer = new JavaScriptSerializer();
        string json = serializer.Serialize(new
        {
            VideoLinkId = videoLink.VideoLinkId,
            URL = videoLink.URL,
			Title = videoLink.Title
        });

        context.Response.ContentType = "application/json";
        context.Response.Write(json);
    }

    private void HandleDeleteRequest(HttpContext context, CrowdReliefDBDataContext db, string programId)
    {
        string videoLinkId = context.Request["videoLinkId"];

        var videoLink = db.VideoLinks.SingleOrDefault(vl => vl.VideoLinkId == new Guid(videoLinkId));
        var programVideoLink = db.ProgramVideoLinks.SingleOrDefault(pvl =>
            pvl.ProgramId ==new Guid(programId)  && pvl.VideoLinkId == new Guid(videoLinkId));

        if (videoLink != null && programVideoLink != null)
        {
            db.ProgramVideoLinks.DeleteOnSubmit(programVideoLink);
            db.VideoLinks.DeleteOnSubmit(videoLink);
            db.SubmitChanges();
        }

        context.Response.StatusCode = 200; // Success
    }

    public bool IsReusable
    {
        get { return false; }
    }
}