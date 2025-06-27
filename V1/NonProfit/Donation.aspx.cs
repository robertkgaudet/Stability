using System;
using System.Collections.Generic;
using System.Linq;

public partial class V1_NonProfit_Donation : System.Web.UI.Page
{
    public string LiteralNonDefaultCampaignNames { get; set; }
    public string orgId { get; set; }
    public string donationCampaignId { get; set; }
    public string paypalLink { get; set; }
    public string venmoLink { get; set; }
    public string cashAppLink { get; set; }
    public string LiteralSummary { get; set; }
    public string LiteralAddress { get; set; }
    public Boolean ShowDonationButton { get; set; }
    public List<DonationCampaignVM> DonationCampaigns { get; set; }
    public DonationCampaignVM DefaultCampaign = new DonationCampaignVM();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(Request.QueryString["organizationId"]))
        {
            Response.Write("No organization Id provided.");
            Response.End();
            return;
        }
        this.organizationId.Value = Request.QueryString["organizationId"].ToString();
        this.orgId = Request.QueryString["organizationId"].ToString();
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        PaymentConfiguration payConfig = dc.PaymentConfigurations.Where(x => x.OrganizationId == new Guid(this.organizationId.Value)).FirstOrDefault();

        var donationCampaigns = (from dca in dc.DonationCampaigns
                                 join oe in dc.OrganizationEvents
                                 on dca.OrganizationEventId equals oe.OrganizationEventId into oeGroup
                                 from oe in oeGroup.DefaultIfEmpty()
                                 where dca.OrganizationId == new Guid(this.organizationId.Value)
                                 select new DonationCampaignVM
                                 {
                                     CampaignName = oe != null ? oe.CampaignName : null,
                                     DonationCampaignId = dca.DonationCampaignId,
                                     IsDefault = dca.IsDefault,
                                     Address = dca.Address,
                                     Summary = dca.Summary,
                                     Description = dca.Description
                                 }).ToList();

        if (payConfig != null)
        {
            paypalLink = payConfig.PayPal;
            venmoLink = payConfig.Venmo;
            cashAppLink = payConfig.CashApp;
            ShowDonationButton = payConfig.ShowDonationButton.Value;
        }

        if (donationCampaigns.Any(x => x.IsDefault))
        {
            DefaultCampaign = donationCampaigns
                                        .Where(x => x.IsDefault == true)
                                        .Select(x => new DonationCampaignVM
                                        {
                                            Summary = x.Summary,
                                            Address = x.Address,
                                            Description = x.Description,
                                            DonationCampaignId = x.DonationCampaignId
                                        }).FirstOrDefault();
        }
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

    }
}