using CQRSCore.Events;

namespace CQRSCore.Infrastructure;

public interface IEventStore
{
    Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
    Task<List<BaseEvent>> GetEventAsync(Guid aggregateId);
}