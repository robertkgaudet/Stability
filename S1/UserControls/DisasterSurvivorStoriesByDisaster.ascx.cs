using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class S1_UserControls_DisasterSurvivorStoriesByDisaster : System.Web.UI.UserControl
{
	Guid	_eventId			= Guid.Empty;
	public string gridItem = string.Empty;
	public string disasterSurvivorStoryCategoryId	= ConfigurationManager.AppSettings["disasterSurvivorStoryCategoryId"].ToString();
	protected void Page_Load(object sender, EventArgs e)
	{
		string delete			= Request.QueryString["d"];
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if(!String.IsNullOrEmpty(delete))
		{
			string articleId = Request.QueryString["articleId"];
			var article = (from a in dc.Articles
						  where a.ArticleId == new Guid(articleId)
						  orderby a.CreatedOn ascending
						  select a).SingleOrDefault();
			
			article.IsDeleted = true;
			dc.SubmitChanges();
		}

		string category			= Request.QueryString["category"];
		string userId			= Request.QueryString["userId"];
		string profileNumber	= Request.QueryString["profileNumber"];
		
		LoadCategories(eventId);

		if(!string.IsNullOrEmpty(profileNumber))
		{
			var userUserId =  (from p in dc.Profiles
						  where p.ProfileNumber == Int32.Parse(profileNumber)
						  select new {p.UserId}).SingleOrDefault();

			userId = userUserId.UserId.ToString();
		}

		if(!String.IsNullOrEmpty(userId))
		{
			LoadStories(new Guid(userId), category, _eventId);
		}
		else
		{
			LoadStories(Guid.Empty, category, _eventId);
		}
	}

	protected void LoadCategories(Guid eventId)
	{
		string categoryList = string.Empty;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var categories = (from c in dc.Categories
						join ac in dc.ArticleCategories on c.CategoryId equals ac.CategoryId
						where c.ParentCategory == "Survivor" && ac.Article.EventId == eventId
						orderby c.Category1
						select new { c.Category1 }).Distinct();

		if(categories.Count() > 0)
		{
			foreach(var category in categories)
			{
				categoryList += "<a href=\"\\c\\" + category.Category1.Replace(" ","-").Replace("/","_") + "\" class=\"btn btn-default m-xs btn-xs\">" + category.Category1 + "</a>";
			}
			litCategories.Text = categoryList;
		}
	}
	
	protected void LoadStories(Guid userId, string category, Guid eventId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		BaseOrganizationWebForm baseOrganizationWebForm = new BaseOrganizationWebForm();

		Guid loggedInUserId = Guid.Empty;
		if(HttpContext.Current.User.Identity.IsAuthenticated)
		{
			loggedInUserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			hypAddStory.Visible = true;
		}

		Guid categoryId = new Guid(disasterSurvivorStoryCategoryId); //Story category.

		if(!string.IsNullOrEmpty(category))
		{
			category = category.Replace("_","/").Replace("-"," ");
			var categoryIdSelector = (from c in dc.Categories
							where c.Category1 == category && c.ParentCategory == "Survivor"
							select new { c.CategoryId }).SingleOrDefault();

			categoryId = categoryIdSelector.CategoryId;
		}
		
		string profileImagePlaceHolderAndPath	= System.Configuration.ConfigurationManager.AppSettings["profileImagePlaceHolderAndPath"].ToString();
		string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		string storyPhotoFolder		= System.Configuration.ConfigurationManager.AppSettings["storyPhotoFolder"].ToString();
	
		var stories = from a in dc.Articles
						join u in dc.Profiles on a.CreatedBy equals u.UserId
						join ac in dc.ArticleCategories on a.ArticleId equals ac.ArticleId
						where ac.CategoryId == categoryId// stories Guid.
						&& a.IsDeleted == false
						orderby a.CreatedOn descending
						select new {a, u};

		litName.Text = "Stability Disaster Survivor Stories";
		litFulleName.Text = "Help those affected by disasters by reading their survival and recovery stories!";

		if(userId != Guid.Empty)
		{
			litName.Text = stories.Take(1).SingleOrDefault().u.Firstname + "'s  Stories";
			litFulleName.Text = "Disaster stories for " + stories.Take(1).SingleOrDefault().u.Firstname + " " + stories.Take(1).SingleOrDefault().u.Lastname;
			stories = stories.Where(s =>(s.u.UserId == userId) || (s.a.UserId == userId));
		}

		if(eventId != Guid.Empty)
		{
			stories = stories.Where(s =>(s.a.EventId == eventId));
		}

		foreach(var story in stories)
		{
			string profilePhoto = string.Empty;
			string articleAuthorName = string.Empty;

			var profileImage = (from ph in dc.ProfilePhotos
							   join p in dc.Photos on ph.PhotoId equals p.PhotoId
							   where ph.UserId == story.u.UserId && ph.IsCurrrent == true
							   orderby p.CreatedOn descending
							   select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			if(profileImage != null)
			{
				//Get the users profile image
				profilePhoto = profilePhotoFolder + profileImage.FilenameCropped;
			}
			else
			{
				profilePhoto = profileImagePlaceHolderAndPath;
			}

			int? viewCount = 0;
			if(story.a.ViewCount != null)
			{
				viewCount = story.a.ViewCount;
			}

			articleAuthorName = "<a href=\"\\s\\" +  story.u.ProfileNumber + "\\" + story.u.Firstname + "-" + story.u.Lastname + "\">" + story.u.Firstname + " " + story.u.Lastname + "</a>";

			//Count the comments.
			//Count the views.
			var commentCount = (from c in dc.ArticleComments
							   where c.ArticleId == story.a.ArticleId
							   select c).Count();

			string text = story.a.Text.Length > 501 ? story.a.Text.Substring(0, 500) + "..." : story.a.Text;
			text = text.Replace("<p>","").Replace("</p>","").Replace("<b>","").Replace("</b>","").Replace("<u>","").Replace("</u>","").Replace("<i>","").Replace("</i>","").Replace("<div>","").Replace("</div>","");
			text = StripHTML(text);
			string adminFooter = string.Empty;
		
			if(HttpContext.Current.User.IsInRole("Administrator") || story.u.UserId == loggedInUserId)
			{
				//string deleteButton = "<button class=\"btn btn-warning btn-sm deleteButton\">Delete This Article</button>";
				string deleteButton = "<a href=\"\\S1\\Default.aspx?d=t&userId=" + story.u.UserId + "&articleId=" + story.a.ArticleId + "\" class=\"deleteConfirmation\">Delete</a>";
				string editButton = "<a href=\"\\S1\\Profile\\EditBlogPost.aspx?userId=" + story.u.UserId + "&articleId=" + story.a.ArticleId + "\" class=\"editConfirmation\">Edit</a>";
				string uploadPhoto = "<a href=\"\\S1\\Profile\\BlogUploadPhoto.aspx?userId=" + story.u.UserId + "&articleId=" + story.a.ArticleId + "\" class=\"editConfirmation\">Add Photo</a>";
				adminFooter = "<div class=\"panel-footer\">" +  deleteButton + " | " + editButton + " | " + uploadPhoto + "</div>";
					//<a href=\"\\S1\\Default.aspx?d=t&userId=" + story.u.UserId + "&articleId=" + story.a.ArticleId + "\" class=\"deleteConfirmation\">Delete</a></div>";
			}


			string storyTitle = "<a href=\"\\a\\" + story.a.ArticleNumber + "\\" + story.a.URLFriendlyTitle + "\"> <h4>" + story.a.Title + "</h4></a>" +  Environment.NewLine;
			string articlePhoto = string.Empty;

			var storyPhoto = (from sp in dc.ArticlePhotos
							 join p in dc.Photos on sp.PhotoId equals p.PhotoId
							 where sp.ArticleId == story.a.ArticleId
							 orderby sp.CreatedOn descending
							 select new {p.FilenameCropped }).Take(1).SingleOrDefault();
			
			//If we have an article photo, use it.
			if(storyPhoto != null)
			{

				articlePhoto =
					"<div class=\"panel-image tinted\">" + Environment.NewLine +
						"<img class=\"img-responsive\" src=\"" + storyPhotoFolder + storyPhoto.FilenameCropped + "\">" + Environment.NewLine +
							"<div class=\"title\">" + Environment.NewLine +
								storyTitle + Environment.NewLine +
								"<small>Stability Survivor Stories</small>" + Environment.NewLine +
								  "</div>" + Environment.NewLine +
						"</div>" +  Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
				storyTitle = string.Empty;
			}

			var survivor = (from s in dc.Profiles
						   where s.UserId == story.a.UserId
						   select s).SingleOrDefault();

			string survivorInfo = string.Empty;
			if(survivor != null)
			{
				survivorInfo = " for <a href=\"\\s\\" +  survivor.ProfileNumber + "\\" + survivor.Firstname + "-" + survivor.Lastname + "\">" + survivor.Firstname + " " + survivor.Lastname + "</a>";
			}
			
			gridItem += Environment.NewLine + Environment.NewLine +			"<!--BEGIN GRID ITEM-->" + Environment.NewLine +				"<div class=\"grid-item\">" + Environment.NewLine +					"<div class=\"hpanel blog-box\">" +  Environment.NewLine +
						"<div class=\"panel-heading\">" +  Environment.NewLine +
							"<div class=\"media clearfix\">" +  Environment.NewLine +
								"<a class=\"pull-left\">" +  Environment.NewLine +
									"<img src=\"" + profilePhoto + "\" alt=\"post-picture\">" +  Environment.NewLine +
								"</a>" +  Environment.NewLine +
								"<div class=\"media-body\">" +  Environment.NewLine +
									"<small>Written by: <span class=\"font-bold\">" + articleAuthorName + " " + survivorInfo + "</span> </small>" +  Environment.NewLine +
									"<br/>" +  Environment.NewLine +
									"<small class=\"text-muted\">" + baseOrganizationWebForm.GetElapsedTime(story.a.CreatedOn) + "</small>" +  Environment.NewLine +
								"</div>" +  Environment.NewLine +
							"</div>" +  Environment.NewLine +
						"</div>" +  Environment.NewLine +
						articlePhoto +Environment.NewLine +
						"<div class=\"panel-body\">" +  Environment.NewLine +
							storyTitle + 
							"<p>" +  Environment.NewLine +
								text + Environment.NewLine + 
							"</p>" +  Environment.NewLine +
						"</div>" +  Environment.NewLine +
						"<div class=\"panel-footer\">" +  Environment.NewLine +
								"<span class=\"pull-right\">" +  Environment.NewLine +
									"<i class=\"fa fa-comments-o\"> </i> " + commentCount + " comments" + Environment.NewLine + 
								"</span>" +  Environment.NewLine +
							"<i class=\"fa fa-eye\"> </i> " + viewCount + " views" +  Environment.NewLine +
						"</div>" +  Environment.NewLine +
						adminFooter +  Environment.NewLine +
					"</div>" +  Environment.NewLine +
				"</div>" + Environment.NewLine +
			"<!--END GRID ITEM-->" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
		}
	}
	protected string StripHTML(string input)
	{
		if (!string.IsNullOrEmpty(input))
		{
			input = Regex.Replace(input, "<.*?>", String.Empty);
		}
		return input;
	}
	public Guid eventId
	{
		get { return _eventId; }
		set { _eventId = value; }
	}
}