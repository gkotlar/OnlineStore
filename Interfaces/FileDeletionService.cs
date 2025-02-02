using Microsoft.AspNetCore.WebUtilities;

namespace OnlineStore.Interfaces
{
    public interface FileDeletionService
    {
        bool DeleteFile(string fileName, string path);
    }
}
