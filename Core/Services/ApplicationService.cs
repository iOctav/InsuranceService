using Core.DbContexts;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class ApplicationService: IApplicationService
{
    public async Task<Guid> CreateApplicationAsync(NewApplicationModel model, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var insuranceId = Guid.NewGuid();
        var applicationId = Guid.NewGuid();
        var consumer = await context.Consumers.FindAsync(new object?[] { model.ConsumerId }, cancellationToken);
        var agent = await context.InsuranceAgents.FindAsync(new object?[] { model.InsuranceAgentId }, cancellationToken);
        var contract = await context.InsuranceContracts.AddAsync(new InsuranceContract
        {
            Id = insuranceId,
            Compensations = Array.Empty<Compensation>(),
            StartDate = model.StartDate ?? DateTime.UtcNow,
            EndDate = model.EndDate,
            InsuranceAmount = model.InsuranceAmount,
            InsurancePremium = model.InsurancePremium,
            Consumer = consumer,
            InsuranceAgent = agent
        }, cancellationToken);
        var application = await context.Applications.AddAsync(new Application
        {
            Id = applicationId,
            Consumer = consumer,
            Date = DateTime.UtcNow,
            Operator = null,
            State = RequestState.Open,
            InsuranceContract = contract.Entity
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return applicationId;
    }

    public async Task<bool> ApproveApplicationAsync(Guid applicationId, Guid operatorId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var application = await context.Applications.FindAsync(new object?[] { applicationId }, cancellationToken);
        var controller = await context.Operators.FindAsync(new object?[] { operatorId }, cancellationToken);
        if (application == null || controller == null)
        {
            return false;
        }

        application.Operator = controller;
        application.State = RequestState.Processing;
        context.Applications.Update(application);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<ConsumerApplication>> GetApplicationsByOperatorId(Guid operatorId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var applications = context.Set<Application>().AsQueryable()
            .Include(x => x.Consumer)
            .Include(x => x.Operator)
            .Include(x => x.InsuranceContract)
            .Include(x => x.InsuranceContract.InsuranceAgent)
            .Where(x => x.State == RequestState.Open);
        return await applications.Select(x => new ConsumerApplication
        {
            Id = x.Id,
            CompanyName = x.InsuranceContract.InsuranceAgent.CompanyName,
            StartDate = x.InsuranceContract.StartDate,
            EndDate = x.InsuranceContract.EndDate,
            Amount = x.InsuranceContract.InsuranceAmount,
            Premium = x.InsuranceContract.InsurancePremium,
            Date = x.Date,
            Surname = x.Consumer.Surname,
            FirstName = x.Consumer.Firstname
        }).ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<OperatorApplicationModel>> GetApplicationsByAgentId(Guid agentId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var applications = context.Set<Application>().AsQueryable()
            .Include(x => x.Consumer)
            .Include(x => x.Operator)
            .Include(x => x.InsuranceContract)
            .Include(x => x.InsuranceContract.InsuranceAgent)
            .Where(x => x.State == RequestState.Processing && x.InsuranceContract.InsuranceAgent.Id.Equals(agentId));
        return await applications.Select(x => new OperatorApplicationModel
        {
            Id = x.Id,
            OperatorFullName = $"{x.Operator.Firstname} {x.Operator.Surname}",
            StartDate = x.InsuranceContract.StartDate,
            EndDate = x.InsuranceContract.EndDate,
            Amount = x.InsuranceContract.InsuranceAmount,
            Premium = x.InsuranceContract.InsurancePremium,
            Date = x.Date,
            Surname = x.Consumer.Surname,
            FirstName = x.Consumer.Firstname
        }).ToListAsync(cancellationToken);
    }

    public async Task<Guid?> RejectApplicationAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var application = await context.Set<Application>().AsQueryable()
            .SingleOrDefaultAsync(x => x.Id.Equals(applicationId), cancellationToken);
        if (application == default)
        {
            return null;
        }
        application.State = RequestState.Closed;
        context.Applications.Update(application);
        await context.SaveChangesAsync(cancellationToken);
        return applicationId;
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