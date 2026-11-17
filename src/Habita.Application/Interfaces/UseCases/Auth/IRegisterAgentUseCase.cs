using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Auth.Requests;
using Habita.Application.Dtos.Auth.Responses;

namespace Habita.Application.Interfaces.UseCases.Auth;

public interface IRegisterAgentUseCase : IUseCase<RegisterAgentRequest, AuthResponse>;
