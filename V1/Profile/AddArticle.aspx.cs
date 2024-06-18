using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_AddArticle : System.Web.UI.Page
{
	public Guid eventId = new Guid(HttpContext.Current.Request.QueryString["eventId"]);

	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			LoadLinkCategories();
		}
	}

	protected void LoadLinkCategories()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var categories = from c in dc.Categories
							orderby c.ParentCategory, c.Category1
							select new {c.Order, c.ParentCategory, category = c.ParentCategory + " - " + c.Category1, c.CategoryId };
		

		ddlInformationCategory.DataSource = categories;
		ddlInformationCategory.DataBind();
	}

	protected void btnSubmitArticle_Click(object sender, EventArgs e)
	{
		Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		Article article = new Article();

		article.CategoryId = new Guid(ddlInformationCategory.SelectedValue);
		article.Title = txtTitle.Value;
		article.Text = txtArticle.Text;
		article.CreatedBy = userId;
		article.CreatedOn = DateTime.Now;
		article.EventId = eventId;
		article.ArticleId = Guid.NewGuid();


		dc.Articles.InsertOnSubmit(article);
		dc.SubmitChanges();
		divAlertArticleMessage.Visible = true;
		litArticleMessage.Text = "New article added.";
	}
}