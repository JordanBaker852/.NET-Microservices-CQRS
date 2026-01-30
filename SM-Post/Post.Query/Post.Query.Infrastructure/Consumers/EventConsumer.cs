using System.Text;
using System.Text.Json;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly IApachePulsarConsumerConfiguration _configuration;
    private readonly IPulsarClient _client;
    private readonly IEventHandler _eventHandler;

    public EventConsumer(IApachePulsarConsumerConfiguration configuration, IEventHandler eventHandler)
    {
        _configuration = configuration;
        
        _client = PulsarClient.Builder()
            .ServiceUrl(new Uri(_configuration.GetServiceUrl()))
            .ConnectionSecurity(EncryptionPolicy.PreferEncrypted)
            .Build();

        _eventHandler = eventHandler;
    }

    public async void Consume(string topic)
    {
        await using var consumer = _client.NewConsumer()
            .Topic(topic)
            .SubscriptionName(_configuration.GetSubscriptionName())
            .InitialPosition(SubscriptionInitialPosition.Earliest)
            .Create();

        await foreach (var message in consumer.Messages())
        {
            var data = Encoding.UTF8.GetString(message.Data);
            var options = new JsonSerializerOptions
            {
                Converters = { new EventJsonConverter() }
            };

            var @event = JsonSerializer.Deserialize<BaseEvent>(data, options);
            var handler =_eventHandler.GetType().GetMethod("On", [ @event.GetType() ]);

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Could not find event handler method!");
            }

            handler.Invoke(_eventHandler, [@event]);
            await consumer.Acknowledge(message);
        }
    }
}