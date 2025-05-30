using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
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
                                        where uo.UserId == UserId && uo.IsPrimary == true && uo.Status== (int)RequestStatus.Approved
                                        select new
                                        {
                                            o.LogoSquare,
                                            o.OrganizationId,
                                            o.Name,
                                            uo.ShowTeamLogo,
                                        }).FirstOrDefault();
                      Guid orgId;
                     if (Guid.TryParse(organizationId, out orgId))
                    {
                        var userOrg = dc.UserOrganizations
                            .FirstOrDefault(uo => uo.UserId == UserId && uo.OrganizationId == orgId && uo.Status== (int)RequestStatus.Approved);

                        if (userOrg != null)
                        {
                            orgUser = dc.Organizations
                                .Where(o => o.OrganizationId == orgId)
                                .Select(o => new
                                {
                                    o.LogoSquare,
                                    o.OrganizationId,
                                    o.Name,
                                    userOrg.ShowTeamLogo
                                })
                                .FirstOrDefault();
                        }
                    }
                    else
                    {
                        orgUserr = (from o in dc.Organizations
                                    join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                    where uo.UserId == UserId && uo.ShowTeamLogo == true && uo.Status== (int)RequestStatus.Approved
                                    orderby o.CreatedOn descending
                                    select new
                                    {
                                        o.LogoSquare,
                                        o.OrganizationId,
                                        o.Name,
                                        uo.ShowTeamLogo,
                                    }).FirstOrDefault();
                    }

                    if (orgUser != null || isprimaryorg != null || orgUserr != null)
                    {
                        // Determine which source to use
                        dynamic source = isprimaryorg ?? orgUser ?? orgUserr;

                        // Determine virtual and physical paths for logo
                        string logoFile = source.LogoSquare;
                        string virtualPath = !string.IsNullOrEmpty(logoFile) ? teamLogo + logoFile : null;
                        string physicalPath = !string.IsNullOrEmpty(virtualPath) ? Server.MapPath(virtualPath) : null;

                        // Apply file-exists check like profile photo logic
                        if (!string.IsNullOrEmpty(physicalPath) && File.Exists(physicalPath))
                        {
                            imgTeamLogo.ImageUrl = virtualPath;
                        }
                        else
                        {
                            imgTeamLogo.ImageUrl = "/V1/Images/Logo-Placeholder.png";
                        }

                        // Set hyperlink and title
                        string url = "/V1/NonProfit/Default.aspx?organizationId=";
                        hypTeamLogo.NavigateUrl = url + source.OrganizationId;
                        imgTeamLogo.Attributes["title"] = source.Name + " Verified";

                        // Determine whether to show/hide team logo
                        bool showLogo = false;

                        if (orgUser != null)
                        {
                            showLogo = orgUser.ShowTeamLogo == true;
                        }
                        else if (isprimaryorg != null && orgUserr != null)
                        {
                            showLogo = true;
                        }
                        else if (orgUserr != null)
                        {
                            showLogo = true;
                        }

                        if (showLogo)
                        {
                            imgTeamLogo.Style.Add(HtmlTextWriterStyle.Display, "inline-flex");
                            hypTeamLogo.Style.Add(HtmlTextWriterStyle.Display, "inline-flex");
                        }
                        else
                        {
                            imgTeamLogo.Style.Add(HtmlTextWriterStyle.Display, "none");
                            hypTeamLogo.Style.Add(HtmlTextWriterStyle.Display, "none");
                        }
                    }

                    hypStabilityLogo.NavigateUrl = "/V1/Member/Default.aspx?userId=" + UserId;
                    imgStabilityBadge.ImageUrl = teamLogo + "purplebadge.png";
                    if (profile.IsDisasterReadyCertified)
                    {
                        imgStabilityBadge.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "inline-flex");
                        hypStabilityLogo.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "inline-flex");
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