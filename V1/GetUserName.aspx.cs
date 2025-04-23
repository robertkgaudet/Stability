using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class V1_PasswordReset : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        UserNameRecovery.MailDefinition.Subject = "Stability Get UserName";
        UserNameRecovery.MailDefinition.BodyFileName = "~/V1/PasswordRecovery.html";
        UserNameRecovery.MailDefinition.IsBodyHtml = true;
	}

	protected void CancelEmail(Object sender, MailMessageEventArgs e)
	{
		//e.Cancel = true;
	}

	protected void UserNameRecovery2_SendingMail(object sender, MailMessageEventArgs e)
	{
		string emailFrom			= System.Configuration.ConfigurationManager.AppSettings["emailFrom"].ToString();
		string emailFromDisplayName = System.Configuration.ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();
		MailAddress fromAddress = new MailAddress(emailFrom, emailFromDisplayName);

		e.Message.From = fromAddress;
		SmtpClient smtp = new SmtpClient();
		//smtp.EnableSsl = true;
		smtp.Send(e.Message);

		//Stop the system from sending the original email.
		e.Cancel = true;
	}
}