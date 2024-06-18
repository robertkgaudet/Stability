using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Template : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.PageTitle			= "Stability - Cajun Navy Hurricane Michael Relief Fund";
		this.Master.PageDescription		= "";
		this.Master.FbDescription		= "";
		this.Master.FbImage				= "Images/HurricaneMichael.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbSite_name			= "Stability - Cajun Navy Hurricane Michael Relief Fund";
		this.Master.FbURL				= Request.Url.AbsoluteUri;
	}
}