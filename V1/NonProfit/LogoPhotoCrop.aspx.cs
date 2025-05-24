using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml;

public partial class V1_Logo_LogoPhotoCrop : BaseOrganizationWebForm
{
    protected string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
    protected string logoFolder_squareLogoFolder = System.Configuration.ConfigurationManager.AppSettings["logoFolder"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        string imageNameResized = Request.QueryString["imageNameResized"];

        if (!string.IsNullOrEmpty(imageNameResized))
        {
            // Set image URL only for logo
            imgLogoPhoto.ImageUrl = logoFolder_squareLogoFolder + imageNameResized;
            lbltext.InnerText = "Crop Your Logo";
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string imageNameResized = Request.QueryString["imageNameResized"];
            string imageNameCropped = imageNameResized.Replace("_resized", "_crop");

            // Define the folder path for logos only
            string imageFileFolder = Server.MapPath(logoFolder_squareLogoFolder);

            string filePathNameResized = Path.Combine(imageFileFolder, imageNameResized);
            string filePathNameCropped = Path.Combine(imageFileFolder, imageNameCropped);

            // CROP THE IMAGE
            int w = Convert.ToInt32(decimal.Parse(W.Value));
            int h = Convert.ToInt32(decimal.Parse(H.Value));
            int x = Convert.ToInt32(decimal.Parse(X.Value));
            int y = Convert.ToInt32(decimal.Parse(Y.Value));

            // Crop the resized image
            byte[] CropImage = Crop(filePathNameResized, w, h, x, y);

            using (MemoryStream ms = new MemoryStream(CropImage, 0, CropImage.Length))
            {
                ms.Write(CropImage, 0, CropImage.Length);
                using (System.Drawing.Image CroppedImage = System.Drawing.Image.FromStream(ms, true))
                {
                    // Save the new cropped image
                    CroppedImage.Save(filePathNameCropped, CroppedImage.RawFormat);
                }
            }

            using (var dc = new CrowdReliefDBDataContext())
            {
                // Get the current user's organization
                var userOrg = dc.UserOrganizations.FirstOrDefault(uo => uo.UserId == userId && uo.IsEnabled == true);
                if (userOrg != null)
                {
                    var organization = dc.Organizations.FirstOrDefault(o => o.OrganizationId == userOrg.OrganizationId);
                    if (organization != null)
                    {
                        // Save the cropped logo
                        organization.Logo = imageNameCropped;
                        dc.SubmitChanges();
                    }
                }
            }

            // Redirect after successful update
            Response.Redirect("/V1/NonProfit/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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