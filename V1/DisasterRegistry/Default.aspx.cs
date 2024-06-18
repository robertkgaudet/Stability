using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_DisasterRegistry_Default : System.Web.UI.Page
{
    public string itemPhotoFolder = System.Configuration.ConfigurationManager.AppSettings["itemPhotoFolder"].ToString();
    public string itemImagePlaceholderFilename = System.Configuration.ConfigurationManager.AppSettings["itemImagePlaceholderFilename"].ToString(); 

    protected void Page_Load(object sender, EventArgs e)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        if (!String.IsNullOrEmpty(Request.QueryString["URLFriendlyName"]))
        {
            string URLFriendlyName = Request.QueryString["URLFriendlyName"];

            Guid userId = (from i in dc.Items
                           where i.URLFriendlyName == URLFriendlyName
                           select i.ItemId).SingleOrDefault();

            LoadItem(userId);
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["itemId"]))
            {
                LoadItem(new Guid(Request.QueryString["itemId"]));
            }
        }
    }

    protected void LoadItem(Guid itemId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var item = (from i in dc.Items
                    join b in dc.Brands on i.BrandId equals b.BrandId
                    where i.ItemId == itemId
                    select new { b, i }).SingleOrDefault();

        if(item != null)
        {
            string itemPhotoURL = itemPhotoFolder + itemImagePlaceholderFilename;
            if (item.i.PhotoId != null && item.i.PhotoId != Guid.Empty)
            {
                var itemPhotoFilename = (from p in dc.Photos
                                where p.PhotoId == item.i.PhotoId
                                select new { p.FilenameResized }).SingleOrDefault();

                if(itemPhotoFilename != null)
                {
                    itemPhotoURL = itemPhotoFolder + itemPhotoFilename.FilenameResized;
                }
            }
            string itemName = item.i.Name;
            string itemDescription = item.i.Description;
            decimal? itemPrice = item.i.Price;
            Guid? photoId = item.i.PhotoId;
            string brandName = item.b.BrandName;


            string pageDescription = itemDescription;
            string pageTitle = "Buy " + itemName + " for Disaster Survivors";


            litDescription.Text = itemDescription;
            litItemName.Text = itemName;
            litBrand.Text = brandName;
            imgItemPhoto.ImageUrl = itemPhotoURL;
            litPrice.Text = "$" + itemPrice.ToString();



            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Administrator"))
                {
                    divAdminlinks.Visible = true;
                    hypEditItem.NavigateUrl = "/V1/Administration/Item/AddItem.aspx?itemId=" + itemId;
                }
            }


        }

        //this.Master.PageTitle = "Stability - " + pageTitle;
        //this.Master.PageDescription = "Stability - " + pageDescription;
        //this.Master.FbDescription = "Stability - " + pageDescription;
        //this.Master.FbImage = "/V1/Images/";
        //this.Master.FbImageType = "image/jpg";
        //this.Master.FbSite_name = "Stability - " + pageTitle;
        //this.Master.FbURL = Request.Url.AbsoluteUri;
    }
}