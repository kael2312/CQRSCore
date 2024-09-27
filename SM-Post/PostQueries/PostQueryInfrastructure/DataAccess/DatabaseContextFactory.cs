using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class DatabaseContextFactory
{
    private readonly Action<DbContextOptionsBuilder> _configureDbContext;

    public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
    {
        _configureDbContext = configureDbContext;
    }
    
    public DatabaseContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        _configureDbContext(optionsBuilder);
        return new DatabaseContext(optionsBuilder.Options);
    }
}