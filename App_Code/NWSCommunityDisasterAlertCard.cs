using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NWSCommunityDisasterAlertCard
/// </summary>
public class NWSCommunityDisasterAlertCard
{
	public string GptMessage { get; set; }          // GPT-generated guidance
	public string Event { get; set; }
	public string Severity { get; set; }
	public string Certainty { get; set; }
	public string Urgency { get; set; }
	public string Description { get; set; }
	public string Headline { get; set; }
	public string SenderName { get; set; }
	public string AreaDesc { get; set; }
	public string Url { get; set; }
	public DateTime Sent { get; set; }
	public DateTime Expires { get; set; }
	public DateTime Ends { get; set; }
	public string Status { get; set; }
	public string MessageType { get; set; }
	public string Category { get; set; }
}
