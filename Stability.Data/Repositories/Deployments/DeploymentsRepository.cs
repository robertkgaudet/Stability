using Stability.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Stability.Data.Repositories.Deployments;
public class DeploymentsRepository : IDeploymentsRepository
{
    private readonly DbContext dbContext;
    public DeploymentsRepository(DbContext dc)
    {
        dbContext = dc;
    }

    public OrganizationEvent GetDeployment(Guid organizationEventId)
    {
        var deployment = (from orgEvents in dbContext.OrganizationEvents
						  where orgEvents.OrganizationEventId == organizationEventId
						  select orgEvents).SingleOrDefault();
        return deployment;
    }
}