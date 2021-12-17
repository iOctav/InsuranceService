using Core.Models;

namespace Core.Services;

public interface IAccountService
{
    public Task<string?> LoginAsync(string username, string password, CancellationToken cancellationToken);
    public Task<string> GetRoleAsync(string username, CancellationToken cancellationToken);
    public Task<Consumer?> GetConsumerAsync(string username, CancellationToken cancellationToken);
    public Task<Operator?> GetOperatorAsync(string username, CancellationToken cancellationToken);
    public Task<InsuranceAgent?> GetAgentAsync(string username, CancellationToken cancellationToken);
}