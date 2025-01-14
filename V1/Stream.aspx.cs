using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Security.Policy;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Services.Description;
using System.Web.Security;
using System.Web.Services;
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

		if (!this.IsPostBack)
		{

		}
		else
		{
		}

		this.Master.PageTitle = "Stability Activity Feed";
		this.Master.PageDescription = "View posts and updates from your team, friends and the public.";
		this.Master.FbDescription = "View posts and updates from your team, friends and the public.";
		this.Master.FbSite_name = "Stability Activity Feed";
		this.Master.HideMasterCover = true;
	}

	public void BindDropdown()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
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
			Literal litReactionCount = (Literal)e.Item.FindControl("litReactionCount");
			HtmlImage imgProfile = (HtmlImage)e.Item.FindControl("imgProfile");

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
}