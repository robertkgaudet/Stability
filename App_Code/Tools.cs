using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Threading.Tasks;
using Twilio.Types;

namespace CrowdRelief
{

    /// <summary>
    /// Summary description for Tools
    /// </summary>
    public class Tools
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;
        //public static async Task SendAsyncSMS(string[] args)
        //{
        //	// Find your Account SID and Auth Token at twilio.com/console
        //	// and set the environment variables. See http://twil.io/secure
        //	string accountSid = Environment.GetEnvironmentVariable("twilioAccountSID");
        //	string authToken = Environment.GetEnvironmentVariable("twilioAuthToken");

        //	TwilioClient.Init(accountSid, authToken);

        //	var message = await MessageResource.CreateAsync(
        //		body: "Join Earth's mightiest heroes. Like Kevin Bacon.",
        //		from: new Twilio.Types.PhoneNumber("+13372704049"),
        //		to: new Twilio.Types.PhoneNumber("+13372580572"));

        //	Console.WriteLine(message.Body);
        //}

        // Method to send SMS to an array of phone numbers
        public void SendSms(string messageBody, string[] phoneNumbers)
        {
            foreach (var phoneNumber in phoneNumbers)
            {
                try
                {
                    var message = MessageResource.Create(
                        body: messageBody,
                        from: new PhoneNumber(_fromNumber),
                        to: new PhoneNumber(phoneNumber)
                    );

                    Console.WriteLine("Message sent to " + phoneNumber + " : SID " + message.Sid);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send message to" + phoneNumber + " : " + ex.Message);
                }
            }
        }

        public class PhoneNumberFormatter
        {
            public static string FormatPhoneNumber(string phoneNumber)
            {
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    return phoneNumber;
                }

                // Use a regular expression to format the phone number
                return Regex.Replace(phoneNumber, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
            }

            public static string CreateClickablePhoneNumber(string phoneNumber)
            {
                string formattedPhoneNumber = FormatPhoneNumber(phoneNumber);
                string clickablePhoneNumber = string.Format("<a href=\"tel:{0}\">{1}</a>", phoneNumber, formattedPhoneNumber);
                return clickablePhoneNumber;
            }
        }
        public Tools(string accountSid, string authToken, string fromNumber)
        {
            //
            // TODO: Add constructor logic here
            //
            _accountSid = accountSid;
            _authToken = authToken;
            _fromNumber = fromNumber;

            // Initialize the Twilio client with credentials
            TwilioClient.Init(_accountSid, _authToken);
        }

        public enum TransactionStatus
        {
            Started,
            Pending,
            Succeeded,
            Failed,
            Returned
        }

        public enum TransactionType
        {
            CreditCard = 1,
            Paypal,
            Venmo,
            CashApp
        }

        public enum FriendStatus
        {
            AddConnection,
            Blocked,
            Connected,
            Delete,
            Pending
        }

		public class FriendInfo
		{
			public string FullName { get; set; }
			public string ProfileImage { get; set; }
			public DateTime AcceptedOn { get; set; }
			public Guid UserId { get; set; }
			public bool? PassedVetting { get; set; }
			public string ProfileDescription { get; set; }
			public string ProfileTitle { get; set; }
			public string CityState { get; set; }
			public string TeamName { get; set; }
			public DateTime CreateDate { get; set; }
		}

        public static List<FriendInfo> PeopleSearch(string searchTerm, int itemCountToReturn)
        {
            //Guid UserId = new Guid(userId);
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            // All DB Users
            var profiles = from profile in dc.Profiles
                           join a in dc.aspnet_Memberships on profile.UserId equals a.UserId
                           where
                           //profile.PassedVetting == null || profile.PassedVetting == true
                           //&&
                           a.IsLockedOut == false && a.IsApproved == true
                           orderby a.CreateDate descending
                           select new FriendInfo
                           {
                               UserId = profile.UserId,
                               ProfileImage = (from p in dc.Photos
                                               join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                               where ph.UserId == profile.UserId
                                               orderby p.CreatedOn descending
                                               select p.FilenameCropped).Take(1).SingleOrDefault(),
                               FullName = profile.Firstname + " " + profile.Lastname,
                               PassedVetting = profile.PassedVetting == null ? false : profile.PassedVetting,
                               ProfileDescription = profile.Description,
                               CreateDate = a.CreateDate,
                               ProfileTitle = profile.Title,
                               CityState = profile.City + " " + profile.State,
                               TeamName = (from uo in dc.UserOrganizations
                                           join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                           where uo.UserId == profile.UserId && uo.IsEnabled == true
                                           select o.Name).Take(1).SingleOrDefault()
                           };

            if (searchTerm != null)
            {
                string lowerSearch = searchTerm.ToLower();

                var matchedProfiles = profiles
                    .Where(item =>
                        item.FullName.ToLower().Contains(lowerSearch) ||
                        item.ProfileDescription.ToLower().Contains(lowerSearch) ||
                        item.ProfileTitle.ToLower().Contains(lowerSearch) ||
                        item.TeamName.ToLower().Contains(lowerSearch) ||
                        item.CityState.ToLower().Contains(lowerSearch))
                    .ToList();

                var ordered = matchedProfiles
        .Select(p =>
        {
            var fullNameParts = p.FullName.Split(' ');
            string firstName = fullNameParts.FirstOrDefault().ToLower() ?? "";
            string lastName = fullNameParts.Length > 1 ? fullNameParts.Last().ToLower() : "";
            string fullName = p.FullName.ToLower();

            int matchLevel;

            if (firstName == lowerSearch || lastName == lowerSearch)
                matchLevel = 3;
            else if (fullName == lowerSearch)
                matchLevel = 2;
            else if (fullName.Contains(lowerSearch))
                matchLevel = 1;
            else
                matchLevel = 0;

            return new
            {
                Profile = p,
                MatchLevel = matchLevel
            };
        })
        .OrderByDescending(x => x.MatchLevel)
        .ThenByDescending(x => x.Profile.CreateDate)
        .Select(x => x.Profile);
                if (itemCountToReturn > 0)
                {
                    ordered = ordered.Take(itemCountToReturn);
                }

                return ordered.Distinct().ToList();
            }

            if (itemCountToReturn > 0)
            {
                profiles = profiles.Take(itemCountToReturn);
            }
            // Bind data to DataList
            return profiles.Distinct().OrderByDescending(sort => sort.CreateDate).ToList();
        }

        public static List<FriendInfo> MyReceivedConnections(Guid userId, int itemCountToReturn)
        {
            //Guid UserId = new Guid(userId);
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var statusType = (from uus in dc.UserUserStatus
                              where uus.Status == "Pending"
                              select new { uus.UserUserStatusId }).SingleOrDefault();

            // Requests I received, i need to accept
            // Fetch data using LINQ to SQL
            var friendsThatRequestedMe = from friend in dc.UserUsers
                                         where friend.AcceptingUserId == userId
                                         //&& friend.IsActive == isActive
                                         && friend.UserUserStatusId == statusType.UserUserStatusId //CONNECTED STATUS TYPE
                                         && friend.UserUserRelationshipId == new Guid("aae059ad-b986-4675-a86c-039b28e6b296") //FRIEND TYPE
                                         orderby friend.RequestedOn descending
                                         select new FriendInfo
                                         {
                                             AcceptedOn = friend.RequestedOn,
                                             UserId = friend.RequestingUserId,
                                             ProfileImage = (from p in dc.Photos
                                                             join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                                             where ph.UserId == friend.RequestingUserId
                                                             orderby p.CreatedOn descending
                                                             select p.FilenameCropped).Take(1).SingleOrDefault(),
                                             FullName = (from profile in dc.Profiles
                                                         where profile.UserId == friend.RequestingUserId //Since I need to accept, show the name of the requestor
                                                         select profile.Firstname + " " + profile.Lastname).SingleOrDefault()
                                         };

            if (itemCountToReturn > 0)
            {
                friendsThatRequestedMe = friendsThatRequestedMe.Take(itemCountToReturn);
            }
            // Bind data to DataList
            return friendsThatRequestedMe.Distinct().ToList();
        }
        public static List<FriendInfo> MySentConnections(Guid userId, int itemCountToReturn)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var statusType = (from uus in dc.UserUserStatus
                              where uus.Status == "Pending"
                              select new { uus.UserUserStatusId }).SingleOrDefault();

            //Requests I sent, pending acceptance
            // Fetch data using LINQ to SQL
            var friendRequestsISent = from friend in dc.UserUsers
                                      where friend.RequestingUserId == userId
                                      //&& friend.IsActive == isActive
                                      && friend.UserUserStatusId == statusType.UserUserStatusId //CONNECTED STATUS TYPE
                                      && friend.UserUserRelationshipId == new Guid("aae059ad-b986-4675-a86c-039b28e6b296") //FRIEND TYPE
                                      orderby friend.RequestedOn descending
                                      select new FriendInfo
                                      {
                                          AcceptedOn = friend.RequestedOn,
                                          UserId = friend.AcceptingUserId,
                                          ProfileImage = (from p in dc.Photos
                                                          join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                                          where ph.UserId == friend.AcceptingUserId
                                                          orderby p.CreatedOn descending
                                                          select p.FilenameCropped).Take(1).SingleOrDefault(),
                                          FullName = (from profile in dc.Profiles
                                                      where profile.UserId == friend.AcceptingUserId //Since I'm the requested, show the name of the acceptor
                                                      select profile.Firstname + " " + profile.Lastname).SingleOrDefault()
                                      };

            if (itemCountToReturn > 0)
            {
                friendRequestsISent = friendRequestsISent.Take(itemCountToReturn);
            }
            // Bind data to DataList
            return friendRequestsISent.Distinct().ToList();
        }
        public static List<FriendInfo> MyConnections(Guid userId, int itemCountToReturn)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var statusType = (from uus in dc.UserUserStatus
                              where uus.Status == "Connected"
                              select new { uus.UserUserStatusId }).SingleOrDefault();

            var friendsThatRequestedMe = from friend in dc.UserUsers
                                         where friend.AcceptingUserId == userId
                                         //&& friend.IsActive == isActive
                                         && friend.UserUserStatusId == statusType.UserUserStatusId //CONNECTED STATUS TYPE
                                         && friend.UserUserRelationshipId == new Guid("aae059ad-b986-4675-a86c-039b28e6b296") //FRIEND TYPE
                                         orderby friend.AcceptedOn descending
                                         select new FriendInfo
                                         {
                                             AcceptedOn = friend.RequestedOn,
                                             UserId = friend.RequestingUserId,
                                             ProfileImage = (from p in dc.Photos
                                                             join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                                             where ph.UserId == friend.RequestingUserId
                                                             orderby p.CreatedOn descending
                                                             select p.FilenameCropped).Take(1).SingleOrDefault(),
                                             FullName = (from profile in dc.Profiles
                                                         where profile.UserId == friend.RequestingUserId //Since I need to accept, show the name of the requestor
                                                         select profile.Firstname + " " + profile.Lastname).SingleOrDefault()
                                         };

            //Requests I received
            // Fetch data using LINQ to SQL
            var friendsIRequested = from friend in dc.UserUsers
                                    where friend.RequestingUserId == userId
                                    //&& friend.IsActive == isActive
                                    && friend.UserUserStatusId == statusType.UserUserStatusId //  == new Guid("DBC81BFF-6772-48BE-9781-77825CEF590D") //statusType.UserUserStatusId  //CONNECTED STATUS TYPE
                                    && friend.UserUserRelationshipId == new Guid("aae059ad-b986-4675-a86c-039b28e6b296") //FRIEND TYPE
                                    orderby friend.RequestedOn descending
                                    select new FriendInfo
                                    {
                                        AcceptedOn = friend.RequestedOn,
                                        UserId = friend.AcceptingUserId,
                                        ProfileImage = (from p in dc.Photos
                                                        join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                                        where ph.UserId == friend.AcceptingUserId
                                                        orderby p.CreatedOn descending
                                                        select p.FilenameCropped).Take(1).SingleOrDefault(),
                                        FullName = (from profile in dc.Profiles
                                                    where profile.UserId == friend.AcceptingUserId //Since I'm the requested, show the name of the acceptor
                                                    select profile.Firstname + " " + profile.Lastname).SingleOrDefault()
                                    };

            var myConnections = friendsIRequested.Union(friendsThatRequestedMe).Distinct();

            if (itemCountToReturn > 0)
            {
                myConnections = myConnections.Take(itemCountToReturn);
            }
            return myConnections.ToList();
        }

        public static string GetColor(string color)
        {
            switch (color)
            {
                case "danger":
                    {
                        color = "red";
                    }
                    break;
                case "warning":
                    {
                        color = "yellow";
                    }
                    break;
                case "info":
                    {
                        color = "blue";
                    }
                    break;
            }
            return color;
        }

        public static void SendEmailTemplate(string firstName, string lastName, string recipientEmail, string message, string subject, string htmlFilePath, System.Web.UI.Control obj)
        {
            //htmlFilePath "~\\CreateAccount.html"
            string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
            string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();

            ListDictionary ldEmailBodyReplacements = new ListDictionary();
            if (!String.IsNullOrEmpty(firstName))
            {
                ldEmailBodyReplacements.Add("<%Firstname%>", firstName);
            }
            if (!String.IsNullOrEmpty(lastName))
            {
                ldEmailBodyReplacements.Add("<%Lastname%>", lastName);
            }
            if (!String.IsNullOrEmpty(message))
            {
                ldEmailBodyReplacements.Add("<%Message%>", message);
            }
            if (!String.IsNullOrEmpty(subject))
            {
                ldEmailBodyReplacements.Add("<%Subject%>", subject);
            }

            MailDefinition mailDefinition = new MailDefinition();

            mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath(htmlFilePath);
            mailDefinition.Subject = subject;
            mailDefinition.IsBodyHtml = true;
            //mailDefinition.From = emailFrom;
            //mailDefinition.CC = "support@Stability.org";

            MailMessage msg = mailDefinition.CreateMailMessage(recipientEmail, ldEmailBodyReplacements, obj);
            //msg.To.Add(new MailAddress(recipientEmail, fullname));

            msg.From = new MailAddress(emailFrom, emailFromDisplayName);
            msg.Bcc.Add(new MailAddress("support@Stability.org", "Stability Support"));
            //msg.Bcc.Add(new MailAddress("robgaudet@Stability.org", "Rob Gaudet"));
            //msg.Bcc.Add(new MailAddress("melissa.adair@gaudet.media", "Melissa Adair"));
            //msg.Bcc.Add(new MailAddress("rob.gaudet@gaudet.media", "Rob Gaudet"));

            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }

        public static string CalculateVolunteersNeeded(Guid userId, int points, bool returnAllNeededVolunteers, Guid eventId)
        {
            string volunteerCount = string.Empty;
            int totalVolunteersNeeded = 0;
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            if (returnAllNeededVolunteers)
            {
                var pointsVolunteerCounts = (from r in dc.Rebuilds
                                             where r.CreatedBy == userId
                                             group r by r.Difficulty into g
                                             select new { diffculty = g.Key, totalDifficulty = g.Sum(x => x.Difficulty), totalCount = g.Count() });

                if (pointsVolunteerCounts != null)
                {
                    int volunteerCountForMath = 0;
                    foreach (var item in pointsVolunteerCounts)
                    {
                        switch (item.diffculty)
                        {
                            case 0:
                                {
                                    volunteerCountForMath = 1;
                                    totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath; //Just count all times 0 was selected, since there is one volunteer per count.

                                    break;
                                }
                            case 1:
                                {
                                    volunteerCountForMath = 5;
                                    totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;

                                    break;
                                }
                            case 2:
                                {
                                    volunteerCountForMath = 10;
                                    totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;


                                    break;
                                }
                            case 3:
                                {
                                    volunteerCountForMath = 20;
                                    totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;


                                    break;
                                }
                        }
                    }
                }
                volunteerCount = totalVolunteersNeeded.ToString();
            }
            else if (userId == Guid.Empty && eventId != null)
            {
                var pointsVolunteerCounts = (from r in dc.Rebuilds
                                             where r.EventId == eventId
                                             group r by r.Difficulty into g
                                             select new { diffculty = g.Key, totalDifficulty = g.Sum(x => x.Difficulty), totalCount = g.Count() });

                if (pointsVolunteerCounts != null)
                {
                    int volunteerCountForMath = 0;
                    foreach (var item in pointsVolunteerCounts)
                    {
                        switch (item.diffculty)
                        {
                            case 0:
                                {
                                    volunteerCountForMath = 1;
                                    totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath; //Just count all times 0 was selected, since there is one volunteer per count.

                                    break;
                                }
                            case 1:
                                {
                                    volunteerCountForMath = 5;
                                    totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;

                                    break;
                                }
                            case 2:
                                {
                                    volunteerCountForMath = 10;
                                    totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;


                                    break;
                                }
                            case 3:
                                {
                                    volunteerCountForMath = 20;
                                    totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;


                                    break;
                                }
                        }
                    }
                }
                volunteerCount = totalVolunteersNeeded.ToString();
            }
            else
            {
                // 0 points		- 1 person, Quick jobs can be completed in less than 1 day with 1 person.
                // 1 point		- 5 persons, Easy jobs can be completed in a couple of days with minimal volunteers.
                // 2 points		- 10 persons, Moderately difficult jobs that take many weeks and require a team with few professional skillsets.
                // 3 points		- 20 persons, Extremely difficult jobs that require months of work, planning and require multiple professional skillsets.

                switch (points)
                {
                    case 0:
                        {
                            volunteerCount = "1";
                            break;
                        }
                    case 1:
                        {
                            volunteerCount = "5";
                            break;
                        }
                    case 2:
                        {
                            volunteerCount = "10";
                            break;
                        }
                    case 3:
                        {
                            volunteerCount = "20";
                            break;
                        }
                }
            }

            return volunteerCount;
        }

        public static string GetImpactedStateCountyString(Guid eventId)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            IEnumerable<USState> states = from s in dc.USStates
                                          join es in dc.EventStates on s.StatesId equals es.StatesId
                                          where es.EventId == eventId
                                          orderby s.Name
                                          select s;

            string stateResults = string.Empty;
            if (states.Count() > 0)
            {
                foreach (var state in states.OrderBy(o => o.Name))
                {

                    string stateName = string.Empty;
                    stateName = "<b>" + state.Name + "</b>";

                    //Get the counties impacted as well.
                    var counties = from co in dc.Counties
                                   join ec in dc.EventCounties on co.CountyId equals ec.CountyId
                                   where co.Code == state.Code && ec.EventId == eventId
                                   orderby co.Name
                                   select new { ec.CountyId, co.Name, co.Code };

                    string comma = ", ";
                    int countyCount = 0;
                    string countyName = string.Empty;
                    if (counties.Count() > 0)
                    {
                        foreach (var county in counties.OrderBy(o => o.Name))
                        {
                            countyCount += 1;
                            if (counties.Count() == countyCount)
                            {
                                comma = "";
                            }
                            countyName += county.Name + comma;
                        }
                    }

                    string countyTerm = countyCount == 1 ? "County" : "Counties";
                    if (state.Name == "Louisiana")
                    {
                        countyTerm = countyCount == 1 ? "Parish" : "Parishes";
                    }
                    stateResults += stateName + " - " + countyCount + " " + countyTerm + " Have Team Deployments<br>" + countyName + " <br>";
                }
            }
            return stateResults;
        }


        public static string GetImpactedStateCountyString()
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var states = (from s in dc.USStates
                          join es in dc.EventStates on s.StatesId equals es.StatesId
                          orderby s.Name descending
                          select s).Distinct();

            string stateResults = string.Empty;
            if (states.Count() > 0)
            {
                foreach (var state in states.OrderBy(o => o.Name))
                {
                    string stateName = string.Empty;

                    //Get the counties impacted as well.
                    var counties = from co in dc.Counties
                                   join ec in dc.EventCounties on co.CountyId equals ec.CountyId
                                   where co.Code == state.Code
                                   orderby co.Name
                                   select new { ec.CountyId, co.Name, co.Code };

                    string comma = ", ";
                    int countyCount = 0;
                    string countyName = string.Empty;
                    if (counties.Count() > 0)
                    {
                        stateName = "<b>" + state.Name + "</b>";
                        foreach (var county in counties.OrderBy(o => o.Name))
                        {
                            countyCount += 1;
                            if (counties.Count() == countyCount)
                            {
                                comma = "";
                            }
                            countyName += county.Name + comma;
                        }

                        string countyTerm = countyCount == 1 ? "County" : "Counties";
                        if (state.Name == "Louisiana")
                        {
                            countyTerm = countyCount == 1 ? "Parish" : "Parishes";
                        }
                        stateResults += stateName + " - " + countyCount + " " + countyTerm + " " + countyName + " <br>";
                    }
                }
            }
            return stateResults;
        }

        public static string GetImpactedStateCountyStringByTeam(Guid organizationId)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            IEnumerable<USState> states = (from s in dc.USStates
                                           join oe in dc.OrganizationEvents on s.StatesId equals oe.StagingStateId
                                           where oe.OrganizationId == organizationId
                                           orderby s.Name
                                           select s).Distinct().ToArray();

            string stateResults = string.Empty;
            if (states.Count() > 0)
            {
                foreach (var state in states.OrderBy(o => o.Name))
                {

                    string stateName = string.Empty;
                    stateName = "<b>" + state.Name + "</b>";

                    //Get the counties impacted as well.
                    var counties = (from co in dc.Counties
                                    join oe in dc.OrganizationEvents on co.CountyId equals oe.StagingCountyId
                                    where co.Code == state.Code && oe.OrganizationId == organizationId
                                    orderby co.Name
                                    select new { co.CountyId, co.Name, co.Code }).Distinct().ToArray();

                    string comma = ", ";
                    int countyCount = 0;
                    string countyName = string.Empty;
                    if (counties.Count() > 0)
                    {
                        foreach (var county in counties.OrderBy(o => o.Name))
                        {
                            countyCount += 1;
                            if (counties.Count() == countyCount)
                            {
                                comma = "";
                            }
                            countyName += county.Name + comma;
                        }
                    }

                    string countyTerm = countyCount == 1 ? "County" : "Counties";
                    if (state.Name == "Louisiana")
                    {
                        countyTerm = countyCount == 1 ? "Parish" : "Parishes";
                    }
                    stateResults += stateName + " - " + countyCount + " " + countyTerm + " " + countyName + " <br>";
                }
            }
            return stateResults;
        }

        public static void SendEmail(
            string message,
            string subject,
            ListDictionary ldEmailBodyReplacements,
            string recipientsEmail,
            string recipientsName,
            string senderName,
            string senderEmail,
            string bodyFileNamePath,
            out string error,
            string bccAdmin = "robgaudet@gocajunnavy.org")
        {
            error = string.Empty;
            //Make sure and update the email sent information.
            try
            {
                string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
                string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();
                string sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"].ToString();

                if (!String.IsNullOrEmpty(senderEmail))
                {
                    emailFrom = senderEmail;
                }

                if (!String.IsNullOrEmpty(senderName))
                {
                    emailFromDisplayName = senderName;
                }

                MailDefinition mailDefinition = new MailDefinition();
                mailDefinition.BodyFileName = string.IsNullOrEmpty(bodyFileNamePath) ? bodyFileNamePath : HttpContext.Current.Server.MapPath(bodyFileNamePath);
                mailDefinition.Subject = subject;
                mailDefinition.IsBodyHtml = true;
                mailDefinition.From = emailFrom;

                MailMessage mail = string.IsNullOrEmpty(bodyFileNamePath) ? mailDefinition.CreateMailMessage(recipientsEmail, ldEmailBodyReplacements, message, new System.Web.UI.Control()) : mailDefinition.CreateMailMessage(recipientsEmail, ldEmailBodyReplacements, new System.Web.UI.Control());
                mail.From = new MailAddress(emailFrom, emailFromDisplayName);
                MailAddress bcc = new MailAddress(bccAdmin);
                MailAddress to = new MailAddress(recipientsEmail, recipientsName);
                mail.Bcc.Add(bcc);
                mail.To.Add(to);

                SmtpClient client = new SmtpClient("smtp.sendgrid.net");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                NetworkCredential credentials = new NetworkCredential("apikey", sendGridApiKey);
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                client.EnableSsl = true;
                client.Port = 587;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }

        //public static void SendEmail(string firstName, string lastName, string htmlFilePath, string subject, string recipientEmail, System.Web.UI.Control obj)
        //{
        //	try
        //	{
        //		//htmlFilePath "~\\CreateAccount.html"
        //		string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
        //		string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();

        //		ListDictionary ldEmailBodyReplacements = new ListDictionary();
        //		if (!String.IsNullOrEmpty(firstName))
        //		{
        //			ldEmailBodyReplacements.Add("<%Firstname%>", firstName);
        //		}
        //		if (!String.IsNullOrEmpty(lastName))
        //		{
        //			ldEmailBodyReplacements.Add("<%Lastname%>", lastName);
        //		}

        //		MailAddress fromAddress = new MailAddress(emailFrom, emailFromDisplayName);

        //		MailDefinition mailDefinition = new MailDefinition();

        //		mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath(htmlFilePath);
        //		mailDefinition.Subject = subject;
        //		mailDefinition.IsBodyHtml = true;

        //		MailMessage newUserMailMessage = mailDefinition.CreateMailMessage(recipientEmail, ldEmailBodyReplacements, obj);

        //		SmtpClient smtp = new SmtpClient();
        //		//smtp.EnableSsl = true;
        //		smtp.Send(newUserMailMessage);
        //	}
        //	catch (Exception ex)
        //	{ }
        //}

        public static string GetElapsedTime(DateTime time)
        {
            string elapsedTime = "0";
            DateTime rightNow = DateTime.Now;

            TimeSpan interval = rightNow - time;

            if (interval.TotalSeconds < 60)
            {
                //Less than 60 minutes then show the interval in minutes.
                elapsedTime = interval.Seconds.ToString() + " " + (interval.Seconds == 1 || interval.Seconds == 0 ? "second ago" : "seconds ago");
            }
            else if (interval.TotalMinutes < 60)
            {
                //Less than 60 minutes then show the interval in minutes.
                elapsedTime = interval.Minutes.ToString() + " " + (interval.Minutes > 1 ? "minutes ago" : "minute ago");
            }
            else if (interval.TotalHours < 24)
            {
                //Less than 24 hours show hours passed.
                elapsedTime = interval.Hours.ToString() + " " + (interval.Hours > 1 ? "hours ago" : "hour ago");
            }
            else if (interval.TotalDays < 7)
            {
                //More than 24 hours show hours passed.
                elapsedTime = time.Date.DayOfWeek + " at " + time.ToShortTimeString();
            }
            else
            {
                elapsedTime = time.ToLongDateString();
            }

            return elapsedTime;
        }
    }
}