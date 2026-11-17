using Habita.Application.Interfaces.Services;

namespace Habita.Application.Dtos.Property.Requests;

public sealed record CreatePropertyRequest(
    string Title,
    string Description,
    decimal Price,
    string Currency,
    decimal SizeM2,
    int Bedrooms,
    int Bathrooms,
    int PropertyTypeId,
    int SaleTypeId,
    IReadOnlyList<IAppFile> ImageFiles,
    IReadOnlyList<int> ImprovementIds
);
