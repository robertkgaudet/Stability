using System;
using System.Collections.Generic;
using System.Linq;
public class DeploymentsRepository
{
    private readonly CrowdReliefDBDataContext dbContext;
    public DeploymentsRepository(CrowdReliefDBDataContext dc)
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