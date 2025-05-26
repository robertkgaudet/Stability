using CrowdRelief;
using System;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_RespondToEvent : BaseOrganizationWebForm
{
    public string disasterDropDown = string.Empty;
    public string preselectedDisasterJQuery = string.Empty;
    public string eventId = string.Empty;
    public string organizationId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        eventId = Request.QueryString["eventId"];
        organizationId = Request.QueryString["organizationId"];
        //If org and eventid already exist, then don't allow them here.
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        if (!IsPostBack)
        {
            //Load the address lookup state list.
            ListItemCollection statesList = new ListItemCollection();
            foreach (string state in States.Names())
            {
                ListItem li = new ListItem(state, state);
                statesList.Add(li);
            }

            ddlState1.DataSource = statesList;
            ddlState1.DataBind();

            if (String.IsNullOrEmpty(eventId))
            {
                //No event id was sent.
                divCreateCause.Visible = false;
                divSelectEvent.Visible = true;
                LoadDisasters();
            }
            else
            {
                Guid guidEventId = new Guid(eventId);
                divCreateCause.Visible = true;
                divSelectEvent.Visible = false;
                //var states = from s in dc.USStates
                //			 //join es in dc.EventStates on s.StatesId equals es.StatesId
                //		//	 where es.EventId == guidEventId
                //			orderby s.Name
                //			 select new { s.StatesId, s.Name };

                //ddlState.DataSource = states.ToList();
                //ddlState.DataBind();
                //ddlState.Items.Insert(0, new ListItem("Choose a State", ""));

                var disaster = (from ev in dc.Events
                                where ev.EventId == guidEventId
                                select new { ev.Name }).SingleOrDefault();

                litEventName.Text = disaster.Name;

                //LoadDisasters();
            }

            var profile = (from p in dc.Profiles
                           join u in dc.aspnet_Memberships on p.UserId equals u.UserId
                           where p.UserId == userId
                           select new { fullName = p.Firstname + " " + p.Lastname, u.Email, p.PhoneNumber }).SingleOrDefault();

            if (profile != null)
            {
                txtEmailAddress.Value = profile.Email;
                txtPOCFullname.Value = profile.fullName;
                txtPhonenumber.Value = profile.PhoneNumber;
            }
        }
    }

    [WebMethod]
    public static bool IsURLFriendlyNameUnique(string urlFriendlyName)
    {
        using (var dc = new CrowdReliefDBDataContext()) // Replace with your actual DataContext
        {
            // Check if the URLFriendlyCampaignName already exists
            return !dc.OrganizationEvents.Any(o => o.URLFriendlyCampaignName == urlFriendlyName);
        }
    }
    public void LoadDisasters()
    {
        eventId = Request.QueryString["eventId"];
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var disasters = from d in dc.Events
                        orderby d.BeginDate descending
                        where d.IsActive == true
                        select new { d };

        int idNumber = 0;
        foreach (var disaster in disasters)
        {
            string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
            disasterDropDown = disasterDropDown + "<li id=\"" + disaster.d.EventId + "\"><a href=\"#\">" + disasterDate + " - " + disaster.d.Name + "</a></li>" + Environment.NewLine;
            idNumber = idNumber + 1;
        }
        if (!String.IsNullOrEmpty(eventId))
        {
            //Hide the Dropdown and show the selected disaster
            var disaster = (from d in dc.Events
                            where d.EventId == new Guid(eventId)
                            orderby d.BeginDate descending
                            select new { d }).SingleOrDefault();

            string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
            preselectedDisasterJQuery = "$(\"#btn-dropdown.disasterEvent\").html('" + disaster.d.Name + " - " + disasterDate + "');";
            hidEventId.Value = eventId;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Guid eventId = new Guid(Request.QueryString["eventId"]);
        organizationId = Request.QueryString["organizationId"];
        string addressId = string.Empty;
        Guid stateId = Guid.Empty;
        Guid countyId = Guid.Empty;
        string address = string.Empty;
        string city = string.Empty;
        string zip = string.Empty;

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        if (!String.IsNullOrEmpty(hidAddressData.Value))
        {
            string hiddenAddressData = hidAddressData.Value;
            string[] addressArray = hiddenAddressData.Split(Convert.ToChar("|"));
            addressId = addressArray[13];
            address = addressArray[3] + " " + addressArray[4];
            city = addressArray[5];
            zip = addressArray[8];
            stateId = (from s in dc.USStates where s.Name == addressArray[6] select s.StatesId).SingleOrDefault();

            string countyName = addressArray[9].Replace("County", "").Trim();
            countyName = countyName.Replace("Parish", "").Trim();
            countyId = (from s in dc.Counties
                        where s.Name == countyName && s.StateId == stateId
                        select s.CountyId).SingleOrDefault();
            var addressesToUpdate = from a in dc.Addresses
                                    where a.County == countyName
                                    select a;

            foreach (var addressUpdate in addressesToUpdate)
            {
                addressUpdate.CountyId = countyId;
            }

            dc.SubmitChanges();
        }
        string pointOfContactName = txtPOCFullname.Value;
        string campaignName = txtCampaignName.Value;
        string URLFriendlyCampaignName = txtURLFriendlyCampaignName.Value.Replace(" ", "").Replace("'", "").Replace("\"", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(",", "").Replace("!", "").Replace("-", "").Replace(":", "").Replace("+", "").Replace("&", "").Replace("*", "");
        bool? isVoad = chkVoad.Checked;
        string volunteerInstructions = Server.HtmlEncode(txtVolunteerInstructions.Text);
        string POCName = txtPOCFullname.Value;
        string phoneNumber = txtPhonenumber.Value;
        string emailAddress = txtEmailAddress.Value;

        OrganizationEvent organizationEvent = new OrganizationEvent();
        Guid organizationEventId = Guid.NewGuid();
        if (!string.IsNullOrEmpty(addressId))
        {
            organizationEvent.AddressId = new Guid(addressId);
            organizationEvent.StagingAddress = address;
            organizationEvent.StagingCity = city;
            organizationEvent.StagingCountyId = countyId;
            organizationEvent.StagingStateId = stateId;
            organizationEvent.StagingZipCode = zip;
        }
        organizationEvent.OrganizationEventId = organizationEventId;
        organizationEvent.EventId = eventId;
        organizationEvent.OrganizationId = new Guid(organizationId);
        organizationEvent.PointOfContactName = pointOfContactName;
        organizationEvent.CampaignName = campaignName;
        organizationEvent.URLFriendlyCampaignName = URLFriendlyCampaignName;
        organizationEvent.VolunteerInstructions = volunteerInstructions;
        organizationEvent.PointOfContactName = POCName;
        organizationEvent.PhoneNumber = phoneNumber;
        organizationEvent.IsActive = true;
        organizationEvent.AcceptsVolunteers = true;
        organizationEvent.Email = emailAddress;
        organizationEvent.Createdby = userId;
        organizationEvent.CreatedOn = DateTime.Now;
        organizationEvent.IsVoadMember = isVoad;

        dc.OrganizationEvents.InsertOnSubmit(organizationEvent);
        dc.SubmitChanges();

        var userEventCheck = from ue in dc.UserEvents
                             where ue.EventId == eventId && ue.UserId == userId
                             select ue;

        if (userEventCheck.Count() == 0)
        {
            UserEvent userEvent = new UserEvent();
            userEvent.UserId = userId;
            userEvent.EventId = eventId;
            userEvent.UserEventId = Guid.NewGuid();
            userEvent.IsVictim = false;
            dc.UserEvents.InsertOnSubmit(userEvent);
            dc.SubmitChanges();
        }

        if (chkNotification.Checked)
        {
            string accountSid = System.Configuration.ConfigurationManager.AppSettings["twilioAccountSID"].ToString();
            string authToken = System.Configuration.ConfigurationManager.AppSettings["twilioAuthToken"].ToString(); ;
            string fromNumber = System.Configuration.ConfigurationManager.AppSettings["twilioPhoneNumber"].ToString();

            var teamMembers = (from a in dc.UserOrganizations
                               join b in dc.aspnet_Users
                               on a.UserId equals b.UserId
                               join c in dc.Profiles
                               on a.UserId equals c.UserId
                               where a.OrganizationId == new Guid(organizationId) && c.ReceiveSMSNotifications == true && a.IsEnabled == true
                               select c.PhoneNumber).ToArray();

            string messageBody = "Dear Member, We are excited to inform you that a new deployment named " + campaignName + " has been created into your organization.";

            // Create an instance of Tools and send the SMS
            var tools = new Tools(accountSid, authToken, fromNumber);
            tools.SendSms(messageBody, teamMembers);
        }
        BaseWebForm.AddNotifications(
   NotificationType.DeploymentIsCreated,
   FeatureTypeEnum.Deployments,
   "deployment created msg ",
   "Deployment has been successfully created for the organization.",
      organizationEvent.OrganizationId,
   true,
   organizationEvent.OrganizationId.ToString()

);
        Response.Redirect("/V1/NonProfitAdministration/PositionsNeeded.aspx?organizationEventId=" + organizationEventId);
    }
    protected void btnSubmit_Cancel(object sender, EventArgs e)
    {
        Response.Redirect("/V1/NonProfit/Deployments.aspx?organizationId=" + organizationId);
    }
}