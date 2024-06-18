using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class CaseManagement_AddLocation : BaseOrganizationWebForm
{
	public string _userId;

	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.PageTitle			= "Stability - Add New Location";
		this.Master.PageDescription		= "";
		this.Master.FbDescription		= "";
		this.Master.FbImage				= "Images/HurricaneMichael.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbSite_name			= "Stability - Add New Location";
		this.Master.FbURL				= Request.Url.AbsoluteUri;
		_userId = Request.QueryString["userId"];

		if (!IsPostBack)
		{
			ListItemCollection statesList = new ListItemCollection();
			foreach (string state in States.Names())
			{
				ListItem li = new ListItem(state, state);
				statesList.Add(li);
			}

			ddlState.DataSource = statesList;
			ddlState.DataBind();
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		//Check the id, if the location already exists, send to the location edit page.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (!String.IsNullOrEmpty(hidAddressData.Value))
		{
			string hiddenAddressData = hidAddressData.Value;
			string[] addressArray = hiddenAddressData.Split(Convert.ToChar("|"));

			Response.Redirect("/CaseManagement/EditHome.aspx?addressId=" + addressArray[13] + "&userId=" + _userId);
		}
		else
		{
			//Error sending data.
		}
	}
}