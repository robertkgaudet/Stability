using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Error : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string errorType = Request.QueryString["ErrorType"];

		switch (errorType)
		{
			case "VictimNoAccess":
				lblErrorMessage.Text = "You are not in the right role to do to this. Administrators and Case Managers only. Request help from your team lead or system administrator.";
				break;
			case "":
				// code block
				break;
			default:
				// code block
				break;
		}
	}
}