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

    }

    protected void btnRecover_Click(object sender,EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string userName = null;

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var userId = (from m in dc.aspnet_Memberships
                          where m.Email == email
                          select m.UserId).FirstOrDefault();

            if (userId !=null)
            {
                userName = (from u in dc.aspnet_Users
                            where u.UserId == userId
                            select u.UserName).FirstOrDefault();
            }
        }

        if (!string.IsNullOrEmpty(userName))
        {
            try
            {
                string emailFrom = System.Configuration.ConfigurationManager.AppSettings["emailFrom"].ToString().Trim();
                string emailFromDisplayName = System.Configuration.ConfigurationManager.AppSettings["emailFromDisplayName"].ToString().Trim();
                string bodyTemplate = System.IO.File.ReadAllText(Server.MapPath("~/V1/UserNameRecovery.html"));
                string emailBody = bodyTemplate.Replace("<%UserName%>", userName);

                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.Subject = "Your Crowd Relief Username";
                mail.Body = emailBody;
                mail.From = new MailAddress(emailFrom, emailFromDisplayName);
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
            

                smtp.Send(mail);

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "A UserName has been emailed to you.";
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error sending email: " + ex.Message;
            }
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "Email not found. Please try again.";
        }
    }
}
