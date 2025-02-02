using Microsoft.AspNetCore.WebUtilities;

namespace OnlineStore.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<string?> UploadFile(IFormFile file, string path);
    }
}