using System.Linq;
using System;
using System.Web;
public partial class V1_UserControls_TeamLogo : System.Web.UI.UserControl
{
    public Guid UserId { get; set; }
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
                    //var userRoles = from ur in dc.aspnet_UsersInRoles
                    //                join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
                    //                where ur.UserId == UserId
                    //                select r.RoleName; 
                    //if (userRoles.Contains("Team Administrator"))
                    //{
                    //    UserName += " (Team Administrator)";
                    //}
                    //else if (userRoles.Contains("Administrator"))
                    //{
                    //    UserName += " (Team Owner)";
                    //}
                    lblprofileusername.Text = UserName;
                    lblprofileusername.Visible = true;
                    string currentPageUrl = HttpContext.Current.Request.Url.AbsolutePath;
                    if (!currentPageUrl.Equals("/V1/Member/Default.aspx", StringComparison.OrdinalIgnoreCase))
                    {
                        hypName.Visible = true;
                        hypName.NavigateUrl = "/V1/Member/Default.aspx?userId=" + UserId;
						hypStabilityLogo.NavigateUrl = "/V1/Member/Default.aspx?userId=" + UserId;
					}
                    else
                    {
                        hypName.Visible = true;
                        hypName.NavigateUrl = string.Empty;
                        hypName.Attributes.Remove("href");

						hypStabilityLogo.NavigateUrl = string.Empty;
						hypStabilityLogo.Attributes.Remove("href");
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
                                       o.EnableTeamMemberVerification,
                                       uo.ShowTeamLogo,
                                   }).Take(1).SingleOrDefault();

                    if (orgUser != null)
                    {
                        if (orgUser.EnableTeamMemberVerification == true && orgUser.ShowTeamLogo == true)
                        {
                          
                            imgTeamLogo.ImageUrl = !string.IsNullOrEmpty(orgUser.LogoSquare)
                                ? teamLogo + orgUser.LogoSquare
                                : "/V1/Images/DefaultLogo.png";

                         
                            imgTeamLogo.Visible = true;
                            imgTeamLogo.Attributes["title"] = orgUser.Name + " Verified";
                            hypTeamLogo.Visible = true;
                        }
                        else
                        {
                            // Hide the team logo if conditions are not met
                            hypTeamLogo.Visible = false;
                        }
                    }
                    if (profile.IsDisasterReadyCertified)
					{
						//Stability Verified, show purple logo.
						hypStabilityLogo.Visible = true;
						hypStabilityLogo.NavigateUrl = "/V1/Member/Default.aspx?userId=" + UserId;

						imgStabilityBadge.ImageUrl = teamLogo + "purplebadge.png";
                        imgStabilityBadge.Visible = true;
                    }
                    else
                    {
						hypStabilityLogo.Visible = false;
						imgStabilityBadge.Visible = false;
                    }
                }
            }
        }
    }
}