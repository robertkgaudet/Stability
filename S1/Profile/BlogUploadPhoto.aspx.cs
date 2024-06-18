using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

public partial class S1_Profile_BlogUploadPhoto : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.HideCategoryList = true;
		Master.PageTitle		= "Upload a CrowdReleif Disaster Story Photo";
		Master.FbURL			= Request.Url.AbsoluteUri;
		Master.FbDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.PageDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.FbImage			= "/S1/Images/CrowdReliefFBPost.jpg";
		Master.FbImageType		= "image/jpg";
		Master.FbSite_name		= "Stability Disaster Aid Platform";

	}
	
	public void btnUpdate_Click(object sender, EventArgs e)
	{
		string storyPhotoFolder		= System.Configuration.ConfigurationManager.AppSettings["storyPhotoFolder"].ToString();
		int storyImageWidth			= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["storyImageWidth"].ToString());
		int storyImageHeight		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["storyImageHeight"].ToString());
		string articleId			= Request.QueryString["articleId"];
		
		try
		{
			if (storyPhotoUpload.PostedFile.ContentLength > 0)
			{
				try
				{
					if (storyPhotoUpload.PostedFile.ContentType == "image/jpeg" || storyPhotoUpload.PostedFile.ContentType == "image/png")
					{
						if (storyPhotoUpload.PostedFile.ContentLength < 5242880)
						{
							string imageGuid				= Guid.NewGuid().ToString();
							string imageFileFolder			= Server.MapPath(storyPhotoFolder);
							string imageExtension			= Path.GetExtension(storyPhotoUpload.PostedFile.FileName);;

							string imageNameOriginal		= imageGuid + imageExtension;
							string imageNameCropped			= imageGuid + "_crop" + imageExtension;
							string imageNameResized			= imageGuid + "_resized" + imageExtension;

							string filePathnameOriginal		= Path.Combine(imageFileFolder, imageNameOriginal);
							string filePathNameCropped		= Path.Combine(imageFileFolder, imageNameCropped);
							string filePathNameResized		= Path.Combine(imageFileFolder, imageNameResized);
							
							storyPhotoUpload.PostedFile.SaveAs(filePathnameOriginal);
							
							CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

							Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
 
							//Insert the image.
							Guid photoId					= Guid.NewGuid();
							Photo photo						= new Photo();
							photo.Filename					= imageNameOriginal;
							photo.Title						= "Story Photo";	
							photo.Description				= "Story Photo";
							photo.PhotoId					= photoId;
							photo.FilenameCropped			= imageNameCropped;
							photo.FilenameResized			= imageNameResized;
							photo.Hidden					= false;
							photo.CreatedOn					= DateTime.Now;
							photo.CreatedBy					= userId;
							dc.Photos.InsertOnSubmit(photo);
							dc.SubmitChanges();
							
							ArticlePhoto articlePhoto		= new ArticlePhoto();
							articlePhoto.IsHeader			= true;
							articlePhoto.PhotoId			= photoId;
							articlePhoto.CreatedBy			= userId;
							articlePhoto.CreatedOn			= DateTime.Now;
							articlePhoto.ArticleId			= new Guid(articleId);
							articlePhoto.ArticlePhotoId		= Guid.NewGuid();
							dc.ArticlePhotos.InsertOnSubmit(articlePhoto);
							dc.SubmitChanges();

							ResizeAndSaveImage(filePathnameOriginal, filePathNameResized, storyImageWidth, storyImageHeight);
							
							var articles = (from a in dc.Articles
										   where a.ArticleId == new Guid(articleId)
										   select new {a.ArticleNumber }).SingleOrDefault();

							Response.Redirect("~/S1/Profile/StoryPhotoCrop.aspx?articleNumber=" + articles.ArticleNumber + "&articleId=" + articleId + "&imageNameResized=" + imageNameResized);
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
	}
	
	protected void ResizeAndSaveImage(string imageFilenamePathOriginal, string imageFilenamePathFinal, int maxWidth, int maxHeight)
	{
		//Get an image object of the newly uploaded file.
		System.Drawing.Image image = System.Drawing.Image.FromFile(imageFilenamePathOriginal);

		if(image.Width < maxWidth && image.Height < maxHeight)
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
}