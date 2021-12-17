using Core.Models;

namespace Core.Services;

public interface ICompensationService
{
    public Task<Guid?> CreateCompensationAsync(NewCompensationModel model, CancellationToken cancellationToken);
    public Task<Guid?> ApproveCompensationAsync(CompensationVerdictModel model, CancellationToken cancellationToken);
    public Task<IEnumerable<Compensation>> GetCompensationsAsync(Guid consumerId, CancellationToken cancellationToken);
    public Task<IEnumerable<CompensationModel>> GetCompensationsAsync(CancellationToken cancellationToken);
    public Task<CompensationModel?> GetCompensationAsync(Guid compensationId, CancellationToken cancellationToken);
    public Task<Guid?> RejectCompensationAsync(Guid compensationId, CancellationToken cancellationToken);
    public Task<Guid?> SignCompensationAsync(Guid compensationId, CancellationToken cancellationToken);
}