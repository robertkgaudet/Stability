using System.Linq;
using System;
using System.Web;
using Twilio.Types;
public partial class V1_UserControls_TeamLogo : System.Web.UI.UserControl
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool ShowPhoneNumber { get; set; }
    public bool ShowEmail { get; set; }
    public string organizationId = string.Empty;




    string teamLogo = System.Configuration.ConfigurationManager.AppSettings["logoFolder"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        organizationId = Request.QueryString["organizationId"];
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
                var userEmail = dc.aspnet_Memberships.FirstOrDefault(p => p.UserId == UserId);
                {
                    UserName = profile.Firstname + " " + profile.Lastname;
                    Email = userEmail.Email;
                    PhoneNumber = profile.PhoneNumber;
                    lblprofileusername.Text = UserName;
                    lblprofileusername.Visible = true;
                    lblemail.Text = Email;
                    lblemail.Visible = ShowEmail;
                    lblphoneNumber.Text = PhoneNumber;
                    lblphoneNumber.Visible = ShowPhoneNumber;


                    string currentPageUrl = HttpContext.Current.Request.Url.AbsolutePath;
                    if (!currentPageUrl.Equals("/V1/Member/Default.aspx", StringComparison.OrdinalIgnoreCase))
                    {
                        hypName.Visible = true;
                        phoneNumber.Visible = ShowPhoneNumber;
                        email.Visible = ShowEmail;
                        hypName.NavigateUrl = "/V1/Member/Default.aspx?userId=" + UserId;
                        hypStabilityLogo.NavigateUrl = "/V1/Member/Default.aspx?userId=" + UserId;
                    }
                    else
                    {
                        hypName.Visible = true;
                        phoneNumber.Visible = ShowPhoneNumber;
                        email.Visible = ShowEmail;
                        hypName.NavigateUrl = string.Empty;
                        hypName.Attributes.Remove("href");

                        hypStabilityLogo.NavigateUrl = string.Empty;
                        hypStabilityLogo.Attributes.Remove("href");
                    }
                    dynamic orgUser = null;
                    dynamic orgUserr = null;
                    var isprimaryorg = (from o in dc.Organizations
                                        join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                        where uo.UserId == UserId && uo.IsPrimary == true
                                        select new
                                        {
                                            o.LogoSquare,
                                            o.OrganizationId,
                                            o.Name,
                                            o.EnableTeamMemberVerification,
                                            uo.ShowTeamLogo,
                                        }).FirstOrDefault();
                      Guid orgId;
                     if (Guid.TryParse(organizationId, out orgId))
                    {
                        var userOrg = dc.UserOrganizations
                            .FirstOrDefault(uo => uo.UserId == UserId && uo.OrganizationId == orgId);

                        if (userOrg != null)
                        {
                            orgUser = dc.Organizations
                                .Where(o => o.OrganizationId == orgId)
                                .Select(o => new
                                {
                                    o.LogoSquare,
                                    o.OrganizationId,
                                    o.Name,
                                    o.EnableTeamMemberVerification,
                                    userOrg.ShowTeamLogo
                                })
                                .FirstOrDefault();
                        }
                    }
                    else
                    {
                        orgUserr = (from o in dc.Organizations
                                    join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                    where uo.UserId == UserId && uo.ShowTeamLogo == true
                                    orderby o.CreatedOn descending
                                    select new
                                    {
                                        o.LogoSquare,
                                        o.OrganizationId,
                                        o.Name,
                                        o.EnableTeamMemberVerification,
                                        uo.ShowTeamLogo,
                                    }).FirstOrDefault();
                    }

                    if (orgUser != null || isprimaryorg !=null || orgUserr !=null)
                    {
                        if (isprimaryorg != null)
                        {
                            imgTeamLogo.ImageUrl = !string.IsNullOrEmpty(isprimaryorg.LogoSquare)
                                ? teamLogo + isprimaryorg.LogoSquare
                                : "/V1/Images/DefaultLogo.png";
                            string url = "/V1/NonProfit/Default.aspx?organizationId=";
                            hypTeamLogo.NavigateUrl = url+isprimaryorg.OrganizationId;
                            imgTeamLogo.Attributes["title"] = isprimaryorg.Name + " Verified";
                        }
                        else if(orgUser!=null)
                        {

                            imgTeamLogo.ImageUrl = !string.IsNullOrEmpty(orgUser.LogoSquare)
                                ? teamLogo + orgUser.LogoSquare
                                : "/V1/Images/DefaultLogo.png";
                            string url = "/V1/NonProfit/Default.aspx?organizationId=";
                            hypTeamLogo.NavigateUrl = url + orgUser.OrganizationId;
                            imgTeamLogo.Attributes["title"] = orgUser.Name + " Verified";
                        }
                        else
                        {
                            imgTeamLogo.ImageUrl = !string.IsNullOrEmpty(orgUserr.LogoSquare)
                                ? teamLogo + orgUserr.LogoSquare
                                : "/V1/Images/DefaultLogo.png";
                            string url = "/V1/NonProfit/Default.aspx?organizationId=";
                            hypTeamLogo.NavigateUrl = url + orgUserr.OrganizationId;
                            imgTeamLogo.Attributes["title"] = orgUserr.Name + " Verified";
                        }

                        if (orgUser != null)
                        {
                            if (orgUser.EnableTeamMemberVerification == true && orgUser.ShowTeamLogo == true)
                            {
                                imgTeamLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "block");
                                hypTeamLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "block");
                            }
                        }
                        else
                        {
                            if (isprimaryorg != null && orgUserr != null)
                            {
                                imgTeamLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "block");
                                hypTeamLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "block");
                            }
                            else if(orgUserr !=null)
                            {
                                imgTeamLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "block");
                                hypTeamLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "block");
                            }
                            else
                            {
                                hypTeamLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "none");
                                imgTeamLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "none");
                            }

                        }
                    }
                    hypStabilityLogo.NavigateUrl = "/V1/Member/Default.aspx?userId=" + UserId;
                    imgStabilityBadge.ImageUrl = teamLogo + "purplebadge.png";
                    if (profile.IsDisasterReadyCertified)
                    {
                        imgStabilityBadge.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "block");
                        hypStabilityLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "block");
                    }
                    else
                    {
                        imgStabilityBadge.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "none");
                        hypStabilityLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "none");
                    }
                }
            }
        }
    }
}