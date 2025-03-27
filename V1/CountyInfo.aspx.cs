using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_CountyInfo : BaseOrganizationWebForm
{
    private const int PageSize = 10;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string stateCode = Request.QueryString["stateCode"];
            string countyId = Request.QueryString["countyId"];
            string eventId = Request.QueryString["eventId"];

            InitializeEmergencyManagementInfo(stateCode, countyId);
            LoadDisasterLocations(countyId, CurrentPage, PageSize, null, null);
        }
    }

    private void InitializeEmergencyManagementInfo(string stateCode, string countyId)
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var emergencyInfoQuery = (from em in dc.EmergencyManagments
                                      join s in dc.USStates on em.StatesId equals s.StatesId
                                      where s.Code == stateCode && em.CountyId == new Guid(countyId)
                                      select em).FirstOrDefault();

            if (emergencyInfoQuery != null)
            {
                litWebsite.Text = emergencyInfoQuery.Website;
                litEOCName.Text = emergencyInfoQuery.EOCName;
                litEMName.Text = emergencyInfoQuery.EmergencyManagerName;
                litPhoneNumber.Text = emergencyInfoQuery.PhoneNumber;
                divInfo.Visible = true;
                divInfoMessage.Visible = false;
            }
            else
            {
                divInfo.Visible = false;
                divInfoMessage.Visible = true;
            }
        }
    }
    private void LoadDisasterLocations(string countyId, int pageNumber, int pageSize, string sortColumn, string sortDirection)
    {
        Guid countyGuid = new Guid(countyId);

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var query = dc.GetDisasterLocationsByCountys(countyGuid, pageNumber, pageSize).AsQueryable();
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = sortDirection == "ASC" ? query.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null))
                                                : query.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null));
            }
            var locations = query.ToList();

            gvLocations.DataSource = locations;
            gvLocations.DataBind();
            gvLocations.Visible = locations.Any();
            pnlNoLocations.Visible = !locations.Any();

            lblCurrentPage.InnerText = "Page " + pageNumber;
            btnPrevious.Enabled = pageNumber > 1;
            btnNext.Enabled = locations.Count == pageSize;
        }
    }

    private int CurrentPage
    {
        get
        {
            return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
        }
        set
        {
            ViewState["CurrentPage"] = value;
        }
    }


    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            LoadDisasterLocations(Request.QueryString["countyId"], CurrentPage, PageSize, ViewState["SortColumn"] as string, ViewState["SortDirection"] as string);
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        CurrentPage++;
        LoadDisasterLocations(Request.QueryString["countyId"], CurrentPage, PageSize, ViewState["SortColumn"] as string, ViewState["SortDirection"] as string);
    }

    protected void gvLocations_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortColumn = e.SortExpression;
        string sortDirection = ViewState["SortDirection"] as string == "ASC" ? "DESC" : "ASC";

        ViewState["SortColumn"] = sortColumn;
        ViewState["SortDirection"] = sortDirection;

        LoadDisasterLocations(Request.QueryString["countyId"], CurrentPage, PageSize, sortColumn, sortDirection);
    }
}
