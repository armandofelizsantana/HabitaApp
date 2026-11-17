using Microsoft.AspNetCore.Authorization;
using Habita.Domain.Enums;

namespace Habita.Api.Policies;

public static class ApiAuthorizationPolicies
{
    public const string ApiAccess = "ApiAccess";

    public static void AddApiAuthorizationPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(
            ApiAccess,
            policy => policy.RequireRole(nameof(Roles.Admin), nameof(Roles.Developer))
        );
    }
}
