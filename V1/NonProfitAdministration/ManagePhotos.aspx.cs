using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using SD = System.Drawing;
using System.Drawing.Drawing2D;

public partial class V1_NonProfitAdministration_ManagePhotos : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{ 
			string organizationId = Request.QueryString["organizationId"];

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			string organizationEventId = Request.QueryString["organizationEventId"];
			if (!String.IsNullOrEmpty(organizationEventId))
			{
				litCause.Visible = false;
			}
			else
			{ 
				//Load the cause list.
				var causes = from oe in dc.OrganizationEvents
							 where oe.IsActive == true && oe.OrganizationId == new Guid(organizationId)
							 orderby oe.CreatedOn descending
							 select oe;

				string causeList = string.Empty;
				foreach (var cause in causes)
				{
					causeList += "<li id=\"" + cause.OrganizationEventId + "\"><a href=\"#\"><i class=\"fa fa-globe\"></i> " + cause.CampaignName + "</a></li>" + Environment.NewLine;
				}
				litCause.Text = causeList;
			}

			//<a href="/Homer/images/gallery/1.jpg" title="Image from Unsplash" data-gallery=""><img src="/Homer/images/gallery/1s.jpg"></a>
			var photos = from oep in dc.OrganizationEventPhotos
						 join oe in dc.OrganizationEvents on oep.OrganizationEventId equals oe.OrganizationEventId
						 join p in dc.Photos on oep.PhotoId equals p.PhotoId
						 where oe.OrganizationId.Equals(organizationId) && oep.IsCover == false
						 select p;

			string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
			string photolist = string.Empty;
			if(photos.Count() > 0)
			{
				foreach (var photo in photos)
				{
					photolist += "<a href=\"" + causePhotoFolder + photo.Filename + "\" title=\"" + photo.Filename + "\" data-gallery=\"\"><img src=\"" + causePhotoFolder + photo.Filename + "\" width=\"300\"></a>";
				}
				litPhotoCallery.Text = photolist;
			}
		}
	}

	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		string organizationId = Request.QueryString["organizationId"]; 
		string organizationEventId = Request.QueryString["organizationEventId"];
		if (String.IsNullOrEmpty(organizationEventId))
		{
			organizationEventId = hidCauseId.Value;
		}

		try
		{
			if (profilePhotoUpload.PostedFile.ContentLength > 0)
			{
				try
				{
					if (profilePhotoUpload.PostedFile.ContentType == "image/jpeg" || profilePhotoUpload.PostedFile.ContentType == "image/heic" || profilePhotoUpload.PostedFile.ContentType == "image/png")
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
							Photo photo = new Photo();
							photo.PhotoId = new Guid(imageGuid);
							photo.Filename = imageNameOriginal;
							photo.CreatedOn = DateTime.Now;
							photo.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
							dc.Photos.InsertOnSubmit(photo);
							dc.SubmitChanges();

							OrganizationEventPhoto organizationEventPhoto = new OrganizationEventPhoto();
							organizationEventPhoto.PhotoId = new Guid(imageGuid);
							organizationEventPhoto.IsCover = false;
							organizationEventPhoto.OrganizationEventPhotoId = Guid.NewGuid();
							organizationEventPhoto.OrganizationEventId = new Guid(organizationEventId);

							dc.OrganizationEventPhotos.InsertOnSubmit(organizationEventPhoto);
							dc.SubmitChanges();

							Response.Redirect("~/V1/NonProfitAdministration/ManagePhotos.aspx?organizationId=" + organizationId);
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