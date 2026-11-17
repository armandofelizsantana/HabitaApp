using Habita.Application.Interfaces.Services;

namespace Habita.Application.Dtos.Auth.Requests;

             
                                                    
              
public sealed record RegisterClientRequest(
    string FirstName,
    string LastName,
    string Phone,
    IAppFile? PhotoFile,
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword,
    string Origin
);
