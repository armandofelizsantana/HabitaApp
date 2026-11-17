using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Admin.Requests;
using Habita.Application.Dtos.Admin.Responses;

namespace Habita.Application.Interfaces.UseCases.Admin;

public interface IGetAdminByIdUseCase
    : IUseCase<GetAdminByIdRequest, AdminListItemResponse>;
