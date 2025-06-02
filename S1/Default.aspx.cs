using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class S1_Default : BaseOrganizationWebForm
{
	public string gridItem = string.Empty;
	public string disasterSurvivorStoryCategoryId	= ConfigurationManager.AppSettings["disasterSurvivorStoryCategoryId"].ToString();
	public string disasterHelperStoryCategoryId	= ConfigurationManager.AppSettings["disasterHelperStoryCategoryId"].ToString();
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
		string eventId			= Request.QueryString["eventId"];
		string profileNumber	= Request.QueryString["profileNumber"];
		Guid userIdGuid			= Guid.Empty;
		Guid eventIdGuid		= Guid.Empty;

		LoadCategories();

		if(!string.IsNullOrEmpty(profileNumber))
		{
			var userUserId =  (from p in dc.Profiles
						  where p.ProfileNumber == Int32.Parse(profileNumber)
						  select new {p.UserId}).SingleOrDefault();

			userId = userUserId.UserId.ToString();
		}

		if(!String.IsNullOrEmpty(userId))
		{
			userIdGuid = new Guid(userId);
		}

		if(!String.IsNullOrEmpty(eventId))
		{
			eventIdGuid = new Guid(eventId);
		}

		LoadStories(userIdGuid, category, eventIdGuid);

	//	Master.BoxedBody = true;
		
		//BEGIN FACEBOOK META TAGS
		Master.PageTitle		= "Create A Community Disaster Relief Team - Stability";
		Master.FbURL			= Request.Url.AbsoluteUri;
		Master.FbDescription	= "Soon after a disaster such as a hurricane, flood or fire, the nation forgets. Stability Disaster Survivor Stories bring to life the real struggle faced by citizens seeking to rebuild their lives after losing everything.";
		Master.PageDescription	= "Soon after a disaster such as a hurricane, flood or fire, the nation forgets. Stability Disaster Survivor Stories bring to life the real struggle faced by citizens seeking to rebuild their lives after losing everything.";
		Master.FbImage			= "/S1/Images/CrowdReliefFBPost.jpg";
		Master.FbImageType		= "image/jpg";
		Master.FbSite_name		= "Stability Disaster Aid Platform";
		//END FACEBOOK META TAGS
	}
	
	protected void LoadCategories()
	{
		string categoryList = string.Empty;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var categories = (from c in dc.Categories
						join ac in dc.ArticleCategories on c.CategoryId equals ac.CategoryId
						where c.ParentCategory == "Survivor"
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

		Guid loggedInUserId = Guid.Empty;
		if(User.Identity.IsAuthenticated)
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
						orderby a.CreatedOn ascending
						select new {a, u};

		var storiesHelpers = from a in dc.Articles
						join u in dc.Profiles on a.CreatedBy equals u.UserId
						join ac in dc.ArticleCategories on a.ArticleId equals ac.ArticleId
						where ac.CategoryId == new Guid(disasterHelperStoryCategoryId)// stories Guid.
						&& a.IsDeleted == false
						orderby a.CreatedOn ascending
						select new {a, u};

		stories = stories.Union(storiesHelpers);

		litName.Text		= "Stability Disaster Stories";
		litFulleName.Text	= "Help those affected by disasters by reading their survival and recovery stories!";

		if(userId != Guid.Empty)
		{
			litName.Text = stories.Take(1).SingleOrDefault().u.Firstname + "'s  Stories";
			litFulleName.Text = "Disaster stories for " + stories.Take(1).SingleOrDefault().u.Firstname + " " + stories.Take(1).SingleOrDefault().u.Lastname;
			stories = stories.Where(c =>(c.u.UserId == userId) || (c.a.UserId == userId));
		}
		
		if(eventId != Guid.Empty)
		{
			stories = stories.Where(s =>(s.a.EventId == eventId));
		}

		stories = stories.OrderByDescending(x => x.a.CreatedOn);

		foreach(var story in stories)
		{
			string profilePhoto = string.Empty;
			string articleAuthorName = string.Empty;
			string containerHeaderText = string.Empty;
			string survivorInfo = string.Empty;

			var articleUser = (from s in dc.Profiles
							where s.UserId == story.a.UserId
							select s).SingleOrDefault();

			string friendlyURLPointer = "sp";
			MembershipUser articleAuthor = Membership.GetUser(story.u.UserId);
			if(Roles.IsUserInRole(articleAuthor.UserName, "helper") || Roles.IsUserInRole(articleAuthor.UserName, "volunteer"))
			{
				//Author is in the helper role.
				friendlyURLPointer = "hp";
			}
			articleAuthorName = "<a href=\"\\" + friendlyURLPointer + "\\" +  story.u.ProfileNumber + "\\" + story.u.Firstname + "-" + story.u.Lastname + "\">" + story.u.Firstname + " " + story.u.Lastname + "</a>";

			//SURVIVOR STORY
			if(articleUser != null)
			{
				survivorInfo = " <a href=\"\\sp\\" +  articleUser.ProfileNumber + "\\" + articleUser.Firstname + "-" + articleUser.Lastname + "\">" + articleUser.Firstname + " " + articleUser.Lastname + "</a>";
			}

			if(!string.IsNullOrEmpty(story.a.CategoryId.ToString()) && new Guid(disasterSurvivorStoryCategoryId) == story.a.CategoryId)
			{

				if(articleUser != null && story.u.ProfileNumber == articleUser.ProfileNumber)
				{
					articleAuthorName = "<a href=\"\\sp\\" +  story.u.ProfileNumber + "\\" + story.u.Firstname + "-" + story.u.Lastname + "\">" + story.u.Firstname + " " + story.u.Lastname + "</a>";
					//Person is writing this article about themselves.
					containerHeaderText = "<small>This story was created by <span class=\"font-bold\">" + articleAuthorName + " who recently suffered loss in a disaster.</span> </small>" +  Environment.NewLine;
				}
				else
				{
					//Person is writing this article about someone else.
					containerHeaderText = "<small>This story was created by disaster volunteer <span class=\"font-bold\">" + articleAuthorName + " about " + survivorInfo + " who recently suffered loss in a disaster.</span> </small>" +  Environment.NewLine;
				}
			}
			else if(!string.IsNullOrEmpty(story.a.CategoryId.ToString()) && new Guid(disasterHelperStoryCategoryId) == story.a.CategoryId)
			{
				survivorInfo = " <a href=\"\\hp\\" +  articleUser.ProfileNumber + "\\" + articleUser.Firstname + "-" + articleUser.Lastname + "\">" + articleUser.Firstname + " " + articleUser.Lastname + "</a>";
				//HELPER STORY
				containerHeaderText = "<small>This story is about disaster volunteer <span class=\"font-bold\">" + survivorInfo + "</span> </small>" +  Environment.NewLine;
			}

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

			//Count the comments.
			//Count the views.
			var commentCount = (from c in dc.ArticleComments
							   where c.ArticleId == story.a.ArticleId
							   select c).Count();

			string text = story.a.Text.Length > 501 ? story.a.Text.Substring(0, 500) + "..." : story.a.Text;
			text = text.Replace("<p>","").Replace("</p>","").Replace("<b>","").Replace("</b>","").Replace("<u>","").Replace("</u>","").Replace("<i>","").Replace("</i>","").Replace("<div>","").Replace("</div>","");
			text = StripHTML(text);
			string adminFooter = string.Empty;
		
			if(User.IsInRole("Administrator") || story.u.UserId == loggedInUserId)
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

			//donation entry
			var donationQuery = (from d in dc.UserDonations where d.RecipientId == story.a.UserId orderby d.CreatedOn descending select d);

			float totalDonated = 0;

			foreach(var pastDonation in donationQuery)
            {
				totalDonated += pastDonation.Amount;
            }

			gridItem += Environment.NewLine + Environment.NewLine +			"<!--BEGIN GRID ITEM-->" + Environment.NewLine +				"<div class=\"grid-item\">" + Environment.NewLine +					"<div class=\"hpanel blog-box\">" +  Environment.NewLine +
						"<div class=\"panel-heading\">" +  Environment.NewLine +
							"<div class=\"media clearfix\">" +  Environment.NewLine +
								"<a class=\"pull-left\">" +  Environment.NewLine +
									"<img src=\"" + profilePhoto + "\" alt=\"post-picture\">" +  Environment.NewLine +
								"</a>" +  Environment.NewLine +
								"<div class=\"media-body\">" +  Environment.NewLine +
									containerHeaderText +  Environment.NewLine +
									"<br/>" +  Environment.NewLine +
									"<small class=\"text-muted\">" + GetElapsedTime(story.a.CreatedOn) + "</small>" +  Environment.NewLine +
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
							"<i class=\"fa fa-eye\"> </i> " + (viewCount + 248) + " views" +  Environment.NewLine +
						"</div>" +  Environment.NewLine +
						"<div class=\"panel-footer\">" + Environment.NewLine +
								"<span class=\"pull-right\">" + Environment.NewLine +
									"<a href=\"/a/" + story.a.ArticleNumber + "/" + story.a.URLFriendlyTitle + "\" ><i class=\"fa fa-heart\"> </i> Donate</a>" + Environment.NewLine +
								"</span>" + Environment.NewLine +
							"<i class=\"fa fa-dollar\"> </i>" + Math.Round(totalDonated, 0).ToString() + " Donated" + Environment.NewLine + 
						"</div>" + Environment.NewLine +
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
}