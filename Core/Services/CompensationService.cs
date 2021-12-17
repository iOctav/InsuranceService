using Core.DbContexts;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CompensationService : ICompensationService
{
    public async Task<Guid?> CreateCompensationAsync(NewCompensationModel model, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var consumer = await context.Set<Consumer>().AsQueryable()
            .Include(x => x.InsuranceContracts)
            .FirstOrDefaultAsync(x => x.Id.Equals(model.ConsumerId), cancellationToken);
        if (consumer == default)
        {
            return null;
        }
        var application = await context.Set<Application>().AsQueryable()
            .Include(x => x.InsuranceContract)
            .Include(x => x.InsuranceContract.Consumer)
            .FirstOrDefaultAsync(x => x.State == RequestState.Approved 
                                      && x.InsuranceContract.Consumer.Id == model.ConsumerId,
                cancellationToken);
        
        if (application == default)
        {
            return null;
        }

        var compensationId = Guid.NewGuid();
        await context.Compensations.AddAsync(new Compensation
        {
            Id = compensationId,
            Amount = model.Amount,
            Description = model.Description,
            Controller = null,
            Date = DateTime.UtcNow,
            State = RequestState.Open,
            Verdict = null,
            InsuranceContract = application.InsuranceContract
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return compensationId;
    }

    public async Task<Guid?> ApproveCompensationAsync(CompensationVerdictModel model, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var compensation = await context.Set<Compensation>().AsQueryable()
            .SingleOrDefaultAsync(x => x.Id.Equals(model.CompensationId), cancellationToken);
        if (compensation == default)
        {
            return null;
        }
        compensation.Amount = model.Amount;
        compensation.Verdict = model.Verdict;
        if (model.Approved)
        {
            compensation.State = RequestState.Processing;
        }

        var controller = await context.Set<Operator>().AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == model.OperatorId, cancellationToken);
        if (controller == default)
        {
            return null;
        }

        compensation.Controller = controller;
        context.Compensations.Update(compensation);
        await context.SaveChangesAsync(cancellationToken);
        return compensation.Id;
    }

    public async Task<IEnumerable<Compensation>> GetCompensationsAsync(Guid consumerId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var consumer = 
            await context.Set<Consumer>().AsQueryable().Include(x => x.Compensations)
                .FirstAsync(x => x.Id == consumerId, cancellationToken);

        return consumer.Compensations;
    }

    public async Task<IEnumerable<CompensationModel>> GetCompensationsAsync(CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        
        var applications = context.Set<Application>().AsQueryable()
            .Include(x => x.InsuranceContract)
            .Where(x => x.State == RequestState.Closed)
            .ToList();
        
        var compensations = context.Set<Compensation>().AsQueryable()
            .Include(x => x.InsuranceContract)
            .Include(x => x.Controller)
            .Include(x => x.InsuranceContract)
            .Include(x => x.InsuranceContract.Consumer)
            .Include(x => x.InsuranceContract.InsuranceAgent)
            .ToList();

        return compensations
            .Where(x => applications.All(y => y.InsuranceContract.Id != x.InsuranceContract.Id))
            .Select(x => new CompensationModel
        {
            FirstName = x.InsuranceContract.Consumer.Firstname,
            Surname = x.InsuranceContract.Consumer.Surname,
            Amount = x.Amount ?? 0,
            Description = x.Description,
            Verdict = x.Verdict,
            AppliedOn = x.Date,
            CompanyName = x.InsuranceContract.InsuranceAgent.CompanyName,
            CompensationId = x.Id,
            InsuranceAmount = x.InsuranceContract.InsuranceAmount,
            OperatorFullName = x.Controller != null ? $"{x.Controller.Firstname} {x.Controller.Surname}" : null,
            Status = x.State,
            ConsumerId = x.InsuranceContract.Consumer.Id
        }).ToList();
    }

    public async Task<CompensationModel?> GetCompensationAsync(Guid compensationId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var compensation = await context.Set<Compensation>().AsQueryable()
            .Include(x => x.InsuranceContract)
            .Include(x => x.Controller)
            .Include(x => x.InsuranceContract)
            .Include(x => x.InsuranceContract.Consumer)
            .Include(x => x.InsuranceContract.InsuranceAgent)
            .SingleOrDefaultAsync(x => x.Id.Equals(compensationId), cancellationToken: cancellationToken);
        if (compensation == default)
        {
            return null;
        }
            
        return new CompensationModel
            {
                FirstName = compensation.InsuranceContract.Consumer.Firstname,
                Surname = compensation.InsuranceContract.Consumer.Surname,
                Amount = compensation.Amount ?? 0,
                Description = compensation.Description,
                Verdict = compensation.Verdict,
                AppliedOn = compensation.Date,
                CompanyName = compensation.InsuranceContract.InsuranceAgent.CompanyName,
                CompensationId = compensation.Id,
                InsuranceAmount = compensation.InsuranceContract.InsuranceAmount,
                OperatorFullName = compensation.Controller != null ? $"{compensation.Controller.Firstname} {compensation.Controller.Surname}" : null,
                Status = compensation.State
            };
    }

    public async Task<Guid?> RejectCompensationAsync(Guid compensationId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var compensation = await context.Set<Compensation>().AsQueryable()
            .SingleOrDefaultAsync(x => x.Id.Equals(compensationId), cancellationToken);
        if (compensation == default)
        {
            return null;
        }

        compensation.State = RequestState.Closed;
        context.Compensations.Update(compensation);
        await context.SaveChangesAsync(cancellationToken);
        return compensationId;
    }

    public async Task<Guid?> SignCompensationAsync(Guid compensationId, CancellationToken cancellationToken)
    {
        await using var context = new InsuranceContext();
        var compensation = await context.Set<Compensation>().AsQueryable()
            .SingleOrDefaultAsync(x => x.Id.Equals(compensationId), cancellationToken);
        if (compensation == default)
        {
            return null;
        }

        compensation.State = RequestState.Approved;
        context.Compensations.Update(compensation);
        await context.SaveChangesAsync(cancellationToken);
        return compensationId;
    }
}