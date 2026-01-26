using Microsoft.Extensions.Options;

namespace Post.Command.Infrastructure.Repositories;

public class MongoDBConfiguration(IOptions<MongoDBConfig> options) : IMongoDBConfiguration
{
    public string GetCollection() => options.Value.Collection;
    public string GetConnectionString() => options.Value.ConnectionString;
    public string GetDatabase() => options.Value.Database;
}

public interface IMongoDBConfiguration
{
    public string GetConnectionString();
    public string GetDatabase();
    public string GetCollection();
}