namespace Habita.Application.Dtos.Auth.Requests;

             
                                                                                 
              
public sealed record LoginRequest(string UserNameOrEmail, string Password, bool RememberMe = false);
