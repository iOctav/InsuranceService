using Core.Models;

namespace Core.Services;

public interface IApplicationService
{
    public Task<Guid> CreateApplicationAsync(NewApplicationModel model, CancellationToken cancellationToken);
    public Task<bool> ApproveApplicationAsync(Guid applicationId, Guid operatorId, CancellationToken cancellationToken);
    public Task<IEnumerable<ConsumerApplication>> GetApplicationsByOperatorId(Guid operatorId, CancellationToken cancellationToken);
    public Task<IEnumerable<OperatorApplicationModel>> GetApplicationsByAgentId(Guid operatorId, CancellationToken cancellationToken);
    public Task<Guid?> RejectApplicationAsync(Guid applicationId, CancellationToken cancellationToken);
}