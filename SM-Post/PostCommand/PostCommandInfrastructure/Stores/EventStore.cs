using CQRSCore.Domain;
using CQRSCore.Events;
using CQRSCore.Exceptions;
using CQRSCore.Infrastructure;
using CQRSCore.Producers;
using Domain.Aggregates;

namespace Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly IEventProducer _eventProducer;

    public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
    {
        _eventStoreRepository = eventStoreRepository;
        _eventProducer = eventProducer;
    }
    
    public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateIdAsync(aggregateId);
        
        if(expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
            throw new ConcurrencyException();

        var version = expectedVersion;
        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @events.GetType().Name;
            var eventModel = new EventModel
            {
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                EventType = eventType,
                EventData = @event,
                TimeStamp = DateTime.Now,
                Version = version
            };
            await _eventStoreRepository.SaveAsync(eventModel);
             
            var topic = Environment.GetEnvironmentVariable("EVENT_TOPIC");
            await _eventProducer.ProduceAsync(topic, @event);
        }
    }

    public async Task<List<BaseEvent>> GetEventAsync(Guid aggregateId)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateIdAsync(aggregateId);
        if (eventStream == null || !eventStream.Any())
            throw new AggregateException("Incorrect post Id provided");

        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
    }
}