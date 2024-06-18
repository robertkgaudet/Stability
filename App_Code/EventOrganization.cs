using System;
using System.Collections.Generic;
using System.Linq;
namespace Stability
{
	public class EventOrganization
	{
		public EventOrganization(Guid eventId, Guid organizationId, string eventName, string organizationName)
		{
			EventName = eventName;
			OrganizationName = organizationName;
			OrganizationId = organizationId;
			EventId = eventId;
		}

		public string EventName { get; set; }
		public string OrganizationName { get; set; }

		public Guid OrganizationId { get; set; }
		public Guid EventId { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1} - {2} - {3}", EventName, OrganizationName, OrganizationId, EventId);
		}
	}
}