using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class S1_Profile_EditProfile : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.HideCategoryList = true;
		Master.PageTitle		= "Edit a Stability User Profile";
		Master.FbURL			= Request.Url.AbsoluteUri;
		Master.FbDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.PageDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.FbImage			= "/S1/Images/CrowdReliefFBPost.jpg";
		Master.FbImageType		= "image/jpg";
		Master.FbSite_name		= "Stability Disaster Aid Platform";
	}
}