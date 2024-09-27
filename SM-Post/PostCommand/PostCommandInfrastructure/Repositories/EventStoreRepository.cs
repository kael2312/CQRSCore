using CQRSCore.Domain;
using CQRSCore.Events;
using Infrastructure.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class EventStoreRepository: IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository(IOptions<MongoDbConfig> config)
    {
        var mongoClient = new MongoClient(config.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
    }
    
    public async Task SaveAsync(EventModel eventModel)
    {
        await _eventStoreCollection.InsertOneAsync(eventModel).ConfigureAwait(false);
    }

    public async Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId)
    {
        return await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false); 
    }
}