using Microsoft.AspNetCore.Http;
using Habita.Application.Interfaces.Services;
using Habita.Application.Models;

namespace Habita.Infrastructure.Shared.Adapters;

public static class FormFileToAppFileAdapter
{
    public static async Task<IAppFile> AdaptAsync(IFormFile formFile)
    {
                                         
        var ms = new MemoryStream();
                                                              
        await formFile.CopyToAsync(ms);
                                                                                               
        ms.Position = 0;
        return new AppFile(ms, formFile.FileName, formFile.ContentType, formFile.Length);
    }
}
