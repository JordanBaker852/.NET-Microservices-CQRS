using Microsoft.Extensions.Options;
using Post.Command.Infrastructure.Config;

namespace Post.Command.Infrastructure.Producers;

public class ApachePulsarConfiguration(IOptions<ApachePulsarConfig> options) : IApachePulsarConfiguration
{
    public string GetListenerName() => options.Value.ListenerName;
    public string GetServiceUrl() => options.Value.ServiceUrl;
}

public interface IApachePulsarConfiguration
{
    public string GetServiceUrl();
    public string GetListenerName();
}