using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Web.Security;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Web.Services.Description;
using GoogleMapsAPI.Places;
using System.Security.Policy;
using System.Activities.Statements;
//using static System.Net.Mime.MediaTypeNames;

public partial class V1_Stream : BaseOrganizationWebForm
{
	public string recoveryStageId = ConfigurationManager.AppSettings["recoveryStageId"].ToString();
	public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
	public string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
	public string eventId = HttpContext.Current.Request.QueryString["eventId"];
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	protected void Page_Load(object sender, EventArgs e)
	{
		LoadPosts();
		BindDropdown();
		if (!this.IsPostBack)
		{
			postField.Visible = false;
			lblPostMessage.Text = "Sign in to post";
			if (User.Identity.IsAuthenticated)
			{
				postField.Visible = true;
				lblPostMessage.Text = "";
				lblPostMessage.Visible = false;


				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

				var profile = (from p in dc.Profiles
							   where p.UserId == userId
							   select p).SingleOrDefault();
				if (profile != null)
				{
					litFullName.Text = profile.Firstname + " " + profile.Lastname;
				}
			}

			bool testMode = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["brainTreeTestMode"].ToString());
			string testNonce = System.Configuration.ConfigurationManager.AppSettings["testNonce"].ToString();

			this.Master.PageTitle = "Stability Activity Feed";
			this.Master.PageDescription = "View posts and updates from your team, friends and the public.";
			this.Master.FbDescription = "View posts and updates from your team, friends and the public.";
			this.Master.FbSite_name = "Stability Activity Feed";
			this.Master.HideMasterCover = true;
		}
		else
		{
		}
	}

	public void BindDropdown()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		AudienceType.Items.Clear();
		var AudianceType = dc.AudienceTypes.ToList();
		foreach (var option in AudianceType)
		{
			ListItem item = new ListItem(option.Type.ToString(), option.AudienceTypeId.ToString());
			AudienceType.Items.Add(item);
		}
		var PostReactionTypes = dc.PostReactionTypes.OrderBy(f => f.OrderId).ToList();
		var div = new HtmlGenericControl("div");

		foreach (var item in PostReactionTypes)
		{
			HtmlGenericControl newDiv = new HtmlGenericControl("span");
			newDiv.InnerHtml = item.PostReactionSymbol;
			newDiv.Attributes["id"] = item.PostReactionTypeId.ToString();
			if (item.OrderId == 2)
			{
				newDiv.Attributes["class"] = "large-icon text-danger thanksReaction";
			}
			else if (item.OrderId == 3)
			{
				newDiv.Attributes["class"] = "large-icon bold-purple-star thanksReaction";
			}
			else
			{
				newDiv.Attributes["class"] = "large-icon thanksReaction";
			}
			newDiv.Attributes["data-toggle"] = "tooltip";
			newDiv.Attributes["data-placement"] = "top";
			newDiv.Attributes["title"] = item.PostReactionType1.ToString();
			div.Controls.Add(newDiv);
		}
		PostReactionTypesId.Controls.Add(div);
	}
	public static string GetTimeAgo(DateTime pastDate)
	{
		TimeSpan timeDifference = DateTime.Now - pastDate;
		double yearsDifference = timeDifference.TotalDays / 365.25;
		if (yearsDifference >= 1)
		{
			int years = (int)yearsDifference;
			return years + " y";
		}
		else if (timeDifference.TotalDays >= 1)
		{
			int days = (int)timeDifference.TotalDays;
			return days + " d";
		}
		else if (timeDifference.TotalHours >= 1)
		{
			int hours = (int)timeDifference.TotalHours;
			return hours + " h";
		}
		else if (timeDifference.TotalMinutes >= 1)
		{
			int minutes = (int)timeDifference.TotalMinutes;
			return minutes + " m";
		}
		else
		{
			return "Just Now";
		}
	}

	public static List<string> ExtractUrls(string input)
	{
		List<string> urls = new List<string>();
		string pattern = @"\b(?:https?|ftp)://(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}(?:/[^\s]*)?\b";

		Regex regex = new Regex(pattern);
		MatchCollection matches = regex.Matches(input);

		foreach (Match match in matches)
		{
			urls.Add(match.Value);
		}
		return urls;
	}
	private static List<Profile> ExtractTaggedUser(string commentText, List<Profile> _users)
	{
		List<Profile> taggedUsers = new List<Profile>();
		var regex = new Regex(@"@([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\s([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\b");
		foreach (Match match in regex.Matches(commentText))
		{
			var name = match.Groups[1].Value.Trim();
			var words = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			name = string.Join(" ", words.Take(2));
			var user = _users.FirstOrDefault(u => (u.Firstname + " " + u.Lastname).Trim().ToLower().Contains(name.ToLower()));
			if (user != null)
			{
				taggedUsers.Add(user);
			}
		}
		return taggedUsers;
	}

	private static string ExtractTaggedMessage(string commentText, List<Profile> _users)
	{
		var regex = new Regex(@"@([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\s([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\b");
		var matches = regex.Matches(commentText);

		
		string urlPattern = @"\b(?:https?|ftp)://(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}(?:/[^\s]*)?\b";
		Regex urlRegex = new Regex(urlPattern);
		MatchCollection matchesUrl = urlRegex.Matches(commentText);

		//List<string> urls = new List<string>();
		//foreach (Match match in matchesUrl)
		//{
		//	urls.Add(match.Value);
		//}

		foreach (Match match in matchesUrl)
		{
			string url = match.Value;
			string anchorTag = "<a target='_blank' href='"+url+ "'>"+url+"</a>";
			// Replace URL with the anchor tag in the comment text
			commentText = commentText.Replace(url, anchorTag);
		}

		string processedComment = regex.Replace(commentText, match =>
		{
			var html = "";
			string fullName = match.Groups[1].Value.Trim();
			var words = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (words.Length > 2)
			{
				fullName = string.Join(" ", words.Take(2));
			}

			var user = _users.FirstOrDefault(u => (u.Firstname + " " + u.Lastname).Trim().ToLower().Contains(fullName.ToLower()));
			if (user != null)
			{
				html = "<a target='_blank' href='/V1/Profile/Profile.aspx?userId=" + user.UserId + "'>" + user.Firstname + " " + user.Lastname + "</a>";
			}

			if (words.Length > 2)
			{
				html = html + " " + string.Join(" ", words.Skip(2));
			}

			return user != null ? html : match.Value;
		});
		return processedComment;
	}

	private static string ReplaceTaggedUsersWithLinks(string comment)
	{
		if (string.IsNullOrEmpty(comment)) return comment;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var regex = new Regex(@"@([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\s([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\b");

		return regex.Replace(comment, match =>
		{
			var html = "";
			string fullName = match.Groups[1].Value.Trim();
			var words = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length > 2)
			{
				fullName = string.Join(" ", words.Take(2));
			}

			var user = dc.Profiles.FirstOrDefault(u => (u.Firstname + " " + u.Lastname).Trim().ToLower().Contains(fullName.ToLower()));

			if (user != null)
			{
				html = "<a target='_blank' href='/V1/Profile/Profile.aspx?userId=" + user.UserId + "'>" + "@@" + fullName + "</a>";
			}

			return user != null ? html : match.Value;
			//return user != null ? "<a target='_blank' href='/V1/Profile/Profile.aspx?userId=" + user.UserId + "'>@@" + fullName + "</a>" : match.Value;
		});
	}
	public void LoadPosts()
	{
		int streamPostPageSize = int.Parse(ConfigurationManager.AppSettings["streamPostPageSize"].ToString()) + 10;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var posts = from p in dc.Posts
					join pr in dc.Profiles on p.CreatedBy equals pr.UserId
					where p.IsVisible == true
					orderby p.CreatedOn descending
					select new
					{
						p.CreatedBy,
						p.PostId,
						p.PostTypeId,
						p.URLImage,
						p.URLTitle,
						p.URLDescription,
						p.SharedURL,
						p.CreatedOn,
						p.Message,
						pr.UserId,
						fullname = pr.Firstname + " " + pr.Lastname
					};
		rptPosts.DataSource = posts;
		rptPosts.DataBind();
	}

	protected void Button1_Click(object sender, EventArgs e)
	{
		Guid postId = SavePost();
		if (Request.Files.Count > 0)
		{
			for (int i = 0; i < Request.Files.Count; i++)
			{
				HttpPostedFile uploadedFile = Request.Files[i];
				string fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
				string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

				if (Array.Exists(allowedExtensions, ext => ext == fileExtension))
				{
					// Generate a unique name for the file using GUID
					string fileName = Guid.NewGuid().ToString();
					string originalFilePath = Server.MapPath("~/V1/Images/PostImages/" + fileName + fileExtension);
					string thumbnailFilePath = Server.MapPath("~/V1/Images/PostImages/Thumbnails/" + fileName + fileExtension);

					// Ensure directories exist
					Directory.CreateDirectory(Server.MapPath("~/Uploads/"));
					Directory.CreateDirectory(Server.MapPath("~/Uploads/Thumbnails/"));

					// Save and resize the original image to a max width of 1500 pixels
					using (Stream fileStream = uploadedFile.InputStream)
					{
						System.Drawing.Image originalImage = System.Drawing.Image.FromStream(fileStream);
						System.Drawing.Image resizedImage = ResizeImage(originalImage, 1500);
						resizedImage.Save(originalFilePath);

						System.Drawing.Image thumbnailImage = ResizeImage(originalImage, 650);
						thumbnailImage.Save(thumbnailFilePath);
					}

					// Save the GUID (filename) to your database here
					SaveToDatabase(fileName + fileExtension, originalFilePath, postId);

					StatusLabel.Text = "Upload successful!";
				}
				else
				{
					StatusLabel.Text = "Only image files (JPG, PNG, GIF) are allowed.";
				}
			}
		}
		else
		{
			StatusLabel.Text = "No files selected.";
		}
		LoadPosts();
	}

	// Stub method to simulate saving the GUID to the database
	private void SaveToDatabase(string fileName, string filePath, Guid postId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		PostImage postImage = new PostImage();
		postImage.PostImageId = Guid.NewGuid();
		postImage.CreatedBy = userId;
		postImage.CreatedOn = DateTime.Now;
		postImage.ImageFilepath = filePath;
		postImage.ImageFilename = fileName;
		postImage.IsDeleted = false;
		postImage.PostId = postId;
		dc.PostImages.InsertOnSubmit(postImage);
		dc.SubmitChanges();
		LoadPosts();
	}

	protected void rptPosts_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var userId = new Guid();
			string username = HttpContext.Current.User.Identity.Name;
			MembershipUser user = Membership.GetUser(username);
			if (user != null)
			{
				userId = new Guid(user.ProviderUserKey.ToString());
			}
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid postId = (Guid)DataBinder.Eval(dataItem.DataItem, "PostId");
			Guid postTypeId = (Guid)DataBinder.Eval(dataItem.DataItem, "PostTypeId");
			DateTime createdOn = (DateTime)DataBinder.Eval(dataItem.DataItem, "CreatedOn");
			String fullname = (String)DataBinder.Eval(dataItem.DataItem, "Fullname");
			String message = (String)DataBinder.Eval(dataItem.DataItem, "Message");
			Guid createdBy = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String URLImage = (String)DataBinder.Eval(dataItem.DataItem, "URLImage");
			String URLDescription = (String)DataBinder.Eval(dataItem.DataItem, "URLDescription");
			String URLTitle = (String)DataBinder.Eval(dataItem.DataItem, "URLTitle");
			String SharedURL = (String)DataBinder.Eval(dataItem.DataItem, "SharedURL");
			HyperLink hypCreatedBy = (HyperLink)e.Item.FindControl("hypCreatedBy");
			Label lblMessageDate = (Label)e.Item.FindControl("lblMessageDate");
			Literal litMessage = (Literal)e.Item.FindControl("litMessage");
			Literal litReactionTitle = (Literal)e.Item.FindControl("litReactionTitle");
			//Repeater rptPostComments = (Repeater)e.Item.FindControl("rptPostComments");
			Literal litReactionCount = (Literal)e.Item.FindControl("litReactionCount");
			Literal litCommentsCount = (Literal)e.Item.FindControl("litCommentsCount");
			HtmlImage imgProfile = (HtmlImage)e.Item.FindControl("imgProfile");
			//int postCount = (int)DataBinder.Eval(dataItem.DataItem, "postCount");

			lblMessageDate.Text = GetElapsedTime(createdOn);
			hypCreatedBy.Text = fullname;
			hypCreatedBy.NavigateUrl = "/V1/Profile/Profile.aspx?userId=" + createdBy;

			var postReaction = from pr in dc.PostReactions
							   join p in dc.Profiles on pr.CreatedBy equals p.UserId
							   where pr.PostId == postId
							   orderby pr.CreatedOn descending
							   select new { pr.PostReactionId, pr.CreatedBy, p.Firstname, pr.ReactionTypeId };
			Guid reactionTypeID = new Guid();
			if (postReaction.Count() > 0)
			{
				if (!string.IsNullOrEmpty(username))
				{
					var dataExist = postReaction.FirstOrDefault(f => f.CreatedBy == userId);
					if (dataExist != null)
					{
						reactionTypeID = (Guid)dataExist.ReactionTypeId;
					}
				}
				if (postReaction.FirstOrDefault(f => f.CreatedBy == userId) != null)
				{
					litReactionCount.Text = "You and " + (postReaction.Count() - 1).ToString() + " others";
				}
				else
				{
					litReactionCount.Text = postReaction.FirstOrDefault().Firstname + " and " + (postReaction.Count() - 1).ToString() + " others";
				}
			}
			else
			{
				litReactionCount.Text = "";
			}

			string postHtml = string.Empty;
			bool URLShared = false;
			string URLLink = SharedURL;

			if (!String.IsNullOrEmpty(SharedURL))
			{
				//A url was used.
				//Make sure the URL is used to open the link.
				postHtml = "<div class=\"post\"";
				URLShared = true;

				//Shorted the URL to the TLD to make the text easier to read.
				SharedURL = ExtractDomainAndTld(SharedURL);
			}
			else
			{
				postHtml = "<div class=\"post\">";
			}

			postHtml += !String.IsNullOrEmpty(message) ? "<p>" + message + "</p>" : string.Empty;// If there's a message, show it.
																								 //
			if (URLShared)
			{
				//Load the URL Preview DIV
				if (!String.IsNullOrEmpty(URLImage))
				{
					postHtml += "<div onclick =\'window.open(\"" + URLLink + "\", \"_blank\")\' class=\"image-container URLPost\"><img class=\"responsive-image\" src=\"" + URLImage + "\" alt=\"Image\"></div>";
				}

				postHtml += "<div class=\"text-container URLPost\"><small class=\"text-muted\">" + SharedURL + "</small></br>";
				postHtml += "<b>" + URLTitle + "</b>";
				postHtml += "<p>" + URLDescription + "</p></div>";
			}

			string divSingleImage = string.Empty;
			//If they shared images, show them too.


			var postImages = from pi in dc.PostImages
							 where pi.PostId == postId
							 select new { pi.ImageFilename, pi.PostImageId };

			//dc.PostReactions

			if (postImages.Count() > 0)
			{
				foreach (var postImage in postImages)
				{
					Guid postImageId = postImage.PostImageId;
					string imageFilename = postImage.ImageFilename;
					if (postImages.Count() == 1)
					{
						divSingleImage = "<div class=\"single-thumbnail-container image-container-post\"><img class=\"thumbnail\" src=\"/V1/Images/PostImages/Thumbnails/" + imageFilename + "\"></div>";
					}
					else
					{
						divSingleImage += "<div class=\"thumbnail-container\"><img class=\"thumbnail\" src=\"/V1/Images/PostImages/Thumbnails/" + imageFilename + "\"></div>";
					}
				}
			}

			postHtml += divSingleImage;
			postHtml += "</div>";

			litMessage.Text = postHtml;

			HyperLink hypVolunteer = (HyperLink)e.Item.FindControl("hypVolunteer");
			HyperLink hypDonate = (HyperLink)e.Item.FindControl("hypDonate");

			var profileImage = (from ph in dc.ProfilePhotos
								join p in dc.Photos on ph.PhotoId equals p.PhotoId
								where ph.UserId == createdBy && ph.IsCurrrent == true
								orderby p.CreatedOn descending
								select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			if (profileImage != null)
			{
				//Get the users profile image
				imgProfile.Src = profilePhotoFolder + profileImage.FilenameCropped;
			}

			if (reactionTypeID == new Guid("463be049-a178-4327-948c-eb3e3e7dce73"))
			{
				litReactionTitle.Text = "<span style='color: #286090'> &#128591; Thank </span>";
			}
			else if (reactionTypeID == new Guid("b247efe7-3da7-44fa-9452-a331f71d337f"))
			{
				litReactionTitle.Text = "<span style='color: #FF0000'> &#10084; Love </span>";
			}
			else if (reactionTypeID == new Guid("8fe324d4-3694-4b7d-b710-df277c74b1c4"))
			{
				litReactionTitle.Text = "<span style='color: #f0ad4e'> &#128171; Bump </span>";
			}
			else if (reactionTypeID == new Guid("6528bbd7-501b-475b-a15f-520bb0a3ffbf"))
			{
				litReactionTitle.Text = "<span style='color: #f0ad4e'> &#128074; Be Strong </span>";
			}
			else if (reactionTypeID == new Guid("43142e57-f55b-4c8d-b024-84e0e5c664e9"))
			{
				litReactionTitle.Text = "<span style='color: #eea236'> &#128558; Wow </span>";
			}
			else
			{
				litReactionTitle.Text = "<span style='color: #777'> &#128077; Thank </span>";
			}


			//var profileImage = (from ph in dc.ProfilePhotos
			//					join p in dc.Photos on ph.PhotoId equals p.PhotoId
			//					where ph.UserId == createdBy && ph.IsCurrrent == true
			//					orderby p.CreatedOn descending
			//					select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			//if (profileImage != null)
			//{
			//	//Get the users profile image
			//	imgProfile.Src = profilePhotoFolder + profileImage.FilenameCropped;
			//}


			//rptPostComments.DataSource = from pc in dc.PostComments
			//							 join c in dc.Comments on pc.CommentId equals c.CommentId
			//							 //join ph in dc.ProfilePhotos on c.CreatedBy equals ph.UserId into phJoin
			//							 //from ph in phJoin.DefaultIfEmpty() // This ensures a left join
			//							 //join p in dc.Photos on ph.PhotoId equals p.PhotoId into pJoin
			//							 //from p in pJoin.DefaultIfEmpty() // This ensures a left join
			//							 where pc.PostId == postId
			//								   && c.ParentId == null
			//								   && c.IsDeleted == false
			//							 orderby c.CreatedOn descending
			//							 select new
			//							 {
			//								 pc.PostCommentId,
			//								 Comment1 = ReplaceTaggedUsersWithLinks(c.Comment1),
			//								 c.CreatedOn,
			//								 c.CommentId,
			//								 isEdit = c.CreatedBy == userId ? true : false,
			//								 postId = pc.PostId,
			//								 timeAgo = GetTimeAgo(c.CreatedOn),
			//								 author = dc.Profiles.FirstOrDefault(f => f.UserId == c.CreatedBy).Firstname,
			//								 ProfileUrl = "/V1/Profile/Profile.aspx?userId=" + c.CreatedBy,
			//								 imgProfileUrl = profilePhotoFolder + (
			//														 (from rph in dc.ProfilePhotos
			//														  join rp in dc.Photos on rph.PhotoId equals rp.PhotoId
			//														  where rph.UserId == c.CreatedBy
			//														  select rp.FilenameCropped).FirstOrDefault() ?? "default.png"),
			//								 replies = dc.Comments
			//											 .Where(f => f.ParentId == c.CommentId && f.IsDeleted == false)
			//											 .OrderBy(f => f.CreatedOn)
			//											 .Select(reply => new
			//											 {
			//												 pc.PostCommentId,
			//												 reply.CommentId,
			//												 Comment1 = ReplaceTaggedUsersWithLinks(reply.Comment1),
			//												 reply.CreatedOn,
			//												 timeAgo = GetTimeAgo(reply.CreatedOn),
			//												 isEdit = reply.CreatedBy == userId ? true : false,
			//												 postId = pc.PostId,
			//												 parentCommentId = c.CommentId,
			//												 ProfileUrl = "/V1/Profile/Profile.aspx?userId=" + reply.CreatedBy,
			//												 author = dc.Profiles.FirstOrDefault(f => f.UserId == reply.CreatedBy).Firstname,
			//												 imgProfileUrl = profilePhotoFolder + (
			//														 (from rph in dc.ProfilePhotos
			//														  join rp in dc.Photos on rph.PhotoId equals rp.PhotoId
			//														  where rph.UserId == reply.CreatedBy
			//														  select rp.FilenameCropped).FirstOrDefault() ?? "default.png"),
			//											 }).ToList()
			//							 };
			//rptPostComments.DataBind();
			litCommentsCount.Text = dc.PostComments.Where(f => f.PostId == postId).ToList().Count.ToString() + " Comments";
		}
	}

	protected Guid SavePost()
	{
		Guid postId = Guid.NewGuid();

		string URLTitle = String.IsNullOrEmpty(postURLTitle.Value) ? null : postURLTitle.Value;
		string URLDescription = String.IsNullOrEmpty(postURLDescription.Value) ? null : postURLDescription.Value;
		string URLImage = String.IsNullOrEmpty(postURLImage.Value) ? null : postURLImage.Value;
		string URL = String.IsNullOrEmpty(postURL.Value) ? null : postURL.Value;
		string postText = String.IsNullOrEmpty(postInput.Value) ? null : postInput.Value;
		string _postTypeId = String.IsNullOrEmpty(postTypeId.Value) ? "8D8CDB63-28D9-4265-AC78-AF06BA6AC582" : postTypeId.Value;
		string audienceTypeId = String.IsNullOrEmpty(AudienceType.Value) ? "FA66D7CD-4B31-4A52-B15B-4E76FD2030C2" : AudienceType.Value; //Default to public

		if (!String.IsNullOrEmpty(postText))
		{
			postText = postText.Replace(Environment.NewLine, "<br />");
		}

		postURLTitle.Value = String.Empty;
		postURLDescription.Value = String.Empty;
		postURLImage.Value = String.Empty;
		postURL.Value = String.Empty;
		postInput.Value = String.Empty;
		postTypeId.Value = String.Empty;
		AudienceType.Value = String.Empty;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		Post post = new Post();
		post.PostId = postId;
		post.ViewCount = 0;
		post.Message = postText;
		post.URLDescription = URLDescription;
		post.URLTitle = URLTitle;
		post.URLImage = URLImage;
		post.IsVisible = true;
		post.CreatedOn = DateTime.Now;
		post.CreatedBy = userId;
		post.SharedURL = URL;
		post.AudienceTypeId = new Guid(audienceTypeId);
		post.PostTypeId = new Guid(_postTypeId);
		dc.Posts.InsertOnSubmit(post);
		dc.SubmitChanges();

		LoadPosts();

		return postId;
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		Guid postId = SavePost();
	}

	static string ExtractDomainAndTld(string url)
	{
		// Create a Uri object from the given URL
		Uri uri = new Uri(url);

		// Get the host part of the URL (e.g., www.example.com)
		string host = uri.Host;

		// Split the host by dots
		string[] parts = host.Split('.');

		// Check that the host contains at least two parts (domain and TLD)
		if (parts.Length >= 2)
		{
			// Get the last two parts (domain and TLD)
			string domain = parts[parts.Length - 2];
			string tld = parts[parts.Length - 1];

			return domain + "." + tld;
		}

		return host; // Fallback if something goes wrong
	}
	// Method to resize the image
	private System.Drawing.Image ResizeImage(System.Drawing.Image originalImage, int maxWidth)
	{
		int originalWidth = originalImage.Width;
		int originalHeight = originalImage.Height;

		// Calculate the new dimensions while maintaining the aspect ratio
		float ratio = (float)maxWidth / originalWidth;
		int newWidth = maxWidth;
		int newHeight = (int)(originalHeight * ratio);

		Bitmap resizedImage = new Bitmap(newWidth, newHeight);
		using (Graphics g = Graphics.FromImage(resizedImage))
		{
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
		}

		return resizedImage;
	}

	[WebMethod]
	public static bool UploadPostReaction(string reactionId, string postId)
	{
		var isRemoved = false;
		var userId = new Guid();
		string username = HttpContext.Current.User.Identity.Name;
		MembershipUser user = Membership.GetUser(username);
		if (user != null)
		{
			userId = new Guid(user.ProviderUserKey.ToString());

			using (var dc = new CrowdReliefDBDataContext())
			{
				PostReaction pr = new PostReaction();
				pr.PostReactionId = Guid.NewGuid();
				pr.PostId = new Guid(postId);
				pr.ReactionTypeId = new Guid(reactionId);
				pr.CreatedBy = userId;
				pr.CreatedOn = DateTime.Now;
				pr.IsDeleted = false;

				var IsExists = dc.PostReactions.FirstOrDefault(f => f.CreatedBy == userId && f.PostId == new Guid(postId));
				if (IsExists != null)
				{
					dc.PostReactions.DeleteOnSubmit(IsExists);
				}
				if (IsExists != null && IsExists.ReactionTypeId == new Guid(reactionId))
				{
					dc.PostReactions.DeleteOnSubmit(IsExists);
					isRemoved = true;
				}
				else
				{
					dc.PostReactions.InsertOnSubmit(pr);
					isRemoved = false;
				}
				dc.SubmitChanges();
				return isRemoved;
			}
		}
		return isRemoved;
	}

	[WebMethod]
	public static bool UploadPostComment(string postId, string commentId, string comment, bool isReply, bool isEditComment)
	{
		var isRemoved = true;
		var userId = new Guid();
		string username = HttpContext.Current.User.Identity.Name;
		MembershipUser user = Membership.GetUser(username);

		if (user != null)
		{
			userId = new Guid(user.ProviderUserKey.ToString());

			using (var dc = new CrowdReliefDBDataContext())
			{
				Comment com = new Comment();
				PostComment pc = new PostComment();
				var users = dc.Profiles.ToList();
				List<Profile> usersList = ExtractTaggedUser(comment, users);
				if (isEditComment == true)
				{
					com = dc.Comments.FirstOrDefault(f => f.CommentId == new Guid(commentId));
					if (com != null)
					{
						com.Comment1 = ExtractTaggedMessage(comment, users);
					}
				}
				else
				{
					com.CommentId = Guid.NewGuid();
					com.Comment1 = ExtractTaggedMessage(comment, users);
					com.CreatedBy = userId;
					com.CreatedOn = DateTime.Now;
					com.IsDeleted = false;
					com.ParentId = isReply ? new Guid(commentId) : (Guid?)null;
					dc.Comments.InsertOnSubmit(com);

					if (!isReply)
					{
						pc.PostId = new Guid(postId);
						pc.CommentId = com.CommentId;
						pc.UserId = userId;
						pc.PostCommentId = Guid.NewGuid();
						pc.IsDeleted = false;
						pc.CreatedOn = DateTime.Now;
						dc.PostComments.InsertOnSubmit(pc);
					}
				}

				if (usersList != null)
				{
					foreach (var item in usersList)
					{
						PostTaggedUser ptu = new PostTaggedUser();
						var isExist = dc.PostTaggedUsers.FirstOrDefault(f => f.CommentId == com.CommentId && f.PostId == new Guid(postId) && f.TaggedUser == item.UserId);
						if (isExist == null)
						{
							ptu.PostTaggedUserId = Guid.NewGuid();
							ptu.CommentId = com.CommentId;
							ptu.PostId = new Guid(postId);
							ptu.TaggedUser = item.UserId;
							ptu.CreatedBy = userId;
							ptu.CreatedOn = DateTime.Now;
							ptu.IsEmailNotification = false;
							ptu.IsSMSNotification = false;
							dc.PostTaggedUsers.InsertOnSubmit(ptu);
						}
						else
						{
							ptu = isExist;
						}

						if (ptu.IsEmailNotification == false)
						{
							//Send Email Notification
							ptu.IsEmailNotification = true;
						}

						if (ptu.IsSMSNotification == false)
						{
							//Send SMS Notification
							ptu.IsSMSNotification = true;
						}
					}
				}
				dc.SubmitChanges();
				return isRemoved;
			}
		}
		return isRemoved;
	}

	[WebMethod]
	public static bool DeletePostComment(string postCommentId)
	{
		var isRemoved = true;
		using (var dc = new CrowdReliefDBDataContext())
		{
			var postComment = dc.PostComments.FirstOrDefault(f => f.CommentId == new Guid(postCommentId));
			if (postComment != null)
			{
				var comment = dc.Comments.FirstOrDefault(f => f.CommentId == postComment.CommentId);
				comment.IsDeleted = true;
				foreach (var item in dc.Comments.Where(f => f.ParentId == comment.CommentId).ToList())
				{
					item.IsDeleted = true;
				}
				//dc.Comments.DeleteOnSubmit(comment);
			}
			//dc.PostComments.DeleteOnSubmit(postComment);
			postComment.IsDeleted = true;
			dc.SubmitChanges();
		}
		return isRemoved;
	}

	[WebMethod]
	public static bool DeletePostReply(string postCommentId)
	{
		var isRemoved = true;
		using (var dc = new CrowdReliefDBDataContext())
		{
			var comment = dc.Comments.FirstOrDefault(f => f.CommentId == new Guid(postCommentId));
			//dc.Comments.DeleteOnSubmit(comment);
			comment.IsDeleted = true;
			dc.SubmitChanges();
		}
		return isRemoved;
	}

	[WebMethod]
	public static List<PostCommentsModel> GetCommentsByPostId(string postId)
	{
		StringWriter sw = new StringWriter();
		HtmlTextWriter writer = new HtmlTextWriter(sw);
		var comments = new List<PostCommentsModel>();
		using (var dc = new CrowdReliefDBDataContext())
		{
			var userId = new Guid();
			string username = HttpContext.Current.User.Identity.Name;
			MembershipUser user = Membership.GetUser(username);
			if (user != null)
			{
				userId = new Guid(user.ProviderUserKey.ToString());
			}

			var roleId = dc.aspnet_UsersInRoles.FirstOrDefault(f => f.UserId == userId).RoleId;
			var role = dc.aspnet_Roles.FirstOrDefault(f => f.RoleId == roleId).RoleName;

			string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
			comments = (from pc in dc.PostComments
						join c in dc.Comments on pc.CommentId equals c.CommentId
						where pc.PostId == new Guid(postId)
							  && c.ParentId == null
							  && c.IsDeleted == false
						orderby c.CreatedOn descending
						select new PostCommentsModel
						{
							PostCommentId = pc.PostCommentId,
							Comment1 = ReplaceTaggedUsersWithLinks(c.Comment1),
							CreatedOn = c.CreatedOn,
							CommentId = c.CommentId,
							IsEdit = c.CreatedBy == userId,
							IsDelete = (role == "ContentManager" ? true : c.CreatedBy == userId),
							PostId = pc.PostId,
							TimeAgo = GetTimeAgo(c.CreatedOn),
							Author = dc.Profiles.FirstOrDefault(f => f.UserId == c.CreatedBy).Firstname,
							ProfileUrl = "/V1/Profile/Profile.aspx?userId=" + c.CreatedBy,
							ImgProfileUrl = profilePhotoFolder + (
												(from rph in dc.ProfilePhotos
												 join rp in dc.Photos on rph.PhotoId equals rp.PhotoId
												 where rph.UserId == c.CreatedBy
												 select rp.FilenameCropped).FirstOrDefault() ?? "profilepicture.png"),
							Replies = dc.Comments
										.Where(f => f.ParentId == c.CommentId && f.IsDeleted == false)
										.OrderBy(f => f.CreatedOn)
										.Select(reply => new Reply
										{
											PostCommentId = pc.PostCommentId,
											CommentId = reply.CommentId,
											Comment1 = ReplaceTaggedUsersWithLinks(reply.Comment1),
											CreatedOn = reply.CreatedOn,
											TimeAgo = GetTimeAgo(reply.CreatedOn),
											IsEdit = c.CreatedBy == userId,
											IsDelete = (role == "ContentManager" ? true : c.CreatedBy == userId),
											PostId = pc.PostId,
											ParentCommentId = c.CommentId,
											ProfileUrl = "/V1/Profile/Profile.aspx?userId=" + reply.CreatedBy,
											Author = dc.Profiles.FirstOrDefault(f => f.UserId == reply.CreatedBy).Firstname,
											ImgProfileUrl = profilePhotoFolder + (
														 (from rph in dc.ProfilePhotos
														  join rp in dc.Photos on rph.PhotoId equals rp.PhotoId
														  where rph.UserId == reply.CreatedBy
														  select rp.FilenameCropped).FirstOrDefault() ?? "profilepicture.png")
										}).ToList()
						}).ToList();

			//return comments;
			//var rptPostComments = new Repeater();
			//rptPostComments.DataSource = comments;
			//rptPostComments.ItemTemplate = new CompiledTemplateBuilder((container) =>
			//{
			//	Literal author = new Literal();
			//	container.Controls.Add(author);
			//});
			//rptPostComments.DataBind();
			//rptPostComments.RenderControl(writer);
		}
		return comments;
	}

	[WebMethod]
	public static object GetUsers()
	{
		using (var dc = new CrowdReliefDBDataContext())
		{
			var users = dc.Profiles.Take(20).Select(pr => new
			{
				Title = pr.Firstname + " " + pr.Lastname,
				UserId = pr.UserId
			}).ToList();

			return users;
		}
	}

}