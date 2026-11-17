using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Habita.Application.Interfaces;
using Habita.Domain.Settings;
using Habita.Infrastructure.Shared.Messaging;
using Habita.Infrastructure.Shared.Storage;

namespace Habita.Infrastructure.Shared;

             
                                                                            
              
public static class ServicesRegistration
{
                 
                                                                                              
                  
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MailSettings>(configuration.GetSection(MailSettings.SectionName));
        services.Configure<FileSettings>(configuration.GetSection(FileSettings.SectionName));

        services.AddSingleton<IRazorRenderer, RazorRenderer>();
        services.AddScoped<IEmailService, MailKitEmailService>();
        services.AddScoped<IFileService, FileService>();

        return services;
    }
}
