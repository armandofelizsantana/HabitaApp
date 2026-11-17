using Habita.Domain.Common;

namespace Habita.Application.Common.Interfaces;

             
                                                                       
              
public interface IUseCase<in TRequest, TResponse>
    where TRequest : notnull
{
    Task<Result<TResponse>> ExecuteAsync(
        TRequest request,
        CancellationToken cancellationToken = default
    );
}

             
                                                                                 
                                                                                 
              
public interface IUseCase<in TRequest>
    where TRequest : notnull
{
    Task<Result> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}
