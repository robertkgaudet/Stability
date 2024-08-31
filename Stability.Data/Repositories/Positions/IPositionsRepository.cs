using System.Collections.Generic;
namespace Stability.Data.Repositories.Positions;
public interface IPositionsRepository
{
    List<OrganizationEventPosition> GetAvailableShifts(Guid organizationEventId);
    List<UserOrganizationEventPosition> GetUpcomingShifts(Guid organizationEventId, Guid userId)
}