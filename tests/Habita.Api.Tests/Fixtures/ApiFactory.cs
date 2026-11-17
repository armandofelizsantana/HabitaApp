using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Habita.Infrastructure.Identity;
using Habita.Infrastructure.Identity.Contexts;
using Habita.Infrastructure.Persistence.Contexts;
using Testcontainers.MsSql;

namespace Habita.Api.Tests.Fixtures;

public sealed class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithCleanUp(true)
        .Build();

    public HttpClient Client { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration(
            (_, config) =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:RealEstateDb"] = _container.GetConnectionString(),
                    }
                );
            }
        );

        builder.ConfigureServices(services =>
        {
            services.AddIdentityBackendUseCases();

                                                                             
                                                                                                
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var inner = scope.ServiceProvider;
            inner.GetRequiredService<AppDbContext>().Database.Migrate();
            inner.GetRequiredService<IdentityContext>().Database.Migrate();
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        Client = CreateClient();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Client?.Dispose();
        await _container.DisposeAsync();
    }
}
