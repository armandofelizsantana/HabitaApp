using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Habita.Domain.ValueObjects;

namespace Habita.Infrastructure.Persistence.EntityConfigurations.ValueConverters;

             
                                                                                    
                                                                                             
              
public sealed class PropertyCodeConverter : ValueConverter<PropertyCode, string>
{
    public PropertyCodeConverter()
        : base(v => v.Value, v => PropertyCode.Unsafe(v)) { }
}
