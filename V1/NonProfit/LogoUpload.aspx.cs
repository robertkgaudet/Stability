using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;

public partial class V1_NonProfit_LogoUpload : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}
	
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		string logoFolder		= System.Configuration.ConfigurationManager.AppSettings["logoFolder"].ToString();
		int logoImageWidth		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["logoImageWidth"].ToString());
		int logoImageHeight		= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["logoImageHeight"].ToString());
		string organizationId	= Request.QueryString["organizationId"];

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
							string imageFileFolder			= Server.MapPath(logoFolder);
							string imageExtension			= Path.GetExtension(profilePhotoUpload.PostedFile.FileName);;

							string imageNameOriginal		= imageGuid + imageExtension;

							string filePathnameOriginal		= Path.Combine(imageFileFolder, imageNameOriginal);
							
							profilePhotoUpload.PostedFile.SaveAs(filePathnameOriginal);

							CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

							var organization = (from o in dc.Organizations
												where o.OrganizationId == new Guid(organizationId)
												select o).SingleOrDefault();

							//Insert the image.
							organization.Logo = imageNameOriginal;
							dc.SubmitChanges();

							//ResizeAndSaveImage(filePathnameOriginal, filePathNameResized, logoImageWidth, logoImageHeight);

							Response.Redirect("~/V1/NonProfit/Default.aspx?organizationId=" + organizationId);
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
	
	//protected void ResizeAndSaveImage(string imageFilenamePathOriginal, string imageFilenamePathFinal, int maxWidth, int maxHeight)
	//{
	//	//Get an image object of the newly uploaded file.
	//	System.Drawing.Image image = System.Drawing.Image.FromFile(imageFilenamePathOriginal);

	//	if(image.Width < maxWidth && image.Height < maxHeight)
	//	{
	//		maxWidth = image.Width;
	//		maxHeight = image.Height;
	//	}
		
	//	var ratioX = (double)maxWidth / image.Width;
	//	var ratioY = (double)maxHeight / image.Height;
	//	var ratio = Math.Min(ratioX, ratioY);
	//	var newWidth = (int)(image.Width * ratio);
	//	var newHeight = (int)(image.Height * ratio);

		
	//	//Create a copy of the image with 
	//	var newImage = new Bitmap(newWidth, newHeight);

	//	//Size the image down, create a new resized image.
	//	Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

	//	//Convert to a bitmap
	//	Bitmap bitmapImage = new Bitmap(newImage);

	//	//Save the new image
	//	bitmapImage.Save(imageFilenamePathFinal);
	//}
}