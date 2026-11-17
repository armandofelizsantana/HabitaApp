using Habita.Application.Interfaces.Services;

namespace Habita.Application.Dtos.Auth.Requests;

             
                                                   
                                                        
              
public sealed record RegisterAgentRequest(
    string FirstName,
    string LastName,
    string Phone,
    IAppFile? PhotoFile,
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword
);
