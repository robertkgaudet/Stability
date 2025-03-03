using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DonationCampaignVM
{
    public string CampaignName { get; set; }
    public Guid DonationCampaignId { get; set; }
    public bool IsDefault { get; set; }
    public string Description { get; set; }
    public string Summary { get; set;}
    public string Address { get; set; }


}