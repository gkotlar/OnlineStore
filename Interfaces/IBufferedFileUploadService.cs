using Microsoft.AspNetCore.WebUtilities;

namespace OnlineStore.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFile(IFormFile file);
        string? GetUniqueFileName(IFormFile file);
    }
}