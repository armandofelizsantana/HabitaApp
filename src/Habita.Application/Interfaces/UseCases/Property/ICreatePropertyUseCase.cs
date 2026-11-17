using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Property.Requests;
using Habita.Application.Dtos.Property.Responses;

namespace Habita.Application.Interfaces.UseCases.Property;

public interface ICreatePropertyUseCase : IUseCase<CreatePropertyRequest, CreatePropertyResponse>;
