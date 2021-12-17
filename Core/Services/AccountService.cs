using Core.DbContexts;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class AccountService : IAccountService
{
    public async Task<string?> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var consumer = await context.Set<Consumer>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        if (consumer != default)
        {
            return consumer.Credential.Password.Equals(password) ? consumer.Credential.Username : null;
        }
        var controller = await context.Set<Operator>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        if (controller != default)
        {
            return controller.Credential.Password.Equals(password) ? controller.Credential.Username : null;
        }
        var agent = await context.Set<InsuranceAgent>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        if (agent != default)
        {
            return agent.Credential.Password.Equals(password) ? agent.Credential.Username : null;
        }

        return null;
    }

    public async Task<string> GetRoleAsync(string username, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var consumer = await context.Set<Consumer>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        if (consumer != default)
        {
            return consumer.Credential.Username.Equals(username) ? "consumer" : null;
        }
        var controller = await context.Set<Operator>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        if (controller != default)
        {
            return controller.Credential.Username.Equals(username) ? "operator" : null;
        }
        var agent = await context.Set<InsuranceAgent>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        if (agent != default)
        {
            return agent.Credential.Username.Equals(username) ? "agent" : null;
        }

        return null;
    }

    public async Task<Consumer?> GetConsumerAsync(string username, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var consumer = await context.Set<Consumer>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        return consumer;
        
    }

    public async Task<Operator?> GetOperatorAsync(string username, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var controller = await context.Set<Operator>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        return controller;
    }

    public async Task<InsuranceAgent?> GetAgentAsync(string username, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var agent = await context.Set<InsuranceAgent>().AsQueryable().Include(x => x.Credential)
            .SingleOrDefaultAsync(x => x.Credential.Username.Equals(username), cancellationToken);
        return agent;
    }
}