using Microsoft.AspNetCore.Mvc;
using Habita.Domain.Common;

namespace Habita.Api.Extensions;

public static class ApiBehaviorOptionsExtensions
{
    public static void ConfigureInvalidModelStateResponse(this ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context
                .ModelState.Where(e => e.Value is not null && e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
                );

            var response = ApiResponse<object>.Failure(
                ErrorCodes.ValidationError,
                "Uno o más campos tienen errores de validación.",
                errors
            );

            return new BadRequestObjectResult(response);
        };
    }
}
