using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_MasterPages_1_Column_Narrow : System.Web.UI.MasterPage
{
	public string _coverImage = "";
	protected void Page_Load(object sender, EventArgs e)
	{
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "Stability_Cover_V3.jpg";
	}
}