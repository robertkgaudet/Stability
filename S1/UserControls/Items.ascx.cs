using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class S1_UserControls_Items : System.Web.UI.UserControl
{
    public string gridItem = string.Empty;
    private Guid userId;
    private Guid itemSubTypePersonSurveyId;
    string productImagePlaceHolderAndPath = System.Configuration.ConfigurationManager.AppSettings["productImagePlaceHolderAndPath"].ToString();
    string productPhotoFolder = System.Configuration.ConfigurationManager.AppSettings["HouseholdItemsFolder"].ToString();
    string surveyPhotoFolder = System.Configuration.ConfigurationManager.AppSettings["surveyPhotoFolder"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        GetItems();
        userId = new Guid(Request.QueryString["userId"]);

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var profile = (from p in dc.Profiles
                       where p.UserId == userId
                       select p).SingleOrDefault();

        hypSurvivorProfilePage.NavigateUrl = "/sp/" + profile.ProfileNumber + "/" + profile.Firstname + "-" + profile.Lastname;
        hypSurvivorProfilePage.Text = "Survivor Profile";

        var itemSubTypePersonSurvey = (from istps in dc.ItemSubTypePersonSurveys
                                       where istps.UserId == userId
                                       select istps).SingleOrDefault();

        litPageTitle.Text = itemSubTypePersonSurvey.Title;
        lblDescription.Text = itemSubTypePersonSurvey.Description;
        LoadPhotos(itemSubTypePersonSurvey.ItemSubTypePersonSurveyId);
    }

    protected void LoadPhotos(Guid itemSubTypePersonSurveyId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var photos = from p in dc.Photos
                     join istpp in dc.ItemSubTypePersonSurveyPhotos on p.PhotoId equals istpp.PhotoId
                     where istpp.ItemSubTypePersonSurveyId == itemSubTypePersonSurveyId
                     orderby p.CreatedOn descending
                     select new { p.Filename, istpp.Description, istpp.Title, p.PhotoId, istpp.ItemSubTypePersonSurveyPhotoId };

        rptPhotos.DataSource = photos;
        rptPhotos.DataBind();
    }

    protected void rptPhotos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            RepeaterItem dataItem = (RepeaterItem)e.Item;
            System.Web.UI.WebControls.Image imgCaroselImage = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgCaroselImage");

            string description = (string)DataBinder.Eval(dataItem.DataItem, "Description");
            string title = (string)DataBinder.Eval(dataItem.DataItem, "Title");
            string filename = (string)DataBinder.Eval(dataItem.DataItem, "Filename");

            imgCaroselImage.ImageUrl = surveyPhotoFolder + filename;
        }
    }

    private void GetItems()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        //Gett he items based on subtypes selected for this person.
        userId = new Guid(Request.QueryString["userId"]);

        //Should be chosen based 
        //Get the wishlist for this person.
        //itemSubTypePersonSurveyId = new Guid(Request.QueryString["itemSubTypePersonSurveyId"]);
        var wishlist   = from istp in dc.ItemSubTypePersons
                           join i in dc.Items on istp.ItemSubTypeId equals i.ItemSubTypeId
                           where istp.UserId == userId && (i.IsDeleted == false || i.IsDeleted == null)
                         orderby i.PhotoId descending, i.CreatedOn descending
                           select new { i, istp };

        foreach(var item in wishlist)
        {

            string productImage = productImagePlaceHolderAndPath;
            var productImageResults = (from ph in dc.Photos
                                where ph.PhotoId == item.i.PhotoId
                                select new { ph.FilenameResized }).Take(1).SingleOrDefault();

            if (productImageResults != null)
            {
                //Get the users profile image
                productImage = productPhotoFolder + productImageResults.FilenameResized;
            }

            gridItem += Environment.NewLine + Environment.NewLine +
                "<!--BEGIN GRID ITEM-->" + Environment.NewLine +
                    "<div class=\"grid-item\">" + Environment.NewLine +
                        "<div class=\"hpanel blog-box\">" + Environment.NewLine +
                            "<div class=\"panel-heading\">" + Environment.NewLine +
                                "<div class=\" clearfix\">" + Environment.NewLine +
                                    "<a class=\"pull-left\">" + Environment.NewLine +
                                        "<img class=\"img-responsive\" style=\"height:250px;\" src=\"" + productImage + "\">" + Environment.NewLine +
                                    "</a>" + Environment.NewLine +
                                "</div>" + Environment.NewLine +
                            "</div>" + Environment.NewLine +
                            "<div class=\"panel-body\" style=\"height:100px;\">" + Environment.NewLine +
                                "<p>" + item.i.Name + "</p>" + Environment.NewLine + 
                            "</div>" + Environment.NewLine +
                            "<div class=\"panel-footer\">" + Environment.NewLine +
                                "<button class=\"btn btn-success btn-md\" style=\"width:100px;\"> Donate <i class=\"fa fa-shopping-cart\"> </i></button>" + Environment.NewLine +
                                "<h4 class=\"no-margins font-extra-bold text-success pull-right\">  $" + item.i.Price + "</h4>" + Environment.NewLine +

                            "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                    "</div>" + Environment.NewLine +
                "<!--END GRID ITEM-->" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;


        }
    }
}