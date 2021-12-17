using Core.DbContexts;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class InsuranceService : IInsuranceService
{
    public async Task<bool> SignNewInsuranceContractAsync(Guid applicationId, Guid insuranceAgentId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var application = await context.Set<Application>().AsQueryable()
            .Include(x => x.InsuranceContract)
            .Include(x => x.InsuranceContract.InsuranceAgent)
            .FirstOrDefaultAsync(x => x.Id == applicationId, cancellationToken);
        if (application == null || application.InsuranceContract.InsuranceAgent.Id != insuranceAgentId)
        {
            return false;
        }

        application.State = RequestState.Approved;
        context.Applications.Update(application);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<InsuranceAgentModel>> GetInsuranceAgentsAsync(CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var agents = context.Set<InsuranceAgent>().AsQueryable().Select(x => new InsuranceAgentModel
        {
            Id = x.Id,
            CompanyName = x.CompanyName,
            Tariff = x.Tariff
        }).ToList();
        return agents;
    }
    

    public async Task<InsuranceContractModel> GetInsuranceContract(Guid insuranceId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var contract = await context.Set<InsuranceContract>().AsQueryable()
            .Include(x => x.Compensations)
            .Include(x => x.Consumer)
            .Include(x => x.InsuranceAgent)
            .FirstAsync(x => x.Id == insuranceId, cancellationToken);
        var application = await context.Set<Application>().AsQueryable()
            .Include(x => x.InsuranceContract)
            .FirstAsync(x => x.InsuranceContract.Id == insuranceId, cancellationToken);
        return new InsuranceContractModel
        {
            Compensations = contract.Compensations.Select(x => new CompensationModel
            {
                Amount = x.Amount ?? 0,
                Description = x.Description
            }),
            State = TransformState(application.State),
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            InsuranceAmount = contract.InsuranceAmount,
            InsurancePremium = contract.InsurancePremium,
            ConsumerId = contract.Consumer.Id,
            InsuranceAgentId = contract.InsuranceAgent.Id,
            AgentCompanyName = contract.InsuranceAgent.CompanyName,
            InsuranceContractId = contract.Id
        };
        
    }

    public async Task<InsuranceContractModel?> GetConsumerInsuranceContract(Guid consumerId, CancellationToken cancellationToken)
    {
        Application? application;
        await using var context = new InsuranceContext();
        {
            var consumer = await context.Set<Consumer>().AsQueryable()
                .Include(x => x.InsuranceContracts)
                .FirstOrDefaultAsync(x => x.Id.Equals(consumerId), cancellationToken);
            if (consumer == default)
            {
                return null;
            }
            application = await context.Set<Application>().AsQueryable()
                .Include(x => x.InsuranceContract)
                .Include(x => x.InsuranceContract.Consumer)
                .FirstOrDefaultAsync(x => x.State != RequestState.Closed 
                                          && x.InsuranceContract.Consumer.Id == consumerId,
                    cancellationToken);
        }
        if (application == default)
        {
            return null;
        }
        var insuranceContract = await GetInsuranceContract(application.InsuranceContract.Id, cancellationToken);
        return insuranceContract.State == InsuranceContractState.Rejected ? null : insuranceContract;
    }

    public async Task<Guid?> StopInsuranceAsync(Guid insuranceId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var contract = await context.Set<InsuranceContract>().AsQueryable()
            .Include(x => x.Compensations)
            .FirstOrDefaultAsync(x => x.Id == insuranceId, cancellationToken);
        var application = await context.Set<Application>().AsQueryable()
            .Include(x => x.InsuranceContract)
            .FirstOrDefaultAsync(x => x.InsuranceContract.Id == insuranceId, cancellationToken);
        if (contract == default || application == default)
        {
            return null;
        }

        application.State = RequestState.Closed;
        context.Applications.Update(application);
        await context.SaveChangesAsync(cancellationToken);
        return insuranceId;
    }

    private static InsuranceContractState TransformState(RequestState state)
    {
        switch (state)
        {
            case RequestState.Open:
                return InsuranceContractState.Created;
            case RequestState.Processing:
                return InsuranceContractState.Processing;
            case RequestState.Approved:
                return InsuranceContractState.Approved;
            case RequestState.Closed:
                return InsuranceContractState.Rejected;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}