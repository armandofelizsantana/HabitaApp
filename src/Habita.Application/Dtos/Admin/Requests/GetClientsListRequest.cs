namespace Habita.Application.Dtos.Admin.Requests;

public sealed record GetClientsListRequest(
    string? SearchTerm = null,
    int Page = 1,
    int PageSize = 20
);
