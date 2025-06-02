using System;
using System.Configuration;
using System.Linq;
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

		if (String.IsNullOrEmpty(volunteerStatus) || volunteerStatus != VolunteerStatus.VettingComplete_Passed.Value)
		{
			Response.Write("Unable to print ID Card. Please email support@stability.org.");
			Response.End();
		}
		this.Master.HideFooter = true;
		this.Master.HideHeader = true;
		this.Master.HideMenu = true;	
		if(User.Identity.IsAuthenticated)
		{
			string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var profilePhoto = (from p in dc.Photos
						   join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
						   where ph.UserId == userId
						   orderby p.CreatedOn descending
							select p).Take(1).SingleOrDefault();    
            if (profilePhoto != null)
			{   
                imgProfile.ImageUrl = profilePhotoFolder + profilePhoto.FilenameCropped;	
            }
            else
            {
                imgProfile.ImageUrl = profilePhotoFolder + "profilepicture.png";

            }
            var profile = (from p in dc.Profiles
                           where p.UserId == userId
                           select p).SingleOrDefault();
            litNumber.Text = "Volunteer #<br /><span class='h4 font-bold'>" + profile.ProfileNumber + "</span>";
            if (!String.IsNullOrEmpty(profile.City))
            {
                locationDD = "" + profile.City + ", " + profile.State + "";
            }
            litTitle.Text = "VETTED DISASTER WORKER";
            if (!String.IsNullOrEmpty(profile.Title))
            {
                litTitle.Text = profile.Title.ToUpper();
            }
            if (profile.StabilityVerifiedDate != null)
            {
                litStabilityVerifiedDate.Text = profile.StabilityVerifiedDate.Value.ToString("MM/dd/yyyy");
                lblStabilityVerifiedDate.Visible = true;
                litStabilityVerifiedDate.Visible = true;
            }
            else
            {
                lblStabilityVerifiedDate.Visible = false;
                litStabilityVerifiedDate.Visible = false;
            }
            litPrintDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            LoadTeamLogoControl();
        }
	}
    private void LoadTeamLogoControl()
    {
        var ucTeamLogo = FindControlRecursive<V1_UserControls_TeamLogo>(this, "ucTeamLogo");
        if (ucTeamLogo != null)
        {
            ucTeamLogo.UserId = userId;
            ucTeamLogo.LoadNameWithBadges();
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                string watermarkImagePath = "/V1/Images/DefaultLogo.png";
                var orgUser = (from o in dc.Organizations
                               join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                               where uo.UserId == userId
                               orderby o.CreatedOn descending
                               select new
                               {
                                   o.LogoSquare,
                                   o.OrganizationId,
                                   o.Name,
                                   o.EnableTeamMemberVerification,
                                   uo.ShowTeamLogo,
                                   uo.TeamVerifiedDate
                               }).Take(1).SingleOrDefault();
                if (orgUser != null)
                {
                    TeamName.Text = orgUser.Name.ToUpper();
                        if (!string.IsNullOrEmpty(orgUser.LogoSquare))
                        {
                            string teamLogo = ConfigurationManager.AppSettings["logoFolder"].ToString();
                            watermarkImagePath = !string.IsNullOrEmpty(orgUser.LogoSquare)
                                ? teamLogo + orgUser.LogoSquare
                                : "/V1/Images/DefaultLogo.png";
                        }                 
                    if (orgUser.TeamVerifiedDate != null)
                    {
                        litTeamVerifiedDate.Text = orgUser.TeamVerifiedDate.Value.ToString("MM/dd/yyyy");
                        lblTeamVerifiedDate.Visible = true;
                        litTeamVerifiedDate.Visible = true;
                    }
                    else
                    {
                        lblTeamVerifiedDate.Visible = false;
                        litTeamVerifiedDate.Visible = false;
                    }
                    watermarkBg.Style.Add("background-image", "url('" + watermarkImagePath + "')");
                }
            }
        }
    }
    public static T FindControlRecursive<T>(Control root, string id) where T : Control
    {
        if (root == null) return null;
        var foundControl = root.FindControl(id) as T;
        if (foundControl != null) return foundControl;
        foreach (Control control in root.Controls)
        {
            foundControl = FindControlRecursive<T>(control, id);
            if (foundControl != null) return foundControl;
        }
        return null;
    }
}