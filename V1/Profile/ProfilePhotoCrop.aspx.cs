using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;

public partial class V1_Profile_ProfilePhotoCrop : BaseOrganizationWebForm
{
	protected string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	protected void Page_Load(object sender, EventArgs e)
	{
		string imageNameResized =  Request.QueryString["imageNameResized"];
		imgProfilePhoto.ImageUrl = profilePhotoFolder + imageNameResized;
	}
	
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			string imageNameResized			=  Request.QueryString["imageNameResized"];
			string imageNameCropped			= imageNameResized.Replace("_resized","_crop");
			string imageFileFolder			= Server.MapPath(profilePhotoFolder);
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
			
			if(User.IsInRole("survivor"))
			{
				Response.Redirect("~/S1/Profile/Default.aspx");
			}
			else
			{
				Response.Redirect("/V1/Member/Default.aspx");
			}
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