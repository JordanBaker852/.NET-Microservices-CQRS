using System.Text.Json;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;

namespace Post.Command.Infrastructure.Producers;

public class EventProducer : IEventProducer
{
    private readonly IApachePulsarConfiguration _apachePulsarConfiguration;
    private readonly IPulsarClient _client;

    public EventProducer(IApachePulsarConfiguration apachePulsarConfiguration)
    {
        _apachePulsarConfiguration = apachePulsarConfiguration;
        
        _client = PulsarClient.Builder()
            .ServiceUrl(new Uri(_apachePulsarConfiguration.GetServiceUrl()))
            .ListenerName(_apachePulsarConfiguration.GetListenerName())
            .ConnectionSecurity(EncryptionPolicy.EnforceEncrypted)
            .Build();
    }

    public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
    {
        await using var producer = _client.NewProducer(Schema.String)
            .Topic(topic)
            .Create();

        var messageResult = producer.NewMessage()
            .Key(Guid.NewGuid().ToString())
            .Send(JsonSerializer.Serialize(@event, @event.GetType()));

        if (messageResult.IsFaulted)
        {
            throw new Exception($"Could not produce {@event.GetType().Name} message to topic - {topic}. Reason - {messageResult.Result}");
        }
    }
}