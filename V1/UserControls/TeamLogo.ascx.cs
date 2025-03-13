using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_UserControls_TeamLogo : System.Web.UI.UserControl
{
    public Guid UserId { get; set; }
    string teamLogo = System.Configuration.ConfigurationManager.AppSettings["logoFolder"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadBadges();
        }
    }
    public void LoadBadges()
    {
        if (UserId != Guid.Empty)
        {
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var profile = dc.Profiles.FirstOrDefault(p => p.UserId == UserId);
                if (profile != null)
                {
                    var orgUser = (from o in dc.Organizations
                                   join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                   where uo.UserId == UserId
                                   orderby o.CreatedOn descending
                                   select new
                                   {
                                       o.LogoSquare,
                                       o.OrganizationId,
                                       uo.ShowTeamLogo
                                   }).Take(1).SingleOrDefault();

                    if (orgUser != null)
                    {
                       
                        if (orgUser.ShowTeamLogo ?? false)
                        {
                            imgTeamLogo.ImageUrl = teamLogo + orgUser.LogoSquare;
                            imgTeamLogo.Visible = true;
                        }
                        else
                        {
                            imgTeamLogo.Visible = false;
                        }
                    }
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