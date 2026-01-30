namespace Post.Query.Infrastructure.Config;    

public class ApachePulsarConsumerConfig
{
    public string ServiceUrl { get; set; }
    public string SubscriptionName { get; set; }
}