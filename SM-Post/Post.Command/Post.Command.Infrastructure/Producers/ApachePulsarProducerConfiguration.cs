using Microsoft.Extensions.Options;
using Post.Command.Infrastructure.Config;

namespace Post.Command.Infrastructure.Producers;

public class ApachePulsarProducerConfiguration(IOptions<ApachePulsarProducerConfig> options) : IApachePulsarProducerConfiguration
{
    public string GetServiceUrl() => options.Value.ServiceUrl;
}

public interface IApachePulsarProducerConfiguration
{
    string GetServiceUrl();
}