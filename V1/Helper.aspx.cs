using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Helper : BaseOrganizationWebForm
{
	public string eventName = HttpContext.Current.Request.QueryString["eventName"];
	public string volunteerLink = string.Empty;
    public string btnsendsupplies = string.Empty;
    public string btndonate = string.Empty;
	public string eventId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{

		string helperPageTitle = System.Configuration.ConfigurationManager.AppSettings["HelperPageTitle"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disaster = (from ev in dc.Events
						where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
						select ev).SingleOrDefault();

		if(disaster != null)
		{
			string pageDescription = disaster.Name+ " helpers can volunteer, deliver supplies, donate, find a non-profit to work with and much more.";
			string pageTitle = disaster.Name + helperPageTitle;

			this.Master.PageTitle = "Stability - " + pageTitle;
			this.Master.PageDescription = "Stability - " + pageDescription;
			this.Master.FbDescription = "Stability - " + pageDescription;
			this.Master.FbImage = "/V1/Images/" + disaster.ImageFileName;
			this.Master.FbImageType = "image/jpg";
			this.Master.FbSite_name = "Stability - " + pageTitle;
			this.Master.FbURL = Request.Url.AbsoluteUri;

			uc1EventHeader.PageTitle		= helperPageTitle;
			uc1EventHeader.EventName		= disaster.Name;
			uc1EventHeader.PageDescription	= pageDescription;
            btnsendsupplies					= disaster.NogginSendSuppliesLink;
            btndonate						= disaster.DonateLink;
            hypGlympse.Text					= disaster.GlympseTag;

			eventId = disaster.EventId.ToString();

            hypZelloDispatchChannel.Text = "Search Zello for \"" + disaster.ZelloPrivateChannelName + "\"";
            hypZelloDispatchChannel.NavigateUrl = disaster.ZelloPrivateChannelURL;

            hypZelloPublicChannel.Text = "Search Zello for \"" + disaster.ZelloPublicChannelName + "\"";
            hypZelloPublicChannel.NavigateUrl = disaster.ZelloPublicChannelURL;

            hypZelloSupplyChannel.Text = "Search Zello for \"" + disaster.ZelloSupplyChannelName + "\"";
            hypZelloSupplyChannel.NavigateUrl = disaster.ZelloSupplyChannelURL;

            hypGlympse.Text = "Tell us where you are, set the \"" + disaster.GlympseTag + "\" tag for tracking.";
            hypGlympse.NavigateUrl = "http://www.Glympse.com/!" + disaster.GlympseTag;

            hypNogginOCA.Text = "Click to Login to Noggin";
            hypNogginOCA.NavigateUrl = disaster.NogginLoginLink;

            hypMyVolunteerWork.NavigateUrl = "/V1/Profile/Time.aspx";

            litTotalVolunteerHours.Text = "Total Volunteer Hours";
            hypMyVolunteerWork.Text = "My Volunteer Hours";

            string volunteerStatus = VolunteerStatus.GetVolunteerStatus(userId).Value;
            if(volunteerStatus == VolunteerStatus.ApplicationComplete.Value)
            {
                //Change the Volunteer button to say volunteer pending.
                btnVolunteer.CssClass = "btn btn-danger btn-lg volunteerPending pull-left m-r-lg m-b-sm btn-block";
                btnVolunteer.Text = "VOLUNTEER";
                litVolunteerDescription.Text = "<b>APPLICATION SUBMITTED</b><p>Your volunteer application is under review. (" + VolunteerStatus.VettingStarted.Value + ")</p>";
            }
            else if(volunteerStatus == VolunteerStatus.NotYetApplied.Value)
            {
                //Change the Volunteer button to say volunteer pending.
                btnVolunteer.CssClass = "btn btn-danger btn-lg btnVolunteer pull-left m-r-lg m-b-sm btn-block";
                btnVolunteer.Text = "VOLUNTEER";
                litVolunteerDescription.Text = "<b>VOLUNTEERS NEEDED</b><p>Volunteer by clicking the Volunteer button.</p>";
            }
            else if(volunteerStatus == VolunteerStatus.VettingComplete_Failed.Value)
            {
                //Change the Volunteer button to say volunteer pending.
                btnVolunteer.CssClass = "btn btn-danger btn-lg volunteerPending pull-left m-r-lg m-b-sm btn-block";
                btnVolunteer.Text = "VOLUNTEER";
                litVolunteerDescription.Text = "<b>APPLICATION PENDING REVIEW</b><p>Please contact Stability regarding your volunteer application.</p>";
            }
            else if(volunteerStatus == VolunteerStatus.VettingComplete_Passed.Value)
            {
                //Change the Volunteer button to say volunteer pending.
                btnVolunteer.CssClass = "btn btn-danger btn-lg volunteerPassed pull-left m-r-lg m-b-sm btn-block";
                btnVolunteer.Text = "VOLUNTEER";
                litVolunteerDescription.Text = "<b>APPLICATION APPROVED</b><p>Your volunteer application has been approved, welcome aboard! You might want to print an ID card or purchase a t-shirt to show your support! (" + VolunteerStatus.VettingComplete_Passed.Value + ")</p>";
            }
            else if(volunteerStatus == VolunteerStatus.VettingStarted.Value)
            {
                //Change the Volunteer button to say volunteer pending.
                btnVolunteer.CssClass = "btn btn-danger btn-lg volunteerPending pull-left m-r-lg m-b-sm btn-block";
                btnVolunteer.Text = "VOLUNTEER";
                litVolunteerDescription.Text = "<b>APPLICATION BEING VETTED</b><p>Your volunteer application is currently being reviewed. (" + VolunteerStatus.VettingStarted.Value + ")</p>";
            }
            else
            {
                btnVolunteer.CssClass = "btn btn-danger btn-lg btnVolunteer pull-left m-r-lg m-b-sm btn-block";
                litVolunteerDescription.Text = "Tell us what you're good at and we'll help you find opportunities to get involved!";
                btnVolunteer.Text = "VOLUNTEER";
            }

			//volunteerLink = "/V1/Profile/Volunteer.aspx?eventId=" + disaster.EventId;
			if(!User.Identity.IsAuthenticated)
			{
				volunteerLink = "/V1/register.aspx";
			}
		}
	}
}