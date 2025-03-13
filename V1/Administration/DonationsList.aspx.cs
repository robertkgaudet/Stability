using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Administration_DonationsList : System.Web.UI.Page
{

    public class PageInfo
    {
        public string PageNumbers { get; set; }
        public int TotalPageCount { get; set; }
    }

    private static int currentPage = 1;
    private int PageSize = 10;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDonations(nameSearchQuery: null, addressSearchQuery: null, startDateQuery: null, endDateQuery: null, amountSearchQuery: null, sortExpression: "Name", sortDirection: "ASC");
        }
        else
        {
            string eventTarget = Request["__EVENTTARGET"];
            string eventArgument = Request["__EVENTARGUMENT"];

            if (eventTarget == "Sort")
            {
                HandleSorting(eventArgument);
            }
            else
            {
                if (!string.IsNullOrEmpty(eventArgument))
                {
                    int page = 0;
                    if (int.TryParse(eventArgument, out page) && page > 0)
                    {
                        CurrentPage = page;
                        BindDonations();
                    }
                }
            }
        }
    }
    private IQueryable<dynamic> GetFilteredSortedDonations(CrowdReliefDBDataContext dc, string nameSearchQuery, string addressSearchQuery, string startDateQuery, string endDateQuery, string amountSearchQuery, string sortExpression, string sortDirection)
    {
        DateTime parsedStartDate, parsedEndDate;
        bool isStartDateValid = DateTime.TryParse(startDateQuery, out parsedStartDate);
        bool isEndDateValid = DateTime.TryParse(endDateQuery, out parsedEndDate);
        decimal parsedAmount;
        bool isAmountSearch = decimal.TryParse(amountSearchQuery, out parsedAmount);

        //var donationsQuery = dc.Donations.Where(d => d.IsTest == false && d.TransactionId != null && d.TransactionId != "");
        var donationsQuery = dc.Donations.Where(d => d.TransactionId != null && d.TransactionId != "");

        if (!string.IsNullOrEmpty(nameSearchQuery))
        {
            donationsQuery = donationsQuery.Where(d => (d.FirstName + " " + d.LastName).Contains(nameSearchQuery));
        }
        if (!string.IsNullOrEmpty(addressSearchQuery))
        {
            donationsQuery = donationsQuery.Where(d =>
                dc.Profiles.Where(p => p.UserId == d.UserId).Select(p => p.Address).FirstOrDefault().Contains(addressSearchQuery));
        }

        if (isStartDateValid && string.IsNullOrEmpty(endDateQuery))
        {
            donationsQuery = donationsQuery.Where(d => d.CreatedAt.Date == parsedStartDate.Date);
        }
        else if (isStartDateValid)
        {
            donationsQuery = donationsQuery.Where(d => d.CreatedAt.Date >= parsedStartDate.Date);
        }
        if (isEndDateValid)
        {
            parsedEndDate = parsedEndDate.Date.AddDays(1);
            donationsQuery = donationsQuery.Where(d => d.CreatedAt.Date <= parsedEndDate);
        }

        if (isAmountSearch)
        {
            donationsQuery = donationsQuery.Where(d => d.Amount == parsedAmount);
        }

        switch (sortExpression)
        {
            case "Name":
                donationsQuery = sortDirection == "ASC"
                    ? donationsQuery.OrderBy(d => d.FirstName + " " + d.LastName)
                    : donationsQuery.OrderByDescending(d => d.FirstName + " " + d.LastName);
                break;
            case "Address":
                donationsQuery = donationsQuery.OrderBy(d => dc.Profiles
                    .Where(p => p.UserId == d.UserId)
                    .Select(p => p.Address)
                    .FirstOrDefault());
                break;
            case "Date":
                donationsQuery = sortDirection == "ASC"
                    ? donationsQuery.OrderBy(d => d.CreatedAt)
                    : donationsQuery.OrderByDescending(d => d.CreatedAt);
                break;
            case "Amount":
                donationsQuery = sortDirection == "ASC"
                    ? donationsQuery.OrderBy(d => d.Amount)
                    : donationsQuery.OrderByDescending(d => d.Amount);
                break;
        }

        return donationsQuery.Select(d => new
        {
            Name = d.FirstName + " " + d.LastName,
            Address = dc.Profiles.Where(p => p.UserId == d.UserId).Select(p => p.Address).FirstOrDefault(),
            CreatedAt = d.CreatedAt,
            Amount = d.Amount
        });
    }

    private void BindDonations(string nameSearchQuery = null, string addressSearchQuery = null, string startDateQuery = null, string endDateQuery = null, string amountSearchQuery = null, string sortExpression = "Name", string sortDirection = "ASC")
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var donationsQuery = GetFilteredSortedDonations(dc, nameSearchQuery, addressSearchQuery, startDateQuery, endDateQuery, amountSearchQuery, sortExpression, sortDirection);

            int totalRecord = donationsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecord / PageSize);

            var pagedDonations = donationsQuery
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            RepeaterDonations.DataSource = pagedDonations;
            RepeaterDonations.DataBind();

            var pageInfo = GetPageNumbers(totalPages);
            lblPageNumbers.Text = pageInfo.PageNumbers;
            btnPrevious.Visible = CurrentPage > 1;
            btnNext.Visible = CurrentPage < totalPages;
        }
    }

    private PageInfo GetPageNumbers(int totalPages)
    {
        var pageNumbers = "";
        var pageInfo = new PageInfo();
        pageInfo.TotalPageCount = totalPages;
        lblCurrentPage.Text = string.Format("Page {0}/{1}", CurrentPage, totalPages);
        int pageRange = 2;
        int startPage = Math.Max(CurrentPage - pageRange, 1);
        int endPage = Math.Min(CurrentPage + pageRange, totalPages);
        if (CurrentPage > pageRange + 1)
        {
            pageNumbers += string.Format("<button type='button' class='prev' onclick='__doPostBack(\"Page_{0}\", \"{0}\")' commandArgument='{0}'>...</button> ", startPage - pageRange);
        }
        for (int i = startPage; i <= endPage; i++)
        {
            var className = i == CurrentPage ? "selected" : "";
            pageNumbers += string.Format("<button type='button' class='{0}' onclick='__doPostBack(\"Page_{1}\", \"{1}\")' commandArgument='{1}'>{1}</button> ", className, i);
        }
        if (CurrentPage < totalPages - pageRange)
        {
            pageNumbers += string.Format("<button type='button' class='next' onclick='__doPostBack(\"Page_{0}\", \"{0}\")' commandArgument='{0}'>...</button> ", endPage + 1);
        }

        pageInfo.PageNumbers = pageNumbers;
        return pageInfo;
    }
    private int CurrentPage
    {
        get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
        set { ViewState["CurrentPage"] = value; }
    }

    private int GetTotalRecordCount()
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            return dc.Donations.Where(d => d.IsTest == false).Count();
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        int totalPages = (int)Math.Ceiling((double)GetTotalRecordCount() / PageSize);
        if (CurrentPage < totalPages)
        {
            CurrentPage++;
            BindDonations();
        }
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            BindDonations();
        }
    }
    private void HandleSorting(string sortExpression)
    {
        string currentSortExpression = ViewState["SortExpression"] as string ?? "Name";
        string currentSortDirection = ViewState["SortDirection"] as string ?? "ASC";

        if (currentSortExpression == sortExpression)
        {
            currentSortDirection = currentSortDirection == "ASC" ? "DESC" : "ASC";
        }
        else
        {
            currentSortDirection = "ASC";
        }

        ViewState["SortExpression"] = sortExpression;
        ViewState["SortDirection"] = currentSortDirection;

        BindDonations(sortExpression: sortExpression, sortDirection: currentSortDirection);
    }


    protected void SearchButton_Click(object sender, EventArgs e)
    {
        string nameSearchQuery = SearchByName.Text.Trim();
        string addressSearchQuery = SearchByAddress.Text.Trim();
        string startDateQuery = StartDate.Text.Trim();
        string endDateQuery = EndDate.Text.Trim();
        string amountSearchQuery = SearchByAmount.Text.Trim();

        BindDonations(nameSearchQuery, addressSearchQuery, startDateQuery, endDateQuery, amountSearchQuery);
    }

    protected void ClearButton_Click(object sender, EventArgs e)
    {
        SearchByName.Text = string.Empty;
        SearchByAddress.Text = string.Empty;
        StartDate.Text = string.Empty;
        EndDate.Text = string.Empty;
        SearchByAmount.Text = string.Empty;
        BindDonations();
    }
    protected void ExportCsvButton_Click(object sender, EventArgs e)
    {
        string nameSearchQuery = SearchByName.Text.Trim();
        string addressSearchQuery = SearchByAddress.Text.Trim();
        string startDateQuery = StartDate.Text.Trim();
        string endDateQuery = EndDate.Text.Trim();
        string amountSearchQuery = SearchByAmount.Text.Trim();
        string sortExpression = ViewState["SortExpression"] as string ?? "Name";
        string sortDirection = ViewState["SortDirection"] as string ?? "ASC";

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var donations = GetFilteredSortedDonations(dc, nameSearchQuery, addressSearchQuery, startDateQuery, endDateQuery, amountSearchQuery, sortExpression, sortDirection).ToList();

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DonationsList.csv");

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                sw.WriteLine("Name,Address,Date,Amount");

                foreach (var donation in donations)
                {
                    sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                        donation.Name,
                        donation.Address,
                        donation.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                        string.Format("${0:F2}", donation.Amount)));
                }

                Response.Write(sw.ToString());
                Response.End();
            }
        }
    }

    protected void ExportPdfButton_Click(object sender, EventArgs e)
    {
        string nameSearchQuery = SearchByName.Text.Trim();
        string addressSearchQuery = SearchByAddress.Text.Trim();
        string startDateQuery = StartDate.Text.Trim();
        string endDateQuery = EndDate.Text.Trim();
        string amountSearchQuery = SearchByAmount.Text.Trim();
        string sortExpression = ViewState["SortExpression"] as string ?? "Name";
        string sortDirection = ViewState["SortDirection"] as string ?? "ASC";

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var donationsQuery = GetFilteredSortedDonations(dc, nameSearchQuery, addressSearchQuery, startDateQuery, endDateQuery, amountSearchQuery, sortExpression, sortDirection);

            var donations = donationsQuery.ToList();

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                var pdfDoc = new iTextSharp.text.Document();
                iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, ms);
                pdfDoc.Open();
                pdfDoc.Add(new iTextSharp.text.Paragraph("Donations List"));

                var table = new iTextSharp.text.pdf.PdfPTable(4);
                table.AddCell("Name");
                table.AddCell("Address");
                table.AddCell("Date");
                table.AddCell("Amount");

                foreach (var donation in donations)
                {
                    string formattedDate = donation.CreatedAt.ToString("yyyy-MM-dd");
                    string formattedAmount = string.Format("${0:F2}", donation.Amount);

                    string address = string.IsNullOrEmpty(donation.Address) ? "No Address" : donation.Address;

                    table.AddCell(donation.Name);
                    table.AddCell(address);
                    table.AddCell(formattedDate);
                    table.AddCell(formattedAmount);
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=DonationsList.pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }
    }
}




