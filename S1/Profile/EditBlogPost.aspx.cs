using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class S1_Profile_EditBlogPost : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string userId = Request.QueryString["userId"];
		string articleId = Request.QueryString["articleId"];
		
		Guid loggedInUserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

		if(!IsPostBack)
		{
			if(User.IsInRole("Administrator") || new Guid(userId) == loggedInUserId)
			{
				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

				var article = (from a in dc.Articles
							  where a.ArticleId == new Guid(articleId)
							  select a).SingleOrDefault();

				txtTitle.Value = article.Title;
				txtArticle.Text = article.Text;
			}
		}
		this.Master.HideCategoryList = true;
		Master.PageTitle		= "Edit a CrowdReleif Disaster Survivor Story";
		Master.FbURL			= Request.Url.AbsoluteUri;
		Master.FbDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.PageDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.FbImage			= "/S1/Images/CrowdReliefFBPost.jpg";
		Master.FbImageType		= "image/jpg";
		Master.FbSite_name		= "Stability Disaster Aid Platform";
	}
	
	protected string StripHTML(string input)
	{
		if (!string.IsNullOrEmpty(input))
		{
			input = Regex.Replace(input, "<.*?>", String.Empty);
		}
		return input;
	}

	protected void btnSubmitArticle_Click(object sender, EventArgs e)
	{
		Guid articleId = new Guid(Request.QueryString["articleId"]);
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		
		var article = (from a in dc.Articles
						where a.ArticleId == articleId
						select a).SingleOrDefault();
		
		article.Title = txtTitle.Value;

		//txtArticle.Text = txtArticle.Text.Replace("<p ","#POPEN#<p ").Replace("</p>","#PCLOSE#");
		//txtArticle.Text = StripHTML(txtArticle.Text);
		article.Text = txtArticle.Text;//"<p>" + txtArticle.Text.Replace("#POPEN#","<p>").Replace("#PCLOSE#","</p>");
		
		dc.SubmitChanges();
		Response.Redirect("/a/" + article.ArticleNumber);
	}
}