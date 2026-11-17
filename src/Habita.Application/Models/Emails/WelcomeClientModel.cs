namespace Habita.Application.Models.Emails;

             
                                                              
              
public sealed record WelcomeClientModel(string FullName, string ActivationLink) : IEmailModel;
