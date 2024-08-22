using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_Member_PeopleSearch : BaseWebForm
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string _receiverUserId = string.Empty;
	public string _sendingUserId = string.Empty;
	public string _hideConnectionButton = string.Empty;
	string searchTerm = String.Empty;
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
		//MY ACCEPTED CONNECTIONS
		List<Tools.FriendInfo> PeopleSearch = Tools.PeopleSearch(searchTerm, recordCount);
		ConnectionsDataList.DataSource = PeopleSearch;
		ConnectionsDataList.DataBind();
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		searchTerm = txtSearchBox.Text;
		if (!String.IsNullOrEmpty(searchTerm))
		{
			LoadConnections(searchTerm, 0); //0 returns all matching records
		}
	}

	protected void ConnectionsDataList_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Boolean PassedVetting = (Boolean)DataBinder.Eval(dataItem.DataItem, "PassedVetting");
			Literal litPassedVetting = (Literal)e.Item.FindControl("litPassedVetting");

			string passedVettingStyle = " fa-pending-color";
			string vettingMessage = string.Empty;
			if (PassedVetting)
			{
				vettingMessage = "Vetting Complete";
				passedVettingStyle = " fa-approved-color";
			}
			litPassedVetting.Text = "<i class=\"fa fa-id-badge pe-1x float-right" + passedVettingStyle + "\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"" + vettingMessage + "\"></i>";

		}
	}
}