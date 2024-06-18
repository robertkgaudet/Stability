using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmailTemplates_MailTest : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		MailMessage message = new MailMessage();
		message.From = new MailAddress(
			"robgaudet@stability.org",
			"Rob - TEST"
		);
		message.To.Add(new MailAddress(
			"robgaudet@gocajunnavy.org",
			"Rob - TEST"
		));
		message.Subject = "Sending with Twilio SendGrid is Fun";
		var textBody = "and easy to do anywhere, especially with C#";
		var htmlBody = "and easy to do anywhere, <b>especially with C#</b>";

		message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
			textBody, null, MediaTypeNames.Text.Plain)
		);
		message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
			htmlBody, null, MediaTypeNames.Text.Html)
		);

		// if you only want to send HTML email, simply use this
		// message.Body = htmlBody;
		// message.IsBodyHtml = true;

		SmtpClient client = new SmtpClient(host: "smtp.sendgrid.net", port: 587);
		client.Credentials = new NetworkCredential(
			userName: "apikey",
			password: Environment.GetEnvironmentVariable("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") // password is the API key
		);

		client.Send(message);
		Response.Write("Sending email");
		Response.Write("Email sent");
	}
}