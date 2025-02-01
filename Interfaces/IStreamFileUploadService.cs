using Microsoft.AspNetCore.WebUtilities;

namespace OnlineStore.Interfaces
{
    public interface IStreamFileUploadService
    {
        Task<bool> UploadFile(MultipartReader reader, MultipartSection section);
    }
}
