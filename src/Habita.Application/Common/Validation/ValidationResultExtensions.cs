using FluentValidation.Results;
using Habita.Domain.Common;

namespace Habita.Application.Common.Validation;

             
                                                                                 
                                                                      
              
public static class ValidationResultExtensions
{
    private const string CombinedValidationCode = "Validation";
    private const string Separator = "; ";

                 
                                                                                                   
                                                           
                  
    public static Error ToError(this ValidationResult result, string code = CombinedValidationCode)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.Errors.Count == 0)
        {
            return Error.Validation(
                CombinedValidationCode,
                "Falló la validación sin errores detallados."
            );
        }

        var message = string.Join(
            Separator,
            result.Errors.Select(failure => failure.ErrorMessage)
        );

        return Error.Validation(code, message);
    }

                 
                                                                                               
                                                           
                  
    public static Result ToResult(
        this ValidationResult result,
        string code = CombinedValidationCode
    )
    {
        ArgumentNullException.ThrowIfNull(result);
        return Result.Failure(result.ToError(code));
    }

                 
                                                                                                  
                                                              
                  
    public static Result<T> ToResult<T>(
        this ValidationResult result,
        string code = CombinedValidationCode
    )
    {
        ArgumentNullException.ThrowIfNull(result);
        return Result<T>.Failure(result.ToError(code));
    }
}
