using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_EventMessages : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SendSMSMessages();
	}

	protected void SendSMSMessages()
	{
		string accountSid = System.Configuration.ConfigurationManager.AppSettings["twilioAccountSID"].ToString();  // Replace with your account SID
		string authToken = System.Configuration.ConfigurationManager.AppSettings["twilioAuthToken"].ToString(); ;    // Replace with your auth token
		string fromNumber = System.Configuration.ConfigurationManager.AppSettings["twilioPhoneNumber"].ToString(); ; // Replace with your Twilio number

		// Array of phone numbers to send the SMS to
		string[] phoneNumbers = { "+13185723161", "+12254273970", "+13372580572" };

		// The message you want to send
		string messageBody = "Ground Force Humanitarian Aid is requesting deployment availability for a potential strike by Francine." + 
			"\r\n\r\n38 volunteer positions are now open on our team." + 
			"\r\n\r\nPlease find an available position to help here. https://www.stability.org/SignUp/SAFECampFrancine2024";

		// Create an instance of Tools and send the SMS
		var tools = new Tools(accountSid, authToken, fromNumber);
		tools.SendSms(messageBody, phoneNumbers);
	}
}