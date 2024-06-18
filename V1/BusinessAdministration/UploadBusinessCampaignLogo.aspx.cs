using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;

public partial class V1_BusinessAdministration_UploadBusinessCampaignLogo : BaseOrganizationWebForm
{
	public string businessEventId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{}
	
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		string businessPhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["businessPhotoFolder"].ToString();
		int businessImageWidth		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["businessImageWidth"].ToString());
		int businessImageHeight		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["businessImageHeight"].ToString());

		try
		{
			if (businessPhotoUpload.PostedFile.ContentLength > 0)
			{
				try
				{
					if (businessPhotoUpload.PostedFile.ContentType == "image/jpeg" || businessPhotoUpload.PostedFile.ContentType == "image/png")
					{
						if (businessPhotoUpload.PostedFile.ContentLength < 5242880)
						{
							string imageGuid				= Guid.NewGuid().ToString();
							string imageFileFolder			= Server.MapPath(businessPhotoFolder);
							string imageExtension			= Path.GetExtension(businessPhotoUpload.PostedFile.FileName);;

							string imageNameOriginal		= imageGuid + imageExtension;
							string imageNameCropped			= imageGuid + "_crop" + imageExtension;
							string imageNameResized			= imageGuid + "_resized" + imageExtension;

							string filePathnameOriginal		= Path.Combine(imageFileFolder, imageNameOriginal);
							string filePathNameCropped		= Path.Combine(imageFileFolder, imageNameCropped);
							string filePathNameResized		= Path.Combine(imageFileFolder, imageNameResized);
							
							businessPhotoUpload.PostedFile.SaveAs(filePathnameOriginal);

							CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
							string businessEventId = Request.QueryString["BusinessEventId"];
							var business = (from be in dc.BusinessEvents
											  where be.BusinessEventId == new Guid(businessEventId)
											  select new { be.BusinessId }).Take(1).SingleOrDefault();
							
							//Insert the image.
							Guid photoId					= Guid.NewGuid();
							Photo photo						= new Photo();
							photo.Filename					= imageNameOriginal;
							photo.Title						= "Business Photo";	
							photo.Description				= "Business Photo";
							photo.PhotoId					= photoId;
							photo.FilenameCropped			= imageNameCropped;
							photo.FilenameResized			= imageNameResized;
							photo.Hidden					= false;
							photo.CreatedOn					= DateTime.Now;
							photo.CreatedBy					= new Guid(Membership.GetUser().ProviderUserKey.ToString());
							dc.Photos.InsertOnSubmit(photo);
							dc.SubmitChanges();
							
							BusinessPhoto businessPhoto			= new BusinessPhoto();
							businessPhoto.IsCurrrent			= true;
							businessPhoto.PhotoId				= photoId;
							businessPhoto.BusinessEventId		= new Guid(businessEventId);
							businessPhoto.BusinessId			= business.BusinessId;
							businessPhoto.UserId				= userId;
							businessPhoto.BusinessPhotoId		= Guid.NewGuid();
							dc.BusinessPhotos.InsertOnSubmit(businessPhoto);
							dc.SubmitChanges();

							ResizeAndSaveImage(filePathnameOriginal, filePathNameResized, businessImageWidth, businessImageHeight);

							Response.Redirect("~/V1/BusinessAdministration/BusinessPhotoCrop.aspx?businessEventId=" + businessEventId + "&imageNameResized=" + imageNameResized);
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