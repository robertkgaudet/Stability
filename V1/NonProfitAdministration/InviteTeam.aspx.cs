using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_InviteTeam : BaseWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		ddlEmailAddresses.Attributes.Add("multiple", "multiple");
		ddlEmailAddresses.Attributes.Add("style", "width:100%;");
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string emails = hidEmailAddresses.Value;
		if (!String.IsNullOrEmpty(emails))
		{
			string[] emailsArray = emails.Split(',');
			string organizationId = Request.QueryString["OrganizationId"];
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			foreach (string email in emailsArray)
			{
				Guid UserOrganizationInviteId = Guid.NewGuid();
				//Check to see if they have already been invited.
				var emailAddress = from uoi in dc.UserOrganizationInvites
								   where uoi.EmailAddress == email
								   select uoi;

				if (emailAddress != null)
				{
					var emailAddressOrganization = (from uoi in dc.UserOrganizationInvites
													where uoi.EmailAddress == email && uoi.OrganizationId == new Guid(organizationId)
													select uoi).SingleOrDefault();

					if (emailAddressOrganization == null)
					{
						//User exists but was invited by another organization to join.
						//Make a new entry with this organization.
						UserOrganizationInvite userOrganizationInvite = new UserOrganizationInvite();
						userOrganizationInvite.OrganizationId = new Guid(organizationId);
						userOrganizationInvite.UserOrganizationInviteId = UserOrganizationInviteId;
						userOrganizationInvite.CreatedBy = userId;
						userOrganizationInvite.CreatedOn = DateTime.Now;
						userOrganizationInvite.EmailAddress = email;
						dc.UserOrganizationInvites.InsertOnSubmit(userOrganizationInvite);
						dc.SubmitChanges();
					}
					else
					{
						//User exists and was invited by the same previous organization again.
						//If so, just send a new email.
						emailAddressOrganization.DateInvitationResentOn = DateTime.Now;
						emailAddressOrganization.CreatedBy = userId;
						emailAddressOrganization.OrganizationId = new Guid(organizationId);
						dc.SubmitChanges();
					}
				}
				else
				{
					//If not, then add them and send an email.
					//Insert the email invites
					UserOrganizationInvite userOrganizationInvite = new UserOrganizationInvite();
					userOrganizationInvite.OrganizationId = new Guid(organizationId);
					userOrganizationInvite.UserOrganizationInviteId = UserOrganizationInviteId;
					userOrganizationInvite.CreatedBy = userId;
					userOrganizationInvite.CreatedOn = DateTime.Now;
					userOrganizationInvite.EmailAddress = email;
					dc.UserOrganizationInvites.InsertOnSubmit(userOrganizationInvite);
					dc.SubmitChanges();

					//Email the user.
				}

                //Get the team name and creator name to send in the email.
                var organizationInfo = (from p in dc.Profiles
                                        join o in dc.UserOrganizations on p.UserId equals o.UserId
                                        where p.UserId == userId && o.Status== (int)RequestStatus.Approved
                                        select new { organizationName = o.Organization.Name, p.Firstname, senderName = p.Firstname + " " + p.Lastname }).FirstOrDefault();

                string senderName				= organizationInfo.senderName;
				string organizationName			= organizationInfo.organizationName;
				string firstName				= organizationInfo.Firstname;

				ListDictionary ldEmailBodyReplacements = new ListDictionary();
				//ldEmailBodyReplacements.Add("<% SenderName %>", senderName);
				//ldEmailBodyReplacements.Add("<% TeamName %>", organizationName);
				ldEmailBodyReplacements.Add("<% FirstName %>", firstName);
				ldEmailBodyReplacements.Add("<% UserOrganizationInviteId %>", UserOrganizationInviteId.ToString());
				string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
				string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();
				string emailError = string.Empty;
				Tools.SendEmail(
					string.Empty,
					senderName + " Has Invited You To Join His Impactoid Disaster Relief Team",
					ldEmailBodyReplacements,
					email,
					firstName,
					senderName,
					emailFrom,
					"~\\EmailTemplates\\MemberInvitation.html",
					out emailError
					);

				//SendEmailInvitations(email, new Guid(organizationId), UserOrganizationInviteId, senderName, organizationName, firstName);

			}
			//Redirect to setup their website.
			dc.SubmitChanges();
			Response.Redirect("/V1/NonProfit/Default.aspx?OrganizationId=" + organizationId);
		}
	}
	protected void btnSkip_Click(object sender, EventArgs e)
	{
        string organizationId = Request.QueryString["OrganizationId"];
        Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=" + organizationId);
    }
    //protected void SendEmailInvitations(string emailAddress, Guid OrganizationId, Guid UserOrganizationInviteId, string SenderName, string TeamName, string FirstName)
    //{
    //	//Make sure and update the email sent information.
    //	try
    //	{
    //		string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
    //		string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();

    //		ListDictionary ldEmailBodyReplacements = new ListDictionary();
    //		ldEmailBodyReplacements.Add("<% SenderName %>", SenderName);
    //		ldEmailBodyReplacements.Add("<% TeamName %>", TeamName);
    //		ldEmailBodyReplacements.Add("<% FirstName %>", FirstName); 
    //		ldEmailBodyReplacements.Add("<% UserOrganizationInviteId %>", UserOrganizationInviteId.ToString());

    //		MailAddress fromAddress = new MailAddress(emailFrom, emailFromDisplayName);

    //		MailDefinition mailDefinition = new MailDefinition();

    //		mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath("~\\EmailTemplates\\MemberInvitation.html");
    //		mailDefinition.Subject = SenderName + " Has Invited You To Join His Impactoid Disaster Relief Team";
    //		mailDefinition.IsBodyHtml = true;

    //		MailMessage newUserMailMessage = mailDefinition.CreateMailMessage(emailAddress, ldEmailBodyReplacements, this);

    //		MailAddress bcc = new MailAddress("robgaudet@gocajunnavy.org");
    //		newUserMailMessage.Bcc.Add(bcc);

    //		SmtpClient smtp = new SmtpClient();
    //		//smtp.EnableSsl = true;
    //		smtp.Send(newUserMailMessage);
    //	}
    //	catch (Exception ex)
    //	{ }

    //}
}