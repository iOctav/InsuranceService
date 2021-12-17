using Core.Models;

namespace Core.Services;

public interface IInsuranceService
{
    public Task<bool> SignNewInsuranceContractAsync(Guid applicationId, Guid insuranceAgentId,
        CancellationToken cancellationToken);
    public Task<IEnumerable<InsuranceAgentModel>> GetInsuranceAgentsAsync(CancellationToken cancellationToken);
    public Task<InsuranceContractModel> GetInsuranceContract(Guid insuranceId, CancellationToken cancellationToken);
    public Task<InsuranceContractModel?> GetConsumerInsuranceContract(Guid consumerId, CancellationToken cancellationToken);
    public Task<Guid?> StopInsuranceAsync(Guid insuranceId, CancellationToken cancellationToken);
}