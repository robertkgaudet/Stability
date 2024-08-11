<%@ WebHandler Language="C#" Class="MessageMember" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IdentityModel.Metadata;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class MessageMember : IHttpHandler, IReadOnlySessionState
{
	public string errorMessage      = string.Empty;
	public void ProcessRequest (HttpContext context) {

		string results = string.Empty;
		context.Response.ContentType = "application/json";
		try
		{
			string requestBody;
			using (var reader = new StreamReader(context.Request.InputStream))
			{
				requestBody = reader.ReadToEnd();
			}

			// Deserialize the JSON data
			var serializer = new JavaScriptSerializer();
			var data = serializer.Deserialize<RequestData>(requestBody);

			string message          = data.message;
			string recipientsName   = data.recipientsName;
			string recipientsEmail  = data.recipientsEmail;
			string teamName         = data.teamName;
			string senderName       = data.senderName;
			string organizationId   = data.organizationId;
			SendMessage(recipientsEmail, recipientsName, teamName, message, organizationId, senderName, out errorMessage);
			if(!string.IsNullOrEmpty(errorMessage))
			{
				throw new Exception();
			}
			else
			{
				var responseMessage = new { message = "Success" };
				// Serialize the response
				var jsonResponse = serializer.Serialize(responseMessage);
				context.Response.Write(jsonResponse);
			}
		}
		catch (Exception ex)
		{
			results += errorMessage + "<Br>" + results + ex.Data + "<br> " + ex.HelpLink + "<br> " + ex.HResult + "<br> " + ex.InnerException + "<br> " + ex.Message + "<br> " + ex.Source + "<br>" + ex.StackTrace + "<br>" + ex.TargetSite;
			var errorResponse = new { error = results };
			var jsonResponse = new JavaScriptSerializer().Serialize(errorResponse);
			//Console.WriteLine("Error sending email: " + results);
			context.Response.StatusCode = 500;
			context.Response.Write(jsonResponse);
		}
	}

	public bool IsReusable
	{
		get { return false; }
	}

	private class RequestData
	{
		public string message { get; set; }
		public string recipientsName { get; set; }
		public string recipientsEmail { get; set; }
		public string teamName { get; set; }
		public string senderName { get; set; }
		public string organizationId { get; set; }
	}

	protected void SendMessage(string recipientsEmail, string recipientsName, string teamName, string message, string organizationId, string senderName, out string error)
	{
		error = string.Empty;
		//Make sure and update the email sent information.
		try
		{
			string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
			string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();
			string sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"].ToString();

			ListDictionary ldEmailBodyReplacements = new ListDictionary();
			ldEmailBodyReplacements.Add("<% TeamName %>", teamName);
			ldEmailBodyReplacements.Add("<% RecipientsName %>", recipientsName);
			ldEmailBodyReplacements.Add("<% OrganizationId %>", organizationId);
			ldEmailBodyReplacements.Add("<% Message %>", message);
			ldEmailBodyReplacements.Add("<% SenderName %>", senderName);


			MailDefinition mailDefinition = new MailDefinition();
			mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath("~\\EmailTemplates\\TeamMemberMessage.html");
			mailDefinition.Subject = senderName + " sent you a message on Stability.";
			mailDefinition.IsBodyHtml = true;
			mailDefinition.From = emailFrom;

			MailMessage mail = mailDefinition.CreateMailMessage(recipientsEmail, ldEmailBodyReplacements, new System.Web.UI.Control());
			mail.From = new MailAddress(emailFrom, emailFromDisplayName);
			MailAddress bcc = new MailAddress("robgaudet@gocajunnavy.org");
			mail.Bcc.Add(bcc);

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
}