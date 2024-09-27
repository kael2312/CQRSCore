using CQRSCore.Events;

namespace CQRSCore.Domain;

public interface IEventStoreRepository
{
    Task SaveAsync(EventModel eventModel);
    Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId);
}