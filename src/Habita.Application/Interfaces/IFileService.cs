using Habita.Application.Interfaces.Services;

namespace Habita.Application.Interfaces;

             
                                                                                    
              
public interface IFileService
{
    string GetAbsolutePath(string relativePath);
    Task<string> UploadFileAsync(IAppFile file, string folderName);
    Task<string> UploadTempFileAsync(IAppFile file);
    bool IsImageValid(IAppFile file);
    void DeleteFile(string filePath);
    Task DeleteFileAsync(string filePath);
}
