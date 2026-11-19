using Habita.Api.Tests.Helpers;
using Habita.Application.Dtos.Property.Requests;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.ValueObjects;
using Habita.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Habita.Api.Tests.Integration;

public sealed class PropertyFilterTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task CombinedFilters_RespectBoundsStatusAndAgent()
    {
        await PropertySeeder.SeedAsync(factory);
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var apartment = await db.Properties.SingleAsync(p => p.Code == PropertyCode.Unsafe("100002"));
        var criteria = new PropertySearchCriteria(2, 3200000m, 3200000m, 2, 1, "100002");
        var filter = criteria.Within<Property>(
            p => p.Status == PropertyStatus.Available && p.AgentId == apartment.AgentId, p => p);

        (await db.Properties.Where(filter).Select(p => p.Id).ToListAsync()).Should().Equal(apartment.Id);
        var outsideAgent = criteria.Within<Property>(p => p.AgentId != apartment.AgentId, p => p);
        (await db.Properties.CountAsync(outsideAgent)).Should().Be(0);
    }

    [Fact]
    public async Task FavoriteFilters_CombineClientScopeWithPropertyFieldsBeforePaging()
    {
        await PropertySeeder.SeedAsync(factory);
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var apartment = await db.Properties.SingleAsync(p => p.Code == PropertyCode.Unsafe("100002"));
        var other = await db.Properties.SingleAsync(p => p.Code == PropertyCode.Unsafe("100003"));
        var clientId = Guid.NewGuid().ToString();
        db.FavoriteProperties.AddRange(
            FavoriteProperty.Create(clientId, apartment.Id).GetValue(),
            FavoriteProperty.Create(clientId, other.Id).GetValue(),
            FavoriteProperty.Create(Guid.NewGuid().ToString(), apartment.Id).GetValue());
        await db.SaveChangesAsync();

        var criteria = new PropertySearchCriteria(null, null, 3200000m, null, 1, null);
        var filter = criteria.Within<FavoriteProperty>(f => f.ClientId == clientId, f => f.Property);
        var matches = db.FavoriteProperties.Where(filter);

        (await matches.CountAsync()).Should().Be(1);
        (await matches.OrderBy(f => f.Id).Take(1).Select(f => f.PropertyId).ToListAsync()).Should().Equal(apartment.Id);
    }
}
