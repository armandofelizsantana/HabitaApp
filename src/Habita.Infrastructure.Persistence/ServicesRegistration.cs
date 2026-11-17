using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Habita.Domain.Interfaces.Events;
using Habita.Domain.Interfaces.Persistence;
using Habita.Domain.Interfaces.Persistence.Repositories;
using Habita.Domain.Interfaces.Services;
using Habita.Infrastructure.Persistence.Contexts;
using Habita.Infrastructure.Persistence.Persistence;
using Habita.Infrastructure.Persistence.Repositories;
using Habita.Infrastructure.Persistence.Services;
using Habita.Infrastructure.Persistence.Services.EventHandlers;

namespace Habita.Infrastructure.Persistence;

public static class ServicesRegistration
{
                 
                                                                                      
                  
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("RealEstateDb"),
                sql =>
                {
                    sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null
                    );
                    sql.CommandTimeout(30);
                }
            )
        );

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IFavoritePropertyRepository, FavoritePropertyRepository>();

        services.AddScoped<IPropertyCodeGenerator, PropertyCodeGenerator>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<
            IEventHandler<Domain.Events.OfferAcceptedEvent>,
            OfferAcceptedEventHandler
        >();

        services.AddScoped<IOfferPolicy, OfferPolicy>();

        return services;
    }
}
