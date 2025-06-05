using System;
using System.Collections.Generic;
using System.Linq;
public static class SearchResponse
{
    public class PostInfo
    {
        public Guid CreatedBy { get; set; }
        public Guid PostId { get; set; }
        public Guid PostTypeId { get; set; }
        public string URLImage { get; set; }
        public string URLTitle { get; set; }
        public string URLDescription { get; set; }
        public string SharedURL { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public string FullName { get; set; }
    }
    public class OrganizationEventResult
    {
        public string CampaignName { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Name { get; set; }
        public string MissionPurpose { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid OrganizationEventId { get; set; }
        public string Logo { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class PortalResult
    {
        public string Name { get; set; }
        public Guid EventId { get; set; }
    }
    public class TeamResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoSquare { get; set; }
        public Guid OrganizationId { get; set; }
        public string URLFriendlyName { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class VolunteerOpportunityInfo
    {
        public Guid OrganizationEventId { get; set; }
        public string CampaignName { get; set; }
        public DateTime DeploymentDate { get; set; }
        public string URLFriendlyCampaignName { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public DateTime EarliestDeploymentDate { get; set; }
        public DateTime LatestDeploymentDate { get; set; }
        public int PositionCount { get; set; }
        public DateTime? RowNum { get; set; } // Nullable in case no deployment date exists
    }

}