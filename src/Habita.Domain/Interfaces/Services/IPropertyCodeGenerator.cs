using Habita.Domain.ValueObjects;

namespace Habita.Domain.Interfaces.Services;

             
                                                                              
                                                                                                            
                                                                  
              
public interface IPropertyCodeGenerator
{
    Task<PropertyCode> GenerateNextAsync(CancellationToken ct = default);
}
