using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Profile_IDCard : BaseOrganizationWebForm
{
	public string zelloDD = string.Empty;
	public string locationDD = string.Empty;
	public string cityStateDD = string.Empty;


	protected void Page_Load(object sender, EventArgs e)
	{
		string volunteerStatus = VolunteerStatus.GetVolunteerStatus(userId).Value;

		if(String.IsNullOrEmpty(volunteerStatus) || volunteerStatus != VolunteerStatus.VettingComplete_Passed.Value)
		{
			Response.Write("Unable to print ID Card. Please call the vetting team at 318-572-3161.");
			Response.End();
		}



		this.Master.HideFooter = true;
		this.Master.HideHeader = true;
		this.Master.HideMenu = true;

		//this.Master.PageTitle			= "Stability - " +  disaster.Name + " Resources Page";
		//this.Master.PageDescription		= "Stability - " +  disaster.Name + " " + disaster.Description;
		//this.Master.FbDescription		= "Stability - " +  disaster.Name + " " + disaster.Description;
		//this.Master.FbImage				= "/V1/Images/Hurricane-Michael-damage.jpg";
		//this.Master.FbImageType			= "image/jpg";
		//this.Master.FbSite_name			= "Stability - " +  disaster.Name + " Resouces Page";
		//this.Master.FbURL				= Request.Url.AbsoluteUri;
		
		if(User.Identity.IsAuthenticated)
		{
			string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var profilePhoto = (from p in dc.Photos
						   join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
						   where ph.UserId == userId
						   orderby p.CreatedOn descending
							select p).Take(1).SingleOrDefault();

			if(profilePhoto != null)
			{
				imgProfile.ImageUrl = profilePhotoFolder + profilePhoto.FilenameCropped;

				var profile = (from p in dc.Profiles
							  where p.UserId == userId
							  select p).SingleOrDefault();

				lblName.Text = profile.Firstname + " " + profile.Lastname;
				
				litNumber.Text = "<dd><h5 class='no-margins'>Volunteer #" + profile.ProfileNumber +  "</h5></dd>";

				if(!String.IsNullOrEmpty(profile.City))
				{
					locationDD = "<dd><h5 class='no-margins'>" + profile.City + ", " + profile.State + "</h5></dd>";
				}
				litTitle.Text = "VETTED DISASTER WORKER";
				if(!String.IsNullOrEmpty(profile.Title))
				{
					litTitle.Text = profile.Title.ToUpper();
				}

				if(!String.IsNullOrEmpty(profile.ZelloName))
				{
					zelloDD = "<dd><h5 class='no-margins'>Zello: " + profile.ZelloName + "</h5></dd>";
				}
			}
		}

	}
}