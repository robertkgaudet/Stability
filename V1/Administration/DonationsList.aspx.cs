using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Administration_DonationsList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindDonations(nameSearchQuery: null, addressSearchQuery: null, dateSearchQuery: null, amountSearchQuery: null, sortExpression: "Name", sortDirection: "ASC");
        }
    }

    private void BindDonations(string nameSearchQuery = null, string addressSearchQuery = null, string dateSearchQuery = null, string amountSearchQuery = null, string sortExpression = "Name", string sortDirection = "ASC")
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            DateTime parsedDate;
            bool isDateSearch = DateTime.TryParse(dateSearchQuery, out parsedDate);
            decimal parsedAmount;
            bool isAmountSearch = decimal.TryParse(amountSearchQuery, out parsedAmount);

            var donationsQuery = dc.Donations
                                   .Where(d => d.IsTest == false &&
                                               (string.IsNullOrEmpty(nameSearchQuery) || (d.FirstName + " " + d.LastName).Contains(nameSearchQuery)) &&
                                               (string.IsNullOrEmpty(addressSearchQuery) || dc.Profiles.Where(p => p.UserId == d.UserId).Select(p => p.Address).FirstOrDefault().Contains(addressSearchQuery)) &&
                                               (string.IsNullOrEmpty(dateSearchQuery) || (isDateSearch && d.CreatedAt.Date == parsedDate.Date)) &&
                                               (string.IsNullOrEmpty(amountSearchQuery) || (isAmountSearch && d.Amount == parsedAmount)))
                                   .Select(d => new
                                   {
                                       Name = d.FirstName + " " + d.LastName,
                                       d.CreatedAt,
                                       d.Amount,
                                       Address = dc.Profiles.Where(p => p.UserId == d.UserId).Select(p => p.Address).FirstOrDefault()
                                   });

            if (sortExpression != null)
            {
                switch (sortExpression)
                {
                    case "Name":
                        donationsQuery = sortDirection == "ASC" ? donationsQuery.OrderBy(d => d.Name) : donationsQuery.OrderByDescending(d => d.Name);
                        break;
                        // Add other sorting cases as needed
                }
            }
            GridView.DataSource = donationsQuery.ToList();
            GridView.DataBind();
        }
    }
    protected void GridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        string sortDirection = e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";

        BindDonations(nameSearchQuery: null, addressSearchQuery: null, dateSearchQuery: null, amountSearchQuery: null, sortExpression: sortExpression, sortDirection: sortDirection);
    }


    protected void SearchButton_Click(object sender, EventArgs e)
    {
        string nameSearchQuery = SearchByName.Text.Trim();
        string addressSearchQuery = SearchByAddress.Text.Trim();
        string dateSearchQuery = SearchByDate.Text.Trim();
        string amountSearchQuery = SearchByAmount.Text.Trim();

        BindDonations(nameSearchQuery, addressSearchQuery, dateSearchQuery, amountSearchQuery);
    }

    protected void ClearButton_Click(object sender, EventArgs e)
    {
        SearchByName.Text = string.Empty;
        SearchByAddress.Text = string.Empty;
        SearchByDate.Text = string.Empty;
        SearchByAmount.Text = string.Empty;
        BindDonations();
    }

    protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView.PageIndex = e.NewPageIndex;
        BindDonations(SearchByName.Text.Trim());
    }

    protected void ExportPdfButton_Click(object sender, EventArgs e)
    {
        string nameSearchQuery = SearchByName.Text.Trim();
        string addressSearchQuery = SearchByAddress.Text.Trim();
        string dateSearchQuery = SearchByDate.Text.Trim();
        string amountSearchQuery = SearchByAmount.Text.Trim();

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            DateTime parsedDate;
            bool isDateSearch = DateTime.TryParse(dateSearchQuery, out parsedDate);

            var donations = dc.Donations
                              .Where(d => d.IsTest == false &&
                                          (string.IsNullOrEmpty(nameSearchQuery) ||
                                           (d.FirstName + " " + d.LastName).Contains(nameSearchQuery)) &&
                                          (string.IsNullOrEmpty(addressSearchQuery) ||
                                           dc.Profiles.Where(p => p.UserId == d.UserId)
                                                      .Select(p => p.Address)
                                                      .FirstOrDefault()
                                                      .Contains(addressSearchQuery)) &&
                                          (string.IsNullOrEmpty(dateSearchQuery) ||
                                           (isDateSearch && d.CreatedAt.Date == parsedDate.Date)) &&
                                          (string.IsNullOrEmpty(amountSearchQuery) ||
                                           d.Amount.ToString().Contains(amountSearchQuery)))
                              .Select(d => new
                              {
                                  Name = d.FirstName + " " + d.LastName,
                                  d.CreatedAt,
                                  d.Amount,
                                  Address = dc.Profiles
                                               .Where(p => p.UserId == d.UserId)
                                               .Select(p => p.Address)
                                               .FirstOrDefault()
                              })
                              .OrderBy(d => d.Name)
                              .ToList();

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

                    table.AddCell(donation.Name);
                    table.AddCell(donation.Address);
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

    protected void ExportCsvButton_Click(object sender, EventArgs e)
    {
        string searchQuery = SearchByName.Text.Trim();
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var donations = dc.Donations
                              .Where(d => d.IsTest == false &&
                                          (string.IsNullOrEmpty(searchQuery) ||
                                           (d.FirstName + " " + d.LastName).Contains(searchQuery)
                                          || d.Amount.ToString().Contains(searchQuery) ||
                                           dc.Profiles
                                              .Where(p => p.UserId == d.UserId)
                                              .Select(p => p.Address)
                                              .FirstOrDefault()
                                              .Contains(searchQuery)))
                              .Select(d => new
                              {
                                  Name = d.FirstName + " " + d.LastName,
                                  d.CreatedAt,
                                  d.Amount,
                                  Address = dc.Profiles
                                               .Where(p => p.UserId == d.UserId)
                                               .Select(p => p.Address)
                                               .FirstOrDefault()
                              })
                              .OrderBy(d => d.Name)
                              .ToList();

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
}


