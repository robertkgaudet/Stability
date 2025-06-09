using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_SpinningLogo : System.Web.UI.UserControl
{
	public string LogoSizeCssClass { get; set; }
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (!string.IsNullOrEmpty(LogoSizeCssClass))
			{
				rotatingLogoDiv.Attributes["class"] += " " + LogoSizeCssClass;
			}
		}
	}
}