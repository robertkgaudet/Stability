using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio.TwiML.Messaging;

public partial class V1_NonProfit_DonationDashboard : System.Web.UI.Page
{
    public decimal todayCount { get; set; }
    public decimal thisWeekCount { get; set; }
    public decimal thisMonthCount { get; set; }
    public decimal thisYearCount { get; set; }
    public decimal totalDonation { get; set; }
    class recentsDonars
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedAt { get; set; }
        public string Amount { get; set; }
    }
    class Deployment
    {

        public string CampaignName { get; set; }

        public string Amount { get; set; }

        public string CreatedAt { get; set; }
    }
    public string organizationId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        totalDonation = 0;
        organizationId = Request.QueryString["OrganizationId"];
        if (string.IsNullOrEmpty(organizationId))
        {
            RecentDonorsRepeaterNoData.Text = "No Recent Donar Found";
            AdditionalInfoRepeaterNoData.Text = "No Deployment Donation Found";
        }

        if (!IsPostBack && !string.IsNullOrEmpty(organizationId))
        {
            using (var dc = new CrowdReliefDBDataContext())
            {
                var today = DateTime.Today;
                var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                var startOfMonth = new DateTime(today.Year, today.Month, 1);
                var startOfYear = new DateTime(today.Year, 1, 1);
                // Calculate the total amount donated today
                var todayAmount = dc.Donations.Where(d => d.CreatedAt.Date == today && d.TransactionId != null && d.TransactionId != "" && d.DonationStatus != null && d.DonationStatus.Equals(CrowdRelief.Tools.TransactionStatus.Succeeded))
                                           .Select(x => x.Amount).ToList();
                todayCount = todayAmount.Sum();

                var thisWeekAmount = dc.Donations.Where(d => d.CreatedAt.Date >= startOfWeek && d.TransactionId != null && d.TransactionId != "" && d.DonationStatus != null && d.DonationStatus.Equals(CrowdRelief.Tools.TransactionStatus.Succeeded))
                                               .Select(x => x.Amount).ToList();
                thisWeekCount = thisWeekAmount.Sum();



                var thisMonthAmount = dc.Donations.Where(d => d.CreatedAt.Date >= startOfMonth && d.TransactionId != null && d.TransactionId != "" && d.DonationStatus != null && d.DonationStatus.Equals(CrowdRelief.Tools.TransactionStatus.Succeeded))
                                               .Select(x => x.Amount).ToList();
                thisMonthCount = thisMonthAmount.Sum();


                var thisYearAmount = dc.Donations.Where(d => d.CreatedAt.Date >= startOfYear && d.TransactionId != null && d.TransactionId != "" && d.DonationStatus != null && d.DonationStatus.Equals(CrowdRelief.Tools.TransactionStatus.Succeeded))
                                              .Select(x => x.Amount).ToList();
                thisYearCount = thisYearAmount.Sum();

                List<recentsDonars> recendDonars = new List<recentsDonars>();

                var groupedDonors = dc.DonationCampaigns
                                    .Where(x => x.OrganizationId.Value.ToString() == organizationId)
                                    .Join(dc.Donations,
                                        dcam => dcam.DonationCampaignId,
                                        d => d.DonationCampaignId,
                                        (dcam, d) => d)
                                    .Where(d => d.TransactionId != null && d.TransactionId != "" && d.DonationStatus != null && d.DonationStatus.Equals(CrowdRelief.Tools.TransactionStatus.Succeeded))
                                    .GroupBy(d => new { d.FirstName, d.LastName })
                                    .Select(g => new
                                    {
                                        FirstName = g.Key.FirstName,
                                        LastName = g.Key.LastName,
                                        TotalAmount = g.Sum(x => x != null ? x.Amount : 0),
                                        LatestDonationDate = g.Max(x => x != null ? x.CreatedAt : (DateTime?)null)
                                    })
                                    .OrderByDescending(x => x.LatestDonationDate)
                                    .Take(5)
                                    .ToList();

                var sstotalDonationQuery = dc.Donations.Where(d => d.TransactionId != null && d.TransactionId != "" && d.DonationStatus != null && d.DonationStatus.Equals(CrowdRelief.Tools.TransactionStatus.Succeeded));
                totalDonation = sstotalDonationQuery.Any() ? sstotalDonationQuery.Sum(x => x.Amount) : 0;

                foreach (var item in groupedDonors)
                {
                    var data = new recentsDonars
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        CreatedAt = item.LatestDonationDate.Value.ToString("dd-MM-yyyy"),
                        Amount = item.TotalAmount.ToString()
                    };
                    recendDonars.Add(data);
                }

                RecentDonorsRepeater.DataSource = recendDonars;
                RecentDonorsRepeater.DataBind();


                List<Deployment> deployments = new List<Deployment>();

                var donationsData = (from d in dc.DonationCampaigns
                                     join de in dc.Donations
                                         .Where(d => d.TransactionId != null && d.TransactionId != "" &&
                                                     d.DonationStatus != null && d.DonationStatus.Equals(CrowdRelief.Tools.TransactionStatus.Succeeded))
                                         on d.DonationCampaignId equals de.DonationCampaignId into donationsGroup
                                     from de in donationsGroup.DefaultIfEmpty()
                                     join E in dc.OrganizationEvents
                                         on d.OrganizationEventId equals E.OrganizationEventId into eventsGroup
                                     from E in eventsGroup.DefaultIfEmpty()
                                     where E != null && E.CampaignName != null && E.CampaignName != "" // Replaced IsNullOrEmpty
                                     && (de != null && de.Amount > 0)
                                     group new { de, E } by new { E.CampaignName } into groupedData
                                     select new
                                     {
                                         CampaignName = groupedData.Key.CampaignName,
                                         CreatedAt = groupedData.Max(x => x.E.CreatedOn), // Keep as DateTime? for now
                                         TotalAmount = groupedData.Sum(g => g.de != null ? g.de.Amount : 0)
                                     })
                      .ToList() // Execute query first
                      .Select(x => new
                      {
                          x.CampaignName,
                          CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.Value.ToString("yyyy-MM-dd") : "N/A", // Format in-memory
                          x.TotalAmount
                      }).ToList(); // Convert formatted data to list


                foreach (var item in donationsData)
                {
                    var data = new Deployment
                    {
                        CampaignName = item.CampaignName,
                        CreatedAt = item.CreatedAt,
                        Amount = item.TotalAmount.ToString()
                    };
                    deployments.Add(data);
                }

                AdditionalInfoRepeater.DataSource = deployments;
                AdditionalInfoRepeater.DataBind();

            }
        }
    }


    protected void btnDonationsList_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/V1/Administration/DonationsList.aspx");
    }
}