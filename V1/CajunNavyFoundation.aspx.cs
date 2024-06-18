using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_CajunNavyFoundation : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.PageTitle			= "Cajun Navy Hurricane Michael Relief Fund";
		this.Master.PageDescription		= "Hurricane Michael caused widespread damage and power outages in communities across Florida's Panhandle. Now a Tropical Storm, Michael is forecast to bring strong winds and rains to Southern Georgia and the Carolinas. This fund supports preparation, relief efforts in the form of emergency supplies like food, water, and medicine as well as longer-term recovery assistance to help residents recover and rebuild.";
		this.Master.FbDescription		= "Hurricane Michael caused widespread damage and power outages in communities across Florida's Panhandle. Now a Tropical Storm, Michael is forecast to bring strong winds and rains to Southern Georgia and the Carolinas. This fund supports preparation, relief efforts in the form of emergency supplies like food, water, and medicine as well as longer-term recovery assistance to help residents recover and rebuild.";
		this.Master.FbImage				= "/V1/Images/MichaelImage.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbSite_name			= "Cajun Navy Hurricane Michael Relief Fund";
		this.Master.FbURL				= Request.Url.AbsoluteUri;
	}
}