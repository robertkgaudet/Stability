using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Configuration;

public partial class S1_Profile_NewBlogPost : System.Web.UI.Page
{
	public string preselectedSurvivorJQuery;
	public string preselectedHelperJQuery;
	public string userDropDown;
	string survivorId = HttpContext.Current.Request.QueryString["survivorId"];
	string helperId = HttpContext.Current.Request.QueryString["helperId"];
	public string storySubject = HttpContext.Current.Request.QueryString["storySubject"];
	public string disasterSurvivorStoryCategoryId	= ConfigurationManager.AppSettings["disasterSurvivorStoryCategoryId"].ToString();
	public string disasterHelperStoryCategoryId	= ConfigurationManager.AppSettings["disasterHelperStoryCategoryId"].ToString();
	public string helperRoleId	= ConfigurationManager.AppSettings["helperRoleId"].ToString();
	public string volunteerRoleId	= ConfigurationManager.AppSettings["volunteerRoleId"].ToString();
	
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			LoadCategories();
			if(!String.IsNullOrEmpty(storySubject))
			{
				if(storySubject.ToLower() == "survivor")
				{
					LoadSurvivors();//Load survivors
				}
				else if(storySubject.ToLower() == "helper")
				{
					LoadHelpers();//Load helpers
				}
			}
			else
			{
				LoadSurvivors();//If no subject type is sent, then load survivors
			}
		}
		this.Master.HideCategoryList = true;

		//BEGIN FACEBOOK META TAGS
		Master.PageTitle		= "Create a New Stability Disaster Story";
		Master.FbURL			= Request.Url.AbsoluteUri;
		Master.FbDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.PageDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.FbImage			= "/S1/Images/CrowdReliefFBPost.jpg";
		Master.FbImageType		= "image/jpg";
		Master.FbSite_name		= "Stability Disaster Aid Platform";
		//END FACEBOOK META TAGS
	}

	protected void LoadCategories()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		//var qualifiers = from o in dc.RebuildStatus
		//				 where o.StatusType == 2
		//				 orderby o.OrderBy
		//				 select o;


		
		var categories = from c in dc.Categories
							where c.ParentCategory == "Survivor" && c.CategoryId != new Guid(disasterSurvivorStoryCategoryId) && c.CategoryId != new Guid(disasterHelperStoryCategoryId)
							orderby c.ParentCategory, c.Category1
							select new {c.Order, Category = c.Category1, c.CategoryId };

		ddlQualifiers.DataSource = categories;
		ddlQualifiers.DataBind();
		ddlQualifiers.Attributes.Add("multiple", "");
		ddlQualifiers.Attributes.Add("style", "width:100%;");
	}

	protected void LoadHelpers()
	{ 
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var helperProfiles = from p in dc.Profiles
							   join ur in dc.aspnet_UsersInRoles on p.UserId equals ur.UserId
								where ur.RoleId == new Guid(helperRoleId)
								|| ur.RoleId == new Guid(volunteerRoleId)
								select new {p};

		if(helperProfiles.Count() == 0)
		{
			//There is no survivor selected...
			Response.Redirect("AddNewSurvivor.aspx?e=AddSurvivor");
		}
		helperProfiles = helperProfiles.Distinct();
		helperProfiles = helperProfiles.OrderBy(x => x.p.Firstname);
		foreach(var helperProfile in helperProfiles)
		{
			userDropDown = userDropDown + "<li id=\"" + helperProfile.p.UserId + "\"><a href=\"#\">" + helperProfile.p.Firstname + " " + helperProfile.p.Lastname +  "</a></li>" + Environment.NewLine;
		}

		if(!String.IsNullOrEmpty(helperId))
		{
			//Hide the Dropdown and show the selected SURVIVOR
			var helperProfile = (from d in dc.UserUsers
							join p in dc.Profiles on d.RequestingUserId equals p.UserId
							where d.RequestingUserId == new Guid(survivorId)
							&& d.IsActive == true
							select new {p}).SingleOrDefault();
			
			preselectedHelperJQuery = "$(\"#btn-dropdown.helperEvent\").html('" + helperProfile.p.Firstname + " " + helperProfile.p.Lastname + "');";
			hidHelperId.Value = helperId;
		}
	}

	protected void LoadSurvivors()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var survivorProfiles = from uu in dc.UserUsers
								join p in dc.Profiles on uu.AcceptingUserId equals p.UserId
								where uu.RequestingUserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								orderby p.Lastname
								select new {p};

		if(survivorProfiles.Count() == 0)
		{
			//There is no survivor selected...
			Response.Redirect("AddNewSurvivor.aspx?e=AddSurvivor");
		}

		foreach(var survivorProfile in survivorProfiles)
		{
			userDropDown = userDropDown + "<li id=\"" + survivorProfile.p.UserId + "\"><a href=\"#\">" + survivorProfile.p.Firstname + " " + survivorProfile.p.Lastname +  "</a></li>" + Environment.NewLine;
		}

		if(!String.IsNullOrEmpty(survivorId))
		{
			//Hide the Dropdown and show the selected SURVIVOR
			var survivorProfile = (from d in dc.UserUsers
							join p in dc.Profiles on d.AcceptingUserId equals p.UserId
							where d.AcceptingUserId == new Guid(survivorId)
							&& d.RequestingUserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
							&& d.IsActive == true
							select new {p}).Take(1).SingleOrDefault();
			
			preselectedSurvivorJQuery = "$(\"#btn-dropdown.survivorEvent\").html('" + survivorProfile.p.Firstname + " " + survivorProfile.p.Lastname + "');";
			hidSurvivorId.Value = survivorId;
		}
	}

	protected void btnSubmitArticle_Click(object sender, EventArgs e)
	{
		Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		Article article = new Article();
		Guid articleId = Guid.NewGuid();
		
		string defaultStoryCategoryId = disasterSurvivorStoryCategoryId;
		if(storySubject.ToLower() == "helper")
		{
			defaultStoryCategoryId = disasterHelperStoryCategoryId;
		}

		if(!string.IsNullOrEmpty(hidSurvivorId.Value))
		{
			article.UserId = new Guid(hidSurvivorId.Value);
		}

		if(!string.IsNullOrEmpty(hidHelperId.Value))
		{
			article.UserId = new Guid(hidHelperId.Value);
		}

		var userEventid = (from ue in dc.UserEvents
						  where ue.UserId == new Guid(hidSurvivorId.Value)
						  select (ue.EventId)).Take(1).SingleOrDefault();

		article.CategoryId = new Guid(defaultStoryCategoryId); //Stories Id in category table.
		article.Title = txtTitle.Value;
		//txtArticle.Text = txtArticle.Text.Replace("<p ","#POPEN#<p ").Replace("</p>","#PCLOSE#");
		//txtArticle.Text = StripHTML(txtArticle.Text);
		article.Text = txtArticle.Text;//.Replace("#POPEN#","<p>").Replace("#PCLOSE#","</p>");
		article.CreatedBy = userId;
		article.CreatedOn = DateTime.Now;
			
		Random rnd = new Random();
		int views = rnd.Next(100, 200);

		article.EventId = userEventid;
		article.URLFriendlyTitle = txtTitle.Value.ToLower().Replace(" as "," ").Replace(" yet "," ").Replace(" nor "," ").Replace(" for "," ").Replace(" was "," ").Replace(" in "," ").Replace(" be "," ").Replace(" are "," ").Replace(" my "," ").Replace(" is "," ").Replace(" and "," ").Replace(" or "," ").Replace(" ","-");
		article.ArticleId = articleId;
		article.ViewCount = views;
		
		dc.Articles.InsertOnSubmit(article);
		dc.SubmitChanges();

		//Get this users disasterId
		//All articles get put into either the helper or survivor category.

		ArticleCategory articleCategory = new ArticleCategory();
		articleCategory.ArticleId = articleId;
		articleCategory.CategoryId = new Guid(defaultStoryCategoryId);
		articleCategory.ArticleCategoryId = Guid.NewGuid();
		dc.ArticleCategories.InsertOnSubmit(articleCategory);
		dc.SubmitChanges();

		foreach (ListItem listItem in ddlQualifiers.Items)
		{
			if (listItem.Selected == true)
			{
				Guid categoryId = new Guid(listItem.Value);

				ArticleCategory articleCategory2 = new ArticleCategory();
				articleCategory2.ArticleId = articleId;
				articleCategory2.CategoryId = categoryId;
				articleCategory2.ArticleCategoryId = Guid.NewGuid();
				dc.ArticleCategories.InsertOnSubmit(articleCategory2);
				dc.SubmitChanges();
			}
		}

		string redirect = "/S1/Default.aspx?userid=" + userId;
		if(chkUploadPhoto.Checked)
		{
			redirect = "/S1/Profile/BlogUploadPhoto.aspx?userId=" + userId + "&articleId=" + articleId;
		}

		divAlertArticleMessage.Visible = true;
		Response.Redirect(redirect);
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