using System.Collections.ObjectModel;

namespace Habita.Domain.Settings;

             
                                                                            
              
public sealed class FileSettings
{
    public const string SectionName = "FileSettings";

                                                                                      
    public string BasePath { get; init; } = null!;

                                                                                            
    public string UrlPrefix { get; init; } = "/uploads";

                                                                                  
    public long MaxImageFileSizeBytes { get; init; } = FileConstants.MaxImageFileSizeBytes;

                                                                               
    public IReadOnlySet<string> AllowedImageExtensions { get; init; } =
        FileConstants.AllowedImageExtensions;

                                                               
    public IReadOnlyList<string> AllowedMimeTypes { get; init; } = FileConstants.AllowedMimeTypes;

                 
                                                                                    
                  
    public bool IsAllowedImageExtension(string? extensionOrFileName)
    {
        if (string.IsNullOrWhiteSpace(extensionOrFileName))
            return false;

        var ext = NormalizeExtension(extensionOrFileName);
        return AllowedImageExtensions.Contains(ext);
    }

                 
                                                                           
                  
    public bool IsAllowedImageSize(long fileSizeBytes) =>
        fileSizeBytes > 0 && fileSizeBytes <= MaxImageFileSizeBytes;

                 
                                                                                      
                  
    private static string NormalizeExtension(string input)
    {
        var trimmed = input.Trim();
        var ext = Path.GetExtension(trimmed);
        if (string.IsNullOrWhiteSpace(ext))
            ext = trimmed.StartsWith(".") ? trimmed : $".{trimmed}";
        return ext.ToLowerInvariant();
    }
}
