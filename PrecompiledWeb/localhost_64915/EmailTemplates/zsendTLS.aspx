<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Net.Mail" %>

<script language="C#" runat="server">
  protected void Page_Load(object sender, EventArgs e)
  {
    MailMessage mail = new MailMessage();

    mail.From = new MailAddress("robgaudet@stability.org");
    mail.To.Add("robgaudet@stability.org");

    mail.Subject = "Test email sent from System.Net.Mail";
    mail.Body = "Mail test";
    mail.Headers.Add("Message-Id",
                      String.Format("<{0}@{1}>",
                      Guid.NewGuid().ToString(),
                      "stability.org"));

    SmtpClient smtp = new SmtpClient("smtp.sendgrid.net");

	System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    NetworkCredential Credentials = new NetworkCredential("apikey", "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");
    smtp.Credentials = Credentials;
    smtp.EnableSsl = true;
    smtp.Port = 587;
    smtp.Send(mail);
    lblMessage.Text = "Mail Sent";
  }
</script>
<html>
<body>
  <form runat="server">
    <asp:Label id="lblMessage" runat="server"></asp:Label>
  </form>
</body>
</html>