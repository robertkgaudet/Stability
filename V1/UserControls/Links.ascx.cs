using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

//public class LinkCategoryParentCategory	
//{
//	public String Link { get; set; }
//	public String Title { get; set; }
//	public String Description { get; set; }
//	public String parentCateogry { get; set; }
//	public String Category { get; set; }
//}

public partial class V1_UserControls_Links : System.Web.UI.UserControl
{
	Guid	_eventId		= Guid.Empty;
	string	_categoryName	= string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			LoadCategories();
		}
	}

	public void LoadCategories()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		IEnumerable<Category> linkCategories = from c in dc.Categories
							 join l in dc.Links on c.CategoryId equals l.CategoryId
							 where l.EventId == _eventId
							 orderby c.ParentCategory, c.Category1
							 select c;

		//IEnumerable<Category> articleCategories =	from c in dc.Categories
		//							join l in dc.Articles on c.CategoryId equals l.CategoryId
		//							where l.EventId == _eventId
		//							select c;

		IEnumerable<Category> categories = linkCategories.Distinct();//linkCategories.Union(articleCategories);

		rptCategories.DataSource = categories;
		rptCategories.DataBind();
	}
	
	//Get the links for this category.
	protected IOrderedQueryable<Link> LoadLinks(Guid eventId, Guid categoryId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var eventLinks =  from l in dc.Links
							where l.EventId == eventId
							&& l.CategoryId == categoryId
							orderby l.Title
							select l;

		return eventLinks;
	}

	//Get the links for this category.
	protected IOrderedQueryable<Article> LoadArticles(Guid eventId, Guid categoryId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var eventArticles =  from l in dc.Articles
							where l.EventId == eventId
							&& l.CategoryId == categoryId
							orderby l.CategoryId
							select l;

		return eventArticles;
	}

	protected void rptLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			String title			= (String)DataBinder.Eval(dataItem.DataItem, "Title");
			String description		= (String)DataBinder.Eval(dataItem.DataItem, "Description");
			String url				= (String)DataBinder.Eval(dataItem.DataItem, "url");
			Guid categoryId			= (Guid)DataBinder.Eval(dataItem.DataItem, "categoryId");
			Guid linkId			= (Guid)DataBinder.Eval(dataItem.DataItem, "linkId");

			Label lblDescription = (Label)e.Item.FindControl("lblDescription");
			HyperLink hypLinkText = (HyperLink)e.Item.FindControl("hypLinkText");
			LinkButton lbDeleteLink = (LinkButton)e.Item.FindControl("lbDeleteLink");
			HtmlGenericControl divDeleteLinks = (HtmlGenericControl)e.Item.FindControl("divDeleteLinks");

			if(HttpContext.Current.User.IsInRole("Administrator"))
			{
				divDeleteLinks.Visible = true;
			}

			lbDeleteLink.CommandArgument = linkId.ToString();
			hypLinkText.NavigateUrl = url;
			hypLinkText.Text = title;
			lblDescription.Text = description;
		}
	}

	protected void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			Guid categoryId				= (Guid)DataBinder.Eval(dataItem.DataItem, "categoryId");
			string category				= (string)DataBinder.Eval(dataItem.DataItem, "category1");
			string parentCategory		= (string)DataBinder.Eval(dataItem.DataItem, "parentCategory");

			Repeater rptLinks = e.Item.FindControl("rptLinks") as Repeater;
			rptLinks.DataSource = LoadLinks(_eventId, categoryId);
			rptLinks.DataBind();
			
			//Repeater rptArticles = e.Item.FindControl("rptArticles") as Repeater;
			//rptArticles.DataSource = LoadArticles(_eventId, categoryId);
			//rptArticles.DataBind();
			
			Literal litParentCategory	= (Literal)e.Item.FindControl("litParentCategory");
			litParentCategory.Text = parentCategory + " - " + category;
			//litParentCategory.Text = category;
		}
	}

	public Guid eventId
	{
		get { return _eventId; }
		set { _eventId = value; }
	}

	protected void repArticles_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			String title			= (String)DataBinder.Eval(dataItem.DataItem, "Title");
			String text				= (String)DataBinder.Eval(dataItem.DataItem, "Text");
			Guid articleId			= (Guid)DataBinder.Eval(dataItem.DataItem, "articleId");
			LinkButton lbDeleteArticle = (LinkButton)e.Item.FindControl("lbDeleteArticle");
			HtmlGenericControl divDeleteArticle = (HtmlGenericControl)e.Item.FindControl("divDeleteArticle");

			Label lblDescription = (Label)e.Item.FindControl("lblDescription");
			Label lblText = (Label)e.Item.FindControl("lblText");

			if(HttpContext.Current.User.IsInRole("Administrator"))
			{
				divDeleteArticle.Visible = true;
			}
			
			lbDeleteArticle.CommandArgument = articleId.ToString();
			lblText.Text = title;
			lblDescription.Text = text;
		}
	}

	protected void lbDeleteArticle_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		LinkButton linkButton = (LinkButton)sender;
		Guid articleId = new Guid(linkButton.CommandArgument);

		//Delete the article
		var article = (from a in dc.Articles
					  where a.ArticleId == articleId
					  select a).SingleOrDefault();

		dc.Articles.DeleteOnSubmit(article);
		dc.SubmitChanges();
		LoadCategories();

	}

	protected void lbDeleteLink_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		LinkButton linkButton = (LinkButton)sender;
		Guid linkId = new Guid(linkButton.CommandArgument);

		//Delete the link
		//Delete the article
		var link = (from a in dc.Links
					  where a.LinkId == linkId
					  select a).SingleOrDefault();

		dc.Links.DeleteOnSubmit(link);
		dc.SubmitChanges();
		LoadCategories();
	}
}