using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Habita.Domain.Common;
using Habita.Domain.Interfaces.Events;

namespace Habita.Infrastructure.Persistence.Contexts;

             
                                                                                          
              
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("RealEstateDb")
            ?? throw new InvalidOperationException(
                $"Connection string 'RealEstateDb' no encontrada en la configuración (entorno: {env})."
            );

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
            connectionString,
            sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        );

        return new AppDbContext(
            optionsBuilder.Options,
            TimeProvider.System,
            NullLogger<AppDbContext>.Instance,
            NullDomainEventDispatcher.Instance
        );
    }

    private sealed class NullDomainEventDispatcher : IDomainEventDispatcher
    {
                                                                                      
        public static readonly NullDomainEventDispatcher Instance = new();

                                                                                                         
        public Task DispatchAsync<TEvent>(TEvent @event, CancellationToken ct = default)
            where TEvent : IDomainEvent => Task.CompletedTask;
    }
}
