namespace Habita.Application.Dtos.Auth.Requests;

             
                                                                               
              
public sealed record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmPassword
);
