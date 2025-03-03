using Braintree;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Impactoid_Version_2_MasterPage : System.Web.UI.MasterPage
{
    //PAGE META TAGS
    public string _pageTitle = string.Empty;
    public string _pageDescription = string.Empty;

    //FB META TAGS
    public string _fbImage = string.Empty;
    public string _fbDescription = string.Empty;
    public string _fbURL = string.Empty;
    public string _fbImageType = string.Empty;
    public string _fbSite_name = string.Empty;
    public string organizationId = string.Empty;
    public string organizationName = string.Empty;
    public string logo = string.Empty;
    public string donateURL = string.Empty;
    public string volunteerURL = string.Empty;
    public List<DonationCampaignVM> DonationCampaigns { get; set; }
    public string donationCampaignId { get; set; }
    public bool ShowDonationButton { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        organizationId = Request.QueryString["organizationId"];
        organizationName = Request.QueryString["organizationName"];

        using (var context = new CrowdReliefDBDataContext())
        {
            var paymentConfig = context.PaymentConfigurations.FirstOrDefault();

            if (paymentConfig != null)
            {
                ShowDonationButton = paymentConfig.ShowDonationButton ?? false;
            }
        }

        
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var donationCampaigns = (from oe in dc.OrganizationEvents
                                      join dca in dc.DonationCampaigns on oe.OrganizationEventId equals dca.OrganizationEventId
                                      where oe.OrganizationId == new Guid(this.organizationId) && !dca.IsDefault
                                 select new { oe.CampaignName, dca.DonationCampaignId, dca.IsDefault, dca.Address, dca.Summary, dca.Description }).ToList();


       
     
        if (donationCampaigns.Any(x => x.IsDefault == false))
        {
            DonationCampaigns = donationCampaigns
                                        .Where(x => x.IsDefault == false)
                                        .Select(x => new DonationCampaignVM
                                        {
                                            CampaignName = x.CampaignName,
                                            DonationCampaignId = x.DonationCampaignId,
                                        }).ToList();
        }
        if (String.IsNullOrEmpty(organizationId) && String.IsNullOrEmpty(organizationName))
        {
            Response.Write("No Organization Id Was Provided. Contact site administrators at help@impactoid.com.");
            Response.End();
        }

    

        Organization organization;
        if (String.IsNullOrEmpty(organizationId))
        {
            organization = (from o in dc.Organizations
                            where o.URLFriendlyName == organizationName
                            select o).SingleOrDefault();
        }
        else
        {
            organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(organizationId)
                            select o).SingleOrDefault();
        }

        if (organization != null)
        {
            if (String.IsNullOrEmpty(organization.Logo))
            {
                imgLogo.Visible = false;
                litLogo.Visible = true;
                litLogo.Text = organization.Name;
            }
            else
            {
                imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + organization.Logo;
                imgLogo.AlternateText = organization.Name + " Logo";
                imgLogo.Visible = true;
                litLogo.Visible = false;
            }

            if (!String.IsNullOrEmpty(organization.DonationURL))
            {
                donateURL = organization.DonationURL;
                volunteerURL = organization.VolunteerURL;
            }

            LoadMetaTags();

            //Do they have causes?
            var causes = from oe in dc.OrganizationEvents
                         where oe.OrganizationId == new Guid(organizationId)
                         && oe.IsActive == true
                         select oe;

            if (causes.Count() == 0)
            {
                divCauses.Visible = false;
            }

            //Do they have programs?
            var programs = from op in dc.OrganizationPrograms
                           where op.OrganizationId == new Guid(organizationId)
                           select op;

            if (programs.Count() == 0)
            {
                divProgram.Visible = false;
            }
        }
    }

    protected void LoadMetaTags()
    {
        title.InnerText = _pageTitle;
        description.Attributes.Add("content", _pageTitle);
        fbTitle.Attributes.Add("content", PageTitle);
        fbImage.Attributes.Add("content", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + FbImage);
        fbDescription.Attributes.Add("content", FbDescription);
        fbURL.Attributes.Add("content", FbURL);
        fbImageType.Attributes.Add("content", FbImageType); // content="image/jpeg" content="image/png"
        fbSite_name.Attributes.Add("content", FbSite_name);
    }

    public string PageTitle
    {
        get { return _pageTitle; }
        set { _pageTitle = value; }
    }
    public string PageDescription
    {
        get { return _pageDescription; }
        set { _pageDescription = value; }
    }
    public string FbImage
    {
        get { return _fbImage; }
        set { _fbImage = value; }
    }
    public string FbURL
    {
        get { return _fbURL; }
        set { _fbURL = value; }
    }
    public string FbImageType
    {
        get { return _fbImageType; }
        set { _fbImageType = value; }
    }
    public string FbSite_name
    {
        get { return _fbSite_name; }
        set { _fbSite_name = value; }
    }
    public string FbDescription
    {
        get { return _fbDescription; }
        set { _fbDescription = value; }
    }
}
