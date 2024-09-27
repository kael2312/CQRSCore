using CQRSCore.Domain;
using CQRSCore.Handlers;
using CQRSCore.Infrastructure;
using Domain.Aggregates;

namespace Infrastructure.Handlers;

public class EventSourcingHandler: IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _eventStore;

    public EventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }
    
    public async Task SaveAsync(AggregateRoot aggregateRoot)
    {
        await _eventStore.SaveEventAsync(aggregateRoot.Id, aggregateRoot.GetUncommittedChanges(), aggregateRoot.Version);
        aggregateRoot.MarkChangesAsCommitted();
    }

    public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
    {
        var aggregate = new PostAggregate();
        var events = await _eventStore.GetEventAsync(aggregateId);
        
        if(events == null || !events.Any())
            return aggregate;
        
        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(x => x.Version).Max();
        
        return aggregate;
    }
}