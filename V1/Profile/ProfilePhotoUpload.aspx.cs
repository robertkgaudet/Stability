using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;

public partial class V1_Profile_ProfilePhotoUpload : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}
	
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		int profileImageWidth		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["profileImageWidth"].ToString());
		int profileImageHeight		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["profileImageHeight"].ToString());
		
		try
		{
			if (profilePhotoUpload.PostedFile.ContentLength > 0)
			{
				try
				{
					if (profilePhotoUpload.PostedFile.ContentType == "image/jpeg" || profilePhotoUpload.PostedFile.ContentType == "image/png")
					{
						if (profilePhotoUpload.PostedFile.ContentLength < 5242880)
						{
							string imageGuid				= Guid.NewGuid().ToString();
							string imageFileFolder			= Server.MapPath(profilePhotoFolder);
							string imageExtension			= Path.GetExtension(profilePhotoUpload.PostedFile.FileName);;

							string imageNameOriginal		= imageGuid + imageExtension;
							string imageNameCropped			= imageGuid + "_crop" + imageExtension;
							string imageNameResized			= imageGuid + "_resized" + imageExtension;

							string filePathnameOriginal		= Path.Combine(imageFileFolder, imageNameOriginal);
							string filePathNameCropped		= Path.Combine(imageFileFolder, imageNameCropped);
							string filePathNameResized		= Path.Combine(imageFileFolder, imageNameResized);
							
							profilePhotoUpload.PostedFile.SaveAs(filePathnameOriginal);

							CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

							//Insert the image.
							Guid photoId					= Guid.NewGuid();
							Photo photo						= new Photo();
							photo.Filename					= imageNameOriginal;
							photo.Title						= "Profile Photo";	
							photo.Description				= "Profile Photo";
							photo.PhotoId					= photoId;
							photo.FilenameCropped			= imageNameCropped;
							photo.FilenameResized			= imageNameResized;
							photo.Hidden					= false;
							photo.CreatedOn					= DateTime.Now;
							photo.CreatedBy					= new Guid(Membership.GetUser().ProviderUserKey.ToString());
							dc.Photos.InsertOnSubmit(photo);
							dc.SubmitChanges();
							
							ProfilePhoto profilePhoto		= new ProfilePhoto();
							profilePhoto.IsCurrrent			= true;
							profilePhoto.PhotoId			= photoId;
							profilePhoto.UserId				= userId;
							profilePhoto.ProfilePhotoId		= Guid.NewGuid();
							dc.ProfilePhotos.InsertOnSubmit(profilePhoto);
							dc.SubmitChanges();

							ResizeAndSaveImage(filePathnameOriginal, filePathNameResized, profileImageWidth, profileImageHeight);

							Response.Redirect("~/V1/Profile/ProfilePhotoCrop.aspx?imageNameResized=" + imageNameResized);
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