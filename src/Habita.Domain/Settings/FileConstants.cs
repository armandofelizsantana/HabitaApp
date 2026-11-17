using System.Collections.ObjectModel;

namespace Habita.Domain.Settings;

             
                                                                       
              
public static class FileConstants
{
                 
                                                               
                  
    public const long MaxImageFileSizeBytes = 15 * 1024 * 1024;

                 
                                         
                  
    public static readonly IReadOnlySet<string> AllowedImageExtensions = new ReadOnlySet<string>(
        new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" }
    );

                 
                                            
                  
    public static readonly IReadOnlyList<string> AllowedMimeTypes =
    [
        "image/jpeg",
        "image/png",
        "image/webp",
    ];

                                                                   
    public const string PropertiesFolder = "properties";

                                                                       
    public const string ProfilesFolder = "profiles";

                                                               
    public const string TempFolder = "temp";
}
