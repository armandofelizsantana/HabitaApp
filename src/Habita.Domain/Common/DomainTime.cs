namespace Habita.Domain.Common;

             
                                                                                      
              
public static class DomainTime
{
    private static TimeProvider _provider = TimeProvider.System;

    public static TimeProvider Provider
    {
        get => _provider;
        set => _provider = value ?? TimeProvider.System;
    }

                                                               
    public static DateTimeOffset UtcNow => _provider.GetUtcNow();
}
