using CQRSCore.Domain;

namespace CQRSCore.Handlers;

public interface IEventSourcingHandler<T>
{
    Task SaveAsync(AggregateRoot aggregateRoot);
    Task<T> GetByIdAsync(Guid id);
    
}