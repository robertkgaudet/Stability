using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Member_PeopleSearch : BaseWebForm
{
    public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
    public string _receiverUserId = string.Empty;
    public string _sendingUserId = string.Empty;
    public string _hideConnectionButton = string.Empty;
    string searchTerm = String.Empty;
    public string SearchKeyword { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!IsPostBack)
            {
                searchTerm = Request.QueryString["searchTerm"];
                if (!String.IsNullOrEmpty(searchTerm))
                {
                    txtSearchBox.Text = searchTerm;
                    LoadConnections(searchTerm, 0);
                }
                else
                {
                    LoadConnections(searchTerm, 50);
                }
                ucMemberNavigation.UserId = userId.ToString();
            }
        }
        else
        {
            Response.Redirect("/SignIn");
        }
    }

    public void LoadConnections(string searchTerm, int recordCount)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        _hideConnectionButton = "style='display:none;'";
        List<Tools.FriendInfo> PeopleSearch = Tools.PeopleSearch(searchTerm, recordCount);
        ConnectionsDataList.DataSource = PeopleSearch;
        ConnectionsDataList.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        searchTerm = txtSearchBox.Text;
        if (!String.IsNullOrEmpty(searchTerm))
        {
            LoadConnections(searchTerm, 0);
        }
    }

    protected void ConnectionsDataList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RepeaterItem dataItem = (RepeaterItem)e.Item;

            bool PassedVetting = (bool)DataBinder.Eval(dataItem.DataItem, "PassedVetting");
            Literal litPassedVetting = (Literal)e.Item.FindControl("litPassedVetting");

            string passedVettingStyle = " fa-pending-color";
            string vettingMessage = "";
            if (PassedVetting)
            {
                vettingMessage = "Vetting Complete";
                passedVettingStyle = " fa-approved-color";
            }

            litPassedVetting.Text = "<i class=\"fa fa-id-badge pe-1x float-right" + passedVettingStyle + "\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"" + vettingMessage + "\"></i>";

            var ucTeamLogo = (V1_UserControls_TeamLogo)e.Item.FindControl("ucTeamLogo");
            if (ucTeamLogo != null)
            {
                ucTeamLogo.UserId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
                ucTeamLogo.SearchKeyword = searchTerm; 
                ucTeamLogo.LoadNameWithBadges();
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                HighlightLiteral(dataItem, "CityState", searchTerm);
                HighlightLiteral(dataItem, "TeamName", searchTerm);
                HighlightLiteral(dataItem, "ProfileTitle", searchTerm);
                HighlightLiteral(dataItem, "ProfileDescription", searchTerm);
            }
        }
    }

    private void HighlightLiteral(RepeaterItem item, string controlId, string keyword)
    {
        var ctrl = item.FindControl(controlId) as Literal;
        if (ctrl != null && !string.IsNullOrEmpty(ctrl.Text))
        {
            ctrl.Text = HighlightSearchTerm(ctrl.Text, keyword);
        }
    }

    public string HighlightSearchTerm(string input, string keyword)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(keyword)) return input;
        return Regex.Replace(input, Regex.Escape(keyword), "<span style='background-color:yellow'><b>$0</b></span>", RegexOptions.IgnoreCase);
    }
}
