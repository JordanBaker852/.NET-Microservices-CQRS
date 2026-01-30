using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Config;

namespace Post.Query.Infrastructure.Consumers;

public class ApachePulsarConsumerConfiguration(IOptions<ApachePulsarConsumerConfig> options) : IApachePulsarConsumerConfiguration
{
    public string GetServiceUrl() => options.Value.ServiceUrl;
    public string GetSubscriptionName() => options.Value.SubscriptionName;
}

public interface IApachePulsarConsumerConfiguration
{
    string GetServiceUrl();
    string GetSubscriptionName();
}