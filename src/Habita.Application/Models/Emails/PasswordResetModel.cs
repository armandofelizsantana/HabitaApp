namespace Habita.Application.Models.Emails;

             
                                                            
              
public sealed record PasswordResetModel(string FullName, string ResetLink) : IEmailModel;
