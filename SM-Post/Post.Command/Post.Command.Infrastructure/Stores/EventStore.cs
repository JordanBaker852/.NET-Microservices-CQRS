using System.Transactions;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Command.Domain.Aggregates;

namespace Post.Command.Infrastructure.Stores;

public class EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer) : IEventStore
{
    public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await eventStoreRepository.FindByAggregateId(aggregateId);

        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
        {
            throw new ConcurrencyException("The expected version was not found");
        }

        var version = expectedVersion;

        foreach(var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;

            var expectedModel = new EventModel
            {
                Timestamp = DateTime.UtcNow,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            using var transaction = new TransactionScope();

            await eventStoreRepository.SaveAsync(@expectedModel);

            var topic = Environment.GetEnvironmentVariable("PULSAR_TOPIC") ?? "DefaultEvents";
            await eventProducer.ProduceAsync(topic, @event);

            transaction.Complete();
        }
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await eventStoreRepository.FindByAggregateId(aggregateId);

        if (eventStream == null || !eventStream.Any())
        {
            throw new AggregateNotFoundException("Incorrect post ID provided!");
        }

        return eventStream.Select(x => x.EventData).OrderBy(x => x.Version).ToList();
    }
}