using Stability.Data.Entities;
namespace Stability.Data.Repositories.Deployments;
public interface IDeploymentsRepository
{
    OrganizationEvent GetDeployment(Guid organizationEventId);
}