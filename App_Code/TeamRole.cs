using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TeamRoles
/// </summary>
public static class TeamRoleService
{
	public static TeamRoles GetTeamRoles(Guid userId, Guid organizationId, bool isSiteAdmin)
	{
		using (var dc = new CrowdReliefDBDataContext())
		{
			var teamRoles = new TeamRoles
			{
				IsSiteAdministrator = isSiteAdmin
			};

			// IsTeamMember
			teamRoles.IsTeamMember = dc.UserOrganizations.Any(uo =>
				uo.OrganizationId == organizationId &&
				uo.UserId == userId &&
				(uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending));

			// IsTeamOwner
			teamRoles.IsTeamOwner = dc.Organizations.Any(o =>
				o.OrganizationId == organizationId && o.OwnerId == userId);

			if (teamRoles.IsTeamOwner)
			{
				teamRoles.OrganizationOwnerUserId = userId;
				teamRoles.IsTeamMember = true;
			}
			else
			{
				// fallback to actual owner
				var org = dc.Organizations.FirstOrDefault(o => o.OrganizationId == organizationId);
				if (org != null)
				{
					teamRoles.OrganizationOwnerUserId = (Guid)org.OwnerId;
				}
			}

			// IsTeamAdministrator
			teamRoles.IsTeamAdministrator = dc.UserOrganizations.Any(uo =>
				uo.OrganizationId == organizationId &&
				uo.UserId == userId &&
				uo.IsTeamAdministrator == true &&
				(uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending));

			return teamRoles;
		}
	}
}



public class TeamRoles
{
	public Guid OrganizationOwnerUserId { get; set; }
	public bool IsSiteAdministrator { get; set; }
	public bool IsTeamOwner { get; set; }
	public bool IsTeamMember { get; set; }
	public bool IsTeamAdministrator { get; set; }
}
