using Habita.Application.Dtos.Auth.Requests;
using Habita.Application.Dtos.Auth.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces;

             
                                                                 
                                                                                       
              
public interface IAccountServiceForWebApp
{
    Task<Result<AuthResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result<AuthResponse>> RegisterClientAsync(
        RegisterClientRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result<AuthResponse>> RegisterAgentAsync(
        RegisterAgentRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result> ActivateAccountAsync(
        ActivateAccountRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result> ResendActivationAsync(
        ResendActivationRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result> ForgotPasswordAsync(
        ForgotPasswordRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result> ResetPasswordAsync(
        ResetPasswordRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Result> ChangePasswordAsync(
        ChangePasswordRequest request,
        CancellationToken cancellationToken = default
    );

    Task LogoutAsync();
}
