
using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess;

public class DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
{
    public DatabaseContext CreateContext()
    {
        DbContextOptionsBuilder<DatabaseContext> options = new();
        configureDbContext(options);

        return new DatabaseContext(options.Options);
    }
}