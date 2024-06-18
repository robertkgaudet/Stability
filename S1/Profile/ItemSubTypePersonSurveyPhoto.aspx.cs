using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web.UI;
using System.Web;

public partial class S1_Profile_ItemSubTypePersonSurveyPhoto : BaseOrganizationWebForm
{
    string surveyPhotoFolder = System.Configuration.ConfigurationManager.AppSettings["surveyPhotoFolder"].ToString();
    string surveyUserId = HttpContext.Current.Request.QueryString["userId"];
    string itemSubTypePersonSurveyId = HttpContext.Current.Request.QueryString["ItemSubTypePersonSurveyId"];
    string deleteItemSubTypePersonSurveyPhotoId = HttpContext.Current.Request.QueryString["ItemSubTypePersonSurveyPhotoId"];
    string deletePhotoId = HttpContext.Current.Request.QueryString["PhotoId"];
    string delete = HttpContext.Current.Request.QueryString["delete"];

    protected void Page_Load(object sender, EventArgs e)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(delete) && !String.IsNullOrEmpty(deleteItemSubTypePersonSurveyPhotoId) && !String.IsNullOrEmpty(deletePhotoId))
            {
                //Get the photo and the surveyPhoto record and delete them.
                var surveyPhoto = (from ispsp in dc.ItemSubTypePersonSurveyPhotos
                                   where ispsp.ItemSubTypePersonSurveyPhotoId == new Guid(deleteItemSubTypePersonSurveyPhotoId)
                                   select ispsp).SingleOrDefault();

                dc.ItemSubTypePersonSurveyPhotos.DeleteOnSubmit(surveyPhoto);
                dc.SubmitChanges();

                var photo = (from p in dc.Photos
                             where p.PhotoId == new Guid(deletePhotoId)
                             select p).SingleOrDefault();

                dc.Photos.DeleteOnSubmit(photo);
                dc.SubmitChanges();
            }
        }

        if (string.IsNullOrEmpty(itemSubTypePersonSurveyId) && !String.IsNullOrEmpty(surveyUserId))
        {
            //Get the surveyPhotoId
            var surveyPhotoId = (from survey in dc.ItemSubTypePersonSurveys
                                 where survey.UserId == new Guid(surveyUserId)
                                 select new { survey.ItemSubTypePersonSurveyId }).SingleOrDefault();
            if (surveyPhotoId != null)
            {
                itemSubTypePersonSurveyId = surveyPhotoId.ItemSubTypePersonSurveyId.ToString();
            }
        }

        if (!string.IsNullOrEmpty(itemSubTypePersonSurveyId))
        {
            LoadPhotos(new Guid(itemSubTypePersonSurveyId));
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int profileImageWidth = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["itemImageWidth"].ToString());
        int profileImageHeight = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["itemImageHeight"].ToString());

        try
        {
            if (itemPhotoUpload.PostedFile.ContentLength > 0)
            {
                try
                {
                    if (itemPhotoUpload.PostedFile.ContentType == "image/jpeg" || itemPhotoUpload.PostedFile.ContentType == "image/png")
                    {
                        if (itemPhotoUpload.PostedFile.ContentLength < 50242880)
                        {
                            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

                            string imageGuid = Guid.NewGuid().ToString();
                            string imageFileFolder = Server.MapPath(surveyPhotoFolder);
                            string imageExtension = Path.GetExtension(itemPhotoUpload.PostedFile.FileName); ;

                            string imageNameOriginal = imageGuid + imageExtension;
                            string imageNameCropped = imageGuid + "_crop" + imageExtension;
                            string imageNameResized = imageGuid + "_resized" + imageExtension;

                            string filePathnameOriginal = Path.Combine(imageFileFolder, imageNameOriginal);
                            string filePathNameCropped = Path.Combine(imageFileFolder, imageNameCropped);
                            string filePathNameResized = Path.Combine(imageFileFolder, imageNameResized);

                            itemPhotoUpload.PostedFile.SaveAs(filePathnameOriginal);

                            //Insert the image.
                            Guid photoId = Guid.NewGuid();

                            Photo photo = new Photo();
                            photo.Filename = imageNameOriginal;
                            photo.Title = txtTitle.Text;
                            photo.Description = txtDescription.Text;
                            photo.PhotoId = photoId;
                            photo.FilenameCropped = imageNameCropped;
                            photo.FilenameResized = imageNameResized;
                            photo.Hidden = false;
                            photo.CreatedOn = DateTime.Now;
                            photo.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                            dc.Photos.InsertOnSubmit(photo);
                            dc.SubmitChanges();
                            
                            ItemSubTypePersonSurveyPhoto itemSubTypePersonSurveyPhoto = new ItemSubTypePersonSurveyPhoto();
                            itemSubTypePersonSurveyPhoto.IsActive = true;
                            itemSubTypePersonSurveyPhoto.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                            itemSubTypePersonSurveyPhoto.CreatedOn = DateTime.Now;
                            itemSubTypePersonSurveyPhoto.Description = txtDescription.Text;
                            itemSubTypePersonSurveyPhoto.ItemSubTypePersonSurveyId = new Guid(itemSubTypePersonSurveyId);
                            itemSubTypePersonSurveyPhoto.ItemSubTypePersonSurveyPhotoId = Guid.NewGuid();
                            itemSubTypePersonSurveyPhoto.PhotoId = photoId;
                            itemSubTypePersonSurveyPhoto.Title = txtTitle.Text;
                            dc.ItemSubTypePersonSurveyPhotos.InsertOnSubmit(itemSubTypePersonSurveyPhoto);
                            dc.SubmitChanges();

                            ResizeAndSaveImage(filePathnameOriginal, filePathNameResized, profileImageWidth, profileImageHeight);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    Response.End();
                }
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            Response.End();
        }

        //LoadPhotos(new Guid(itemSubTypePersonSurveyId));
        Response.Redirect("~/S1/Profile/ItemSubTypePersonSurveyPhoto.aspx?itemSubTypePersonSurveyId=" + itemSubTypePersonSurveyId);
    }

    protected void LoadPhotos(Guid itemSubTypePersonSurveyId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var photos = from p in dc.Photos
                     join istpp in dc.ItemSubTypePersonSurveyPhotos on p.PhotoId equals istpp.PhotoId
                     where istpp.ItemSubTypePersonSurveyId == itemSubTypePersonSurveyId
                     orderby p.CreatedOn descending
                     select new { p.FilenameResized, istpp.Description, istpp.Title, p.PhotoId, istpp.ItemSubTypePersonSurveyPhotoId };

        rptPhotos.DataSource = photos;
        rptPhotos.DataBind();
    }

    protected void ResizeAndSaveImage(string imageFilenamePathOriginal, string imageFilenamePathFinal, int maxWidth, int maxHeight)
    {
        //Get an image object of the newly uploaded file.
        System.Drawing.Image image = System.Drawing.Image.FromFile(imageFilenamePathOriginal);

        if (image.Width < maxWidth && image.Height < maxHeight)
        {
            maxWidth = image.Width;
            maxHeight = image.Height;
        }

        var ratioX = (double)maxWidth / image.Width;
        var ratioY = (double)maxHeight / image.Height;
        var ratio = Math.Min(ratioX, ratioY);
        var newWidth = (int)(image.Width * ratio);
        var newHeight = (int)(image.Height * ratio);


        //Create a copy of the image with 
        var newImage = new Bitmap(newWidth, newHeight);

        //Size the image down, create a new resized image.
        Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

        //Convert to a bitmap
        Bitmap bitmapImage = new Bitmap(newImage);

        //Save the new image
        bitmapImage.Save(imageFilenamePathFinal);
    }

    protected void rptPhotos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            RepeaterItem dataItem = (RepeaterItem)e.Item;
            System.Web.UI.WebControls.Image imgPhoto = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgPhoto");
            Literal litTitle = (Literal)e.Item.FindControl("litTitle");
            Label lblDescription = (Label)e.Item.FindControl("lblDescription");
            HyperLink hypDelete = (HyperLink)e.Item.FindControl("hypDelete");

            string description = (string)DataBinder.Eval(dataItem.DataItem, "Description");
            string title = (string)DataBinder.Eval(dataItem.DataItem, "Title");
            string filenameResized = (string)DataBinder.Eval(dataItem.DataItem, "FilenameResized");
            Guid photoId = (Guid)DataBinder.Eval(dataItem.DataItem, "photoId");
            Guid ItemSubTypePersonSurveyPhotoId = (Guid)DataBinder.Eval(dataItem.DataItem, "ItemSubTypePersonSurveyPhotoId");

            hypDelete.NavigateUrl = "?itemSubTypePersonSurveyId=" + itemSubTypePersonSurveyId + "&delete=true&ItemSubTypePersonSurveyPhotoId=" + ItemSubTypePersonSurveyPhotoId + "&photoId=" + photoId;
            imgPhoto.ImageUrl = surveyPhotoFolder + filenameResized;
            lblDescription.Text = description;
            litTitle.Text = title;
        }
    }
}