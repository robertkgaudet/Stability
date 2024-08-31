using Stability.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Stability.Data.Repositories.Positions;
public class PositionsRepository : IPositionsRepository
{
    private readonly DbContext dbContext;
    public PositionsRepository(DbContext dc)
    {
        dbContext = dc;
    }

    public List<OrganizationEventPosition> GetAvailableShifts(Guid organizationEventId)
    {
        var availableShifts = (from shifts in dbContext.OrganizationEventPositions
						  where shifts.OrganizationEventId == organizationEventId
						  select shifts).ToList();
        //TODO filter down based on required capacity per day

        return availableShifts
    }

    public List<UserOrganizationEventPosition> GetUpcomingShifts(Guid organizationEventId, Guid userId)
    {   
        //TODO: join on organization event positions and only show this deployments upcoming events i've signed up for

        var upcomingShifts = (from userShifts in dbContext.UserOrganizationEventPositions
						  where userShifts.UserId == userId
						  select userShifts).ToList();

        //TODO: the ideal contract would have the days they are working and a list of shifts for each day
        return upcomingShifts;
    }
}