using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;

public partial class V1_NonProfitAdministration_CoverImage1600x600 : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}
	
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		string organizationId = Request.QueryString["organizationId"];

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
							string imageFileFolder			= Server.MapPath(causePhotoFolder);
							string imageExtension			= Path.GetExtension(profilePhotoUpload.PostedFile.FileName);;

							string imageNameOriginal		= imageGuid + imageExtension;

							string filePathnameOriginal		= Path.Combine(imageFileFolder, imageNameOriginal);
							
							profilePhotoUpload.PostedFile.SaveAs(filePathnameOriginal);

							CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

							var organization = (from o in dc.Organizations
												where o.OrganizationId == new Guid(organizationId)
												select o).SingleOrDefault();

							organization.CoverImage = imageNameOriginal;
							dc.SubmitChanges();

							Response.Redirect("~/V1/NonProfit/NonProfit.aspx?organizationId=" + organizationId);
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
}