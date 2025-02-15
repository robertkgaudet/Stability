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
	protected string logoFolder_squareLogoFolder = System.Configuration.ConfigurationManager.AppSettings["logoFolder"].ToString();
	protected void Page_Load(object sender, EventArgs e)
	{
        string imageNameResized = Request.QueryString["imageNameResized"];
        string imageType = Request.QueryString["imageType"].ToLower().ToString();

        if (!string.IsNullOrEmpty(imageType))
        {
            switch (imageType)
            {
                case "profile":
                    imgProfilePhoto.ImageUrl = profilePhotoFolder + imageNameResized;
                    lbltext.InnerText = "Crop Your Profile Photo";
                    break;

                case "logo":
                    imgProfilePhoto.ImageUrl = logoFolder_squareLogoFolder + imageNameResized;
                    lbltext.InnerText = "Crop Your Logo";
                    break;

                case "squarelogo":
                    imgProfilePhoto.ImageUrl = logoFolder_squareLogoFolder + imageNameResized;
                    lbltext.InnerText = "Crop Your Square Logo";
                    break;
            }
        }



    }

    protected void btnUpdate_Click(object sender, EventArgs e)
	{
		string imageFileFolder = string.Empty;

        try
		{
			string imageNameResized			=  Request.QueryString["imageNameResized"];
			string imageType = Request.QueryString["imageType"].ToLower().ToString();
			string imageNameCropped			= imageNameResized.Replace("_resized","_crop");
            switch (imageType)
            {
                case "profile":
                    imageFileFolder = Server.MapPath(profilePhotoFolder);
                    break;

                case "logo":
                    imageFileFolder = Server.MapPath(logoFolder_squareLogoFolder);
                    break;

                case "squarelogo":
                    imageFileFolder = Server.MapPath(logoFolder_squareLogoFolder);
                    break;
            }
            
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

            switch (imageType)
            {
                case "profile":
                    if (User.IsInRole("survivor"))
                    {
                        Response.Redirect("~/S1/Profile/Default.aspx");
                    }
                    else
                    {
                        Response.Redirect("/V1/Member/Default.aspx");
                    }
                    break;

                case "logo":
					Response.Redirect("/V1/NonProfit/Default.aspx");
                    break;

                case "squarelogo":
                    Response.Redirect("/V1/NonProfit/Default.aspx");
                    break;
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