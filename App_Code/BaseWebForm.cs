using System;
using System.Linq;
using System.Web.Security;
using System.Web.SessionState;

/// <summary>
/// Summary description for BaseWebForm
/// </summary>
public class BaseWebForm : System.Web.UI.Page, IRequiresSessionState
{
    private Guid m_userId;
    private Guid m_userOrganizationId;
    public BaseWebForm()
    {
        if (User.Identity.IsAuthenticated)
        {
            m_userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

            //Get this users organization id.
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var userOrganization = (from uo in dc.UserOrganizations
                                    join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                    where uo.UserId == m_userId && o.IsActive == true && uo.Status== (int)RequestStatus.Approved && uo.Status == (int)RequestStatus.Pending
                                    select new { o.Name, o.OrganizationId }).Take(1).SingleOrDefault();

            if (userOrganization != null)
            {
                m_userOrganizationId = userOrganization.OrganizationId;
            }
        }
    }

    public static string FormatPhoneNumberAsLink(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit))
        {
            return string.Empty; // Return an empty string if the input is null, empty, or not a valid 10-digit number
        }

        string formattedNumber = String.Format("{0:(###) ###-####}", Convert.ToInt64(phoneNumber));
        return "<a href=\"tel:" + phoneNumber + "\">" + formattedNumber + "</a>";
    }

    public string GetsDisastersForDropDown()
    {
        string preselectedDisasterJQuery = string.Empty;
        string returnedEventName = string.Empty;
        return GetsDisastersForDropDown(string.Empty, out preselectedDisasterJQuery, out returnedEventName);
    }
    public string GetsDisastersForDropDown(string eventId)
    {
        string preselectedDisasterJQuery = string.Empty;
        string returnedEventName = string.Empty;
        return GetsDisastersForDropDown(eventId, out preselectedDisasterJQuery, out returnedEventName);
    }
    public string GetsDisastersForDropDown(string eventId, out string preselectedDisasterJQuery, out string returnedEventName)
    {
        string disasterDropDown = string.Empty;
        returnedEventName = string.Empty;
        preselectedDisasterJQuery = string.Empty;

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var disasters = from d in dc.Events
                        orderby d.BeginDate descending
                        select new { d };

        int idNumber = 0;
        foreach (var disaster in disasters)
        {
            string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
            disasterDropDown = disasterDropDown + "<option value=\"" + disaster.d.URLFriendlyName + "\">" + disasterDate + " - " + disaster.d.Name + "</option>" + Environment.NewLine;
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

            var eventDetails = (from ev in dc.Events
                                where ev.EventId == new Guid(eventId)
                                select ev).SingleOrDefault();

            if (eventDetails != null)
            {
                returnedEventName = eventDetails.Name;
            }
        }

        return disasterDropDown;
    }
    public static bool AddNotifications(NotificationType notificationType, FeatureTypeEnum featureType, string title, string description,
                                     Guid recipientUserId, bool postToStream, string redirectURLParameters,Guid orgId)
    {
        try
        {
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var featureTypeId = dc.FeatureTypes.Where(x => x.FeatureName == featureType.ToString()).Select(x => x.FeatureTypeId).FirstOrDefault();
                if (featureTypeId != null)
                {
                    Notification notification = new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        NotificationType = (int)notificationType,
                        Title = title,
                        Description = description,
                        FeatureTypeId = featureTypeId,
                        SenderUserId = new Guid(Membership.GetUser().ProviderUserKey.ToString()),
                        RecipientUserId = recipientUserId,
                        PostToStream = postToStream,
                        CreatedOn = DateTime.Now,
                        RedirectURLParameters = redirectURLParameters,
                        OrganizationId = orgId  
                    };
                    dc.Notifications.InsertOnSubmit(notification);
                    dc.SubmitChanges();
                    return true;
                }
                return false;
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error occurred: " + ex.Message);
            return false;
        }
    }

    public Guid userOrganizationId
    {
        get
        { return m_userOrganizationId; }
        set
        { m_userOrganizationId = value; }
    }
    public Guid userId
    {
        get
        { return m_userId; }
        set
        { m_userId = value; }
    }
}