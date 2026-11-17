using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Client.Requests;
using Habita.Application.Dtos.Client.Responses;

namespace Habita.Application.Interfaces.UseCases.Client;

public interface IGetClientProfileUseCase
    : IUseCase<GetClientProfileRequest, ClientProfileResponse>;
