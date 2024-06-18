using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class V1_Survivor : BaseOrganizationWebForm
{
	public string eventName = HttpContext.Current.Request.QueryString["eventName"];
    public string btnRebuild = string.Empty;
    public string btnrescue = string.Empty;
    public string btnrequestsupplies = string.Empty;
	public string eventId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		string survivorPageTitle = System.Configuration.ConfigurationManager.AppSettings["SurvivorPageTitle"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disaster = (from ev in dc.Events
						where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
						select ev).SingleOrDefault();

		if(disaster != null)
		{
			string pageDescription = disaster.Name+ " Survivors can request help, connect with non-profits, find volunteers, track their home rebuild and find supplies.";
			string pageTitle = disaster.Name + survivorPageTitle;

			this.Master.PageTitle = "Stability - " + pageTitle;
			this.Master.PageDescription = "Stability - " + pageDescription;
			this.Master.FbDescription = "Stability - " + pageDescription;
			this.Master.FbImage = "/V1/Images/" + disaster.ImageFileName;
			this.Master.FbImageType = "image/jpg";
			this.Master.FbSite_name = "Stability - " + pageTitle;
			this.Master.FbURL = Request.Url.AbsoluteUri;

			uc1EventHeader.PageTitle = survivorPageTitle;
			uc1EventHeader.EventName = disaster.Name;
			uc1EventHeader.PageDescription = pageDescription;

			var homeCount = from c in dc.Rebuilds
								where c.EventId == disaster.EventId
								select c;
			
			homeCount = homeCount.Where(c => c.CreatedBy == userId);
			hypRebuildingHomesCount.Text = "Rebuilding " + homeCount.Count().ToString() + " homes.";
			hypRebuildingHomesCount.NavigateUrl = "~/V1/Profile/Default.aspx";
			litFollowingHomesCount.Text = "Following " + homeCount.Count().ToString() + " homes.";

			
            btnrequestsupplies				= disaster.NogginRequestSuppliesLink;
            btnrescue						= disaster.NogginRequestRescueLink;
			eventId							= disaster.EventId.ToString();
		}
	}
}