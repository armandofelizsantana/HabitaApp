namespace Habita.Application.Models.Emails;

             
                                                                      
              
public record AccountActivationModel(
    string UserName,
    string ActivationUrl,
    string SupportEmail = "soporte@habita.local"
) : IEmailModel;
