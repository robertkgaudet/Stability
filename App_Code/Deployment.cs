using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
namespace Stability
{
	public class Deployment
	{
		public Deployment(
			//string county, 
			//string urlFriendlyDeploymentName, 
			//string deploymentName, 
			//string urlFriendlyName, 
			//bool isVoadMember, 
			//string logo, 
			//Guid eventId, 
			//Guid organizationId,
			//string organizationName
			)
		{
			//OrganizationName = organizationName;
			//OrganizationId = organizationId;
			//EventId = eventId;



		}

		public string County { get; set; }
		public string URLFriendlyDeploymentName { get; set; }
		public string DeploymentName { get; set; }
		public string URLFriendlyName { get; set; }
		public bool ISVoadMember { get; set; }
		public decimal VolunteerHourlyRate { get; set; }
		public string Logo { get; set; }
		public Guid EventId { get; set; }
		public Guid OrganizationId { get; set; }
		public Guid OrganizationEventId { get; set; }
		public string OrganizationName { get; set; }
	}
}