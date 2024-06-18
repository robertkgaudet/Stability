using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;

public partial class V1_MasterPages_DisasterRegistry : System.Web.UI.MasterPage
{
    public string _pageTitle = string.Empty;
    public string _pageDescription = string.Empty;
    public string _fbImage = string.Empty;
    public string _fbURL = string.Empty;
    public string _fbImageType = string.Empty;
    public string _fbSite_name = string.Empty;
    public string _fbDescription = string.Empty;
    public bool _hideHeader = false;
    public bool _hideFooter = false;
    public bool _hideMenu = false;
    public string bodyTag = string.Empty;
    public string profileURL = string.Empty;
    public string fixedFooter = "fixed-footer";
    public string fixedHeader = "fixed-header";
    public Guid userId = Guid.NewGuid();
    public string activeDisasterCount = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        form1.Action = HttpContext.Current.Request.RawUrl;
        LoadActiveDisasters();
        title.Text = PageTitle;
        description.Attributes.Add("content", PageTitle);


        if (_hideHeader)
        {
            fixedHeader = string.Empty;
            header.Visible = false;
        }
        if (_hideFooter)
        {
            fixedFooter = string.Empty;
            footer.Visible = false;
        }

        fbTitle.Attributes.Add("content", PageTitle);
        fbImage.Attributes.Add("content", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + FbImage);
        fbURL.Attributes.Add("content", FbURL);
        fbImageType.Attributes.Add("content", FbImageType);// content="image/jpeg" content="image/png"
        fbSite_name.Attributes.Add("content", FbSite_name);
        fbDescription.Attributes.Add("content", FbDescription);

        //activeStatus.Attributes.Add("checked", "checked");
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
        }
        else
        {

        }
    }

    private void LoadActiveDisasters()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var activeDisasters = from ev in dc.Events
                              where ev.IsActive == true
                              orderby ev.BeginDate descending
                              select ev;

        string disasters = string.Empty;
        activeDisasterCount = activeDisasters.Count().ToString();
        foreach (var disaster in activeDisasters)
        {
            disasters += "<li><a href=\"/Disaster/" + disaster.URLFriendlyName + "\">" + disaster.Name + "</a></li>" + Environment.NewLine;
        }
        litActiveDisasters.Text = disasters;
    }

    public string PageTitle
    {
        get
        {
            return _pageTitle;
        }
        set
        {
            _pageTitle = value;
        }
    }
    public string PageDescription
    {
        get
        {
            return _pageDescription;
        }
        set
        {
            _pageDescription = value;
        }
    }
    public string FbImage
    {
        get
        {
            return _fbImage;
        }
        set
        {
            _fbImage = value;
        }
    }
    public string FbURL
    {
        get
        {
            return _fbURL;
        }
        set
        {
            _fbURL = value;
        }
    }
    public string FbImageType
    {
        get
        {
            return _fbImageType;
        }
        set
        {
            _fbImageType = value;
        }
    }
    public string FbSite_name
    {
        get
        {
            return _fbSite_name;
        }
        set
        {
            _fbSite_name = value;
        }
    }
    public string FbDescription
    {
        get
        {
            return _fbDescription;
        }
        set
        {
            _fbDescription = value;
        }
    }
}