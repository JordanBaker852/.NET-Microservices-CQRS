using CQRS.Core.Domain;
using CQRS.Core.Events;
using MongoDB.Driver;

namespace Post.Command.Infrastructure.Repositories;

public class EventStoreRespository : IEventStoreRepository
{
    private readonly IMongoDBConfiguration _mongoDBConfiguration;
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRespository(IMongoDBConfiguration mongoDBConfiguration)
    {
        _mongoDBConfiguration = mongoDBConfiguration;

        var mongoClient = new MongoClient(_mongoDBConfiguration.GetConnectionString());
        var mongoDatabase = mongoClient.GetDatabase(_mongoDBConfiguration.GetDatabase());

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(_mongoDBConfiguration.GetCollection());   
    }

    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        return await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
    }

    public async Task SaveAsync(EventModel @event)
    {
        await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
    }
}