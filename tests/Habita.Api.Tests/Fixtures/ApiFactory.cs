using System.Security.Cryptography;
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
    private readonly string _jwtKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithCleanUp(true)
        .Build();

    public HttpClient Client { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration(
            (_, config) =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:RealEstateDb"] = _container.GetConnectionString(),
                        ["JwtSettings:SecretKey"] = _jwtKey,
                        ["JwtSettings:Issuer"] = "Habita.Tests",
                        ["JwtSettings:Audience"] = "Habita.Tests.Client",
                        ["JwtSettings:DurationInMinutes"] = "60",
                    }
                );
            }
        );

        builder.ConfigureServices(services =>
        {
            services.AddIdentityBackendUseCases();
            services.AddSingleton<Habita.Application.Interfaces.IEmailService, TestEmailService>();

                                                                             
                                                                                                
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

internal sealed class TestEmailService : Habita.Application.Interfaces.IEmailService
{
    public Task<bool> SendEmailAsync<T>(string to, string subject, string templateName, T model, CancellationToken cancellationToken = default)
        where T : Habita.Application.Models.Emails.IEmailModel => Task.FromResult(true);
}
