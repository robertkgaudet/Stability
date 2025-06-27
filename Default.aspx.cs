using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : BaseWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (HttpContext.Current.User.Identity.IsAuthenticated)
		{
				//Response.Redirect("~/V1/NonProfit/Stream.aspx?organizationId=" + userOrganizationId.ToString(), false);
				Response.Redirect("/V1/Member/Default.aspx", false);
		}
	}
}