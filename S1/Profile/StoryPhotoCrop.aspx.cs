using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;

public partial class S1_Profile_StoryPhotoCrop : BaseOrganizationWebForm
{
	protected string storyPhotoFolder = System.Configuration.ConfigurationManager.AppSettings["storyPhotoFolder"].ToString();
	protected void Page_Load(object sender, EventArgs e)
	{
		string imageNameResized =  Request.QueryString["imageNameResized"];
		imgProfilePhoto.ImageUrl = storyPhotoFolder + imageNameResized;

		
		Master.PageTitle		= "Crop a CrowdReleif Disaster Story Photo";
		Master.FbURL			= Request.Url.AbsoluteUri;
		Master.FbDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.PageDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.FbImage			= "/S1/Images/CrowdReliefFBPost.jpg";
		Master.FbImageType		= "image/jpg";
		Master.FbSite_name		= "Stability Disaster Aid Platform";
	}
	
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		string articleNumber = Request.QueryString["articleNumber"];
		try
		{
			string imageNameResized			=  Request.QueryString["imageNameResized"];
			string imageNameCropped			= imageNameResized.Replace("_resized","_crop");
			string imageFileFolder			= Server.MapPath(storyPhotoFolder);
			string filePathNameResized		= Path.Combine(imageFileFolder, imageNameResized);
			string filePathNameCropped		= Path.Combine(imageFileFolder, imageNameCropped);

			//CROP THE IMAGE
			int w							= Convert.ToInt32(decimal.Parse(W.Value));
			int h							= Convert.ToInt32(decimal.Parse(H.Value));
			int x							= Convert.ToInt32(decimal.Parse(X.Value));
			int y							= Convert.ToInt32(decimal.Parse(Y.Value));

			//Crop the resized image
			byte[] CropImage = Crop(filePathNameResized, w, h, x, y); //CROP RESIZED IMAGE

			using (MemoryStream ms = new MemoryStream(CropImage, 0, CropImage.Length))
			{
				ms.Write(CropImage, 0, CropImage.Length);

				using(SD.Image CroppedImage = SD.Image.FromStream(ms, true))
				{
					//Save the new cropped image
					CroppedImage.Save(filePathNameCropped, CroppedImage.RawFormat);
				}
			}
			
			Response.Redirect("~/a/" + articleNumber);
		}
		catch (Exception ex)
		{
			Response.Write(ex.Message);
			Response.End();
		}
	}

	static byte[] Crop(string Img, int Width, int Height, int X, int Y)
	{
		try
		{
			using (SD.Image OriginalImage = SD.Image.FromFile(Img))
			{
				using (SD.Bitmap bmp = new SD.Bitmap(Width, Height))
				{
					bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
					using (SD.Graphics Graphic = SD.Graphics.FromImage(bmp))
					{
						Graphic.SmoothingMode = SmoothingMode.AntiAlias;
						Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
						Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
						Graphic.DrawImage(OriginalImage, new SD.Rectangle(0, 0, Width, Height), X, Y, Width, Height, SD.GraphicsUnit.Pixel);
						MemoryStream ms = new MemoryStream();
						bmp.Save(ms, OriginalImage.RawFormat);
						return ms.GetBuffer();
					}
				}
			}
		}
		catch (Exception Ex)
		{
			throw (Ex);
		}
	}
}