using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Auth.Requests;

namespace Habita.Application.Interfaces.UseCases.Auth;

public interface IResendActivationUseCase : IUseCase<ResendActivationRequest>;
