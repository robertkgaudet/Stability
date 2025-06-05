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

public partial class V1_SquareLogo_SquareLogoPhotoCrop : BaseOrganizationWebForm
{
    protected string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
    protected string logoFolder_squareLogoFolder = System.Configuration.ConfigurationManager.AppSettings["logoFolder"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        string imageNameResized = Request.QueryString["imageNameResized"];

        if (string.IsNullOrEmpty(imageNameResized))
        {
            Response.Write("Invalid or missing image name.");
            return;
        }

        // This page is now exclusively for cropping the square logo
        imgsquareLogoPhoto.ImageUrl = logoFolder_squareLogoFolder + imageNameResized;
        lbltext.InnerText = "Crop Your Square Logo";
    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string imageNameResized = Request.QueryString["imageNameResized"];
            if (string.IsNullOrEmpty(imageNameResized))
            {
                Response.Write("Invalid or missing image name.");
                return;
            }

            string imageNameCropped = imageNameResized.Replace("_resized", "_crop");
            string imageFileFolder = Server.MapPath(logoFolder_squareLogoFolder);

            string filePathNameResized = Path.Combine(imageFileFolder, imageNameResized);
            string filePathNameCropped = Path.Combine(imageFileFolder, imageNameCropped);

            // Get cropping coordinates
            int w = Convert.ToInt32(decimal.Parse(W.Value));
            int h = Convert.ToInt32(decimal.Parse(H.Value));
            int x = Convert.ToInt32(decimal.Parse(X.Value));
            int y = Convert.ToInt32(decimal.Parse(Y.Value));

            // Crop the image
            byte[] cropImage = Crop(filePathNameResized, w, h, x, y);

            using (MemoryStream ms = new MemoryStream(cropImage, 0, cropImage.Length))
            {
                ms.Write(cropImage, 0, cropImage.Length);

                using (System.Drawing.Image croppedImage = System.Drawing.Image.FromStream(ms, true))
                {
                    croppedImage.Save(filePathNameCropped, croppedImage.RawFormat);
                }
            }

            // Save the cropped square logo to the database
            using (var dc = new CrowdReliefDBDataContext())
            {
                var userOrg = dc.UserOrganizations.FirstOrDefault(uo => uo.UserId == userId && uo.Status== (int)RequestStatus.Approved && uo.Status == (int)RequestStatus.Pending);
                if (userOrg != null)
                {
                    var organization = dc.Organizations.FirstOrDefault(o => o.OrganizationId == userOrg.OrganizationId);
                    if (organization != null)
                    {
                        organization.LogoSquare = imageNameCropped;
                        dc.SubmitChanges();
                    }
                }
            }

            // Redirect after successful save
            Response.Redirect("/V1/NonProfit/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
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