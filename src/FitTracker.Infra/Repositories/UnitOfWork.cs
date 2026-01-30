using FitTracker.Domain.Repositories;
using FitTracker.Infra.Context;

namespace FitTracker.Infra.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _dbContext;

    public UnitOfWork(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}