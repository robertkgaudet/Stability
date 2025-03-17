using System.Linq;
using System;

public partial class V1_UserControls_TeamLogo : System.Web.UI.UserControl
{
    public Guid UserId { get; set; }
    public string PageName { get; set; }
    public string UserName { get; set; }
    string teamLogo = System.Configuration.ConfigurationManager.AppSettings["logoFolder"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadNameWithBadges();
        }
    }

    public void LoadNameWithBadges()
    {
        if (UserId != Guid.Empty)
        {
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var profile = dc.Profiles.FirstOrDefault(p => p.UserId == UserId);
                if (profile != null)
                {
                
                    UserName = profile.Firstname + " " + profile.Lastname;
                    lblprofileusername.Text = UserName;
                    lblprofileusername.Visible = true;

                  
                    if (!string.IsNullOrEmpty(PageName))
                    {
                        hypName.Visible = true; 
                        if (PageName == "people")
                        {
                            hypName.NavigateUrl = "/V1/Member/Default.aspx?userId=" + UserId;
                        }
                        else if (PageName == "profile")
                        {
                            hypName.NavigateUrl = "/V1/Profile/Profile.aspx?userId=" + UserId;
                        }
                    }
                    else
                    {
                      
                        hypName.Visible = true; 
                    }

                
                    var orgUser = (from o in dc.Organizations
                                   join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                   where uo.UserId == UserId
                                   orderby o.CreatedOn descending
                                   select new
                                   {
                                       o.LogoSquare,
                                       o.OrganizationId,
                                       o.Name,
                                       uo.ShowTeamLogo
                                   }).Take(1).SingleOrDefault();

                    if (orgUser != null)
                    {
                        if (orgUser.ShowTeamLogo ?? false)
                        {
                            imgTeamLogo.ImageUrl = teamLogo + orgUser.LogoSquare;
                            imgTeamLogo.Visible = true;
                            imgTeamLogo.Attributes["title"] = orgUser.Name + " Verified";
                            hypTeamLogo.Visible = true;
                        }
                        else
                        {
                            hypTeamLogo.Visible = false;
                        }
                    }

                    // Set the badge (if applicable)
                    if (profile.IsDisasterReadyCertified)
                    {
                        imgStabilityBadge.ImageUrl = teamLogo + "purplebadge.png";
                        imgStabilityBadge.Visible = true;
                    }
                    else
                    {
                        imgStabilityBadge.Visible = false;
                    }
                }
            }
        }
    }

}