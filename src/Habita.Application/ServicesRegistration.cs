using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Agent;
using Habita.Application.Interfaces.UseCases.Catalog;
using Habita.Application.Interfaces.UseCases.Chat;
using Habita.Application.Interfaces.UseCases.Client;
using Habita.Application.Interfaces.UseCases.Favorites;
using Habita.Application.Interfaces.UseCases.Offers;
using Habita.Application.Interfaces.UseCases.Property;
using Habita.Application.Services;
using Habita.Application.UseCases.Agent;
using Habita.Application.UseCases.Catalog;
using Habita.Application.UseCases.Chat;
using Habita.Application.UseCases.Client;
using Habita.Application.UseCases.Favorites;
using Habita.Application.UseCases.Improvement;
using Habita.Application.UseCases.Offers;
using Habita.Application.UseCases.Property;
using Habita.Application.ViewModels.Shared;

namespace Habita.Application;

             
                                                                                       
                                                                                     
                                                                          
              
public static class ServicesRegistration
{
                 
                                                                                   
                  
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
                     
        services.AddAutoMapper(_ => { }, Assembly.GetExecutingAssembly());
                           
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                              
        services.AddAllUseCases();

                                                   
        services.AddApplicationServices();

                                   
        services.AddViewModelInfrastructure();

        return services;
    }

    internal static IServiceCollection AddAllUseCases(this IServiceCollection services)
    {
        services.AddPropertyUseCases();
        services.AddOfferUseCases();
        services.AddFavoriteUseCases();
        services.AddChatUseCases();
        services.AddCatalogUseCases();
        services.AddClientUseCases();
        services.AddAgentUseCases();

        return services;
    }

    internal static IServiceCollection AddPropertyUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetPropertyListUseCase, GetPropertyListUseCase>();
        services.AddScoped<ISearchPropertyByCodeUseCase, SearchPropertyByCodeUseCase>();
        services.AddScoped<IGetPropertyDetailUseCase, GetPropertyDetailUseCase>();
        services.AddScoped<IGetAgentPropertiesUseCase, GetAgentPropertiesUseCase>();
        services.AddScoped<ICreatePropertyUseCase, CreatePropertyUseCase>();
        services.AddScoped<IUpdatePropertyUseCase, UpdatePropertyUseCase>();
        services.AddScoped<IDeletePropertyUseCase, DeletePropertyUseCase>();

        return services;
    }

    internal static IServiceCollection AddFavoriteUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAddFavoriteUseCase, AddFavoriteUseCase>();
        services.AddScoped<IGetMyFavoritesUseCase, GetMyFavoritesUseCase>();
        services.AddScoped<IRemoveFavoriteUseCase, RemoveFavoriteUseCase>();

        return services;
    }

    internal static IServiceCollection AddChatUseCases(this IServiceCollection services)
    {
        services.AddScoped<ISendMessageUseCase, SendMessageUseCase>();
        services.AddScoped<IReplyMessageUseCase, ReplyMessageUseCase>();
        services.AddScoped<IGetConversationUseCase, GetConversationUseCase>();
        services.AddScoped<IGetMyConversationsUseCase, GetMyConversationsUseCase>();

        return services;
    }

    internal static IServiceCollection AddOfferUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateOfferUseCase, CreateOfferUseCase>();
        services.AddScoped<IAcceptOfferUseCase, AcceptOfferUseCase>();
        services.AddScoped<IRejectOfferUseCase, RejectOfferUseCase>();
        services.AddScoped<IGetPropertyOffersUseCase, GetPropertyOffersUseCase>();
        services.AddScoped<IGetPropertyOfferClientsUseCase, GetPropertyOfferClientsUseCase>();
        services.AddScoped<IGetMyOffersUseCase, GetMyOffersUseCase>();

        return services;
    }

    internal static IServiceCollection AddClientUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetClientDashboardUseCase, GetClientDashboardUseCase>();
        services.AddScoped<IGetClientProfileUseCase, GetClientProfileUseCase>();

        return services;
    }

    internal static IServiceCollection AddAgentUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetAgentProfileUseCase, GetAgentProfileUseCase>();
        services.AddScoped<IGetPublicAgentsUseCase, GetPublicAgentsUseCase>();

        return services;
    }

    internal static IServiceCollection AddCatalogUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateImprovementUseCase, CreateImprovementUseCase>();
        services.AddScoped<IUpdateImprovementUseCase, UpdateImprovementUseCase>();
        services.AddScoped<IGetAllImprovementsUseCase, GetAllImprovementsUseCase>();
        services.AddScoped<IDeleteImprovementUseCase, DeleteImprovementUseCase>();
        services.AddScoped<IGetImprovementByIdUseCase, GetImprovementByIdUseCase>();

        services.AddScoped<ICreatePropertyTypeUseCase, CreatePropertyTypeUseCase>();
        services.AddScoped<IUpdatePropertyTypeUseCase, UpdatePropertyTypeUseCase>();
        services.AddScoped<IGetAllPropertyTypesUseCase, GetAllPropertyTypesUseCase>();
        services.AddScoped<IDeletePropertyTypeUseCase, DeletePropertyTypeUseCase>();
        services.AddScoped<IGetPropertyTypeByIdUseCase, GetPropertyTypeByIdUseCase>();
        services.AddScoped<IGetPublicPropertyTypesUseCase, GetPublicPropertyTypesUseCase>();

                   
        services.AddScoped<ICreateSaleTypeUseCase, CreateSaleTypeUseCase>();
        services.AddScoped<IUpdateSaleTypeUseCase, UpdateSaleTypeUseCase>();
        services.AddScoped<IGetAllSaleTypesUseCase, GetAllSaleTypesUseCase>();
        services.AddScoped<IDeleteSaleTypeUseCase, DeleteSaleTypeUseCase>();
        services.AddScoped<IGetSaleTypeByIdUseCase, GetSaleTypeByIdUseCase>();

        return services;
    }

    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IAgentService, AgentService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IAdminService, AdminService>();

        return services;
    }

    internal static IServiceCollection AddViewModelInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IViewModelBuilder<BaseViewModel>, ViewModelBuilder>();

        return services;
    }
}
