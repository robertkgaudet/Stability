using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

public partial class V1_Administration_Item_ItemPhotoUpload : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}
	
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["itemPhotoFolder"].ToString();
		int profileImageWidth		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["itemImageWidth"].ToString());
		int profileImageHeight		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["itemImageHeight"].ToString());
		
		try
		{
			if (itemPhotoUpload.PostedFile.ContentLength > 0)
			{
				try
				{
					if (itemPhotoUpload.PostedFile.ContentType == "image/jpeg" || itemPhotoUpload.PostedFile.ContentType == "image/png")
					{
						if (itemPhotoUpload.PostedFile.ContentLength < 5242880)
						{
                            string itemId                   = Request.QueryString["itemId"];
							string imageGuid				= Guid.NewGuid().ToString();
							string imageFileFolder			= Server.MapPath(profilePhotoFolder);
							string imageExtension			= Path.GetExtension(itemPhotoUpload.PostedFile.FileName);;

							string imageNameOriginal		= imageGuid + imageExtension;
							string imageNameCropped			= imageGuid + "_crop" + imageExtension;
							string imageNameResized			= imageGuid + "_resized" + imageExtension;

							string filePathnameOriginal		= Path.Combine(imageFileFolder, imageNameOriginal);
							string filePathNameCropped		= Path.Combine(imageFileFolder, imageNameCropped);
							string filePathNameResized		= Path.Combine(imageFileFolder, imageNameResized);

                            itemPhotoUpload.PostedFile.SaveAs(filePathnameOriginal);

							CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

                            var item = (from i in dc.Items
                                        where i.ItemId == new Guid(itemId)
                                        select i).SingleOrDefault();

                            //Insert the image.
                            Guid photoId					= Guid.NewGuid();
							Photo photo						= new Photo();
							photo.Filename					= imageNameOriginal;
							photo.Title						= item.Name;	
							photo.Description				= item.Description;
							photo.PhotoId					= photoId;
							photo.FilenameCropped			= imageNameCropped;
							photo.FilenameResized			= imageNameResized;
							photo.Hidden					= false;
							photo.CreatedOn					= DateTime.Now;
							photo.CreatedBy					= new Guid(Membership.GetUser().ProviderUserKey.ToString());
							dc.Photos.InsertOnSubmit(photo);
							dc.SubmitChanges();

                            item.PhotoId = photoId;
                            dc.SubmitChanges();

							ResizeAndSaveImage(filePathnameOriginal, filePathNameResized, profileImageWidth, profileImageHeight);

							Response.Redirect("~/V1/Administration/Item/List.aspx");
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