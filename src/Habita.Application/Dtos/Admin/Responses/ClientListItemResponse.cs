namespace Habita.Application.Dtos.Admin.Responses;

public sealed record ClientListItemResponse(
    string Id,
    string FirstName,
    string LastName,
    string? Email,
    string? UserName,
    string? Phone,
    bool IsActive
);
