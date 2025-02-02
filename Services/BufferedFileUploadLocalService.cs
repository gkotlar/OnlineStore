
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using OnlineStore.Interfaces;

namespace OnlineStore.Services
{
    public class BufferedFileUploadLocalService : IBufferedFileUploadService
    {
        public async Task<string?> UploadFile(IFormFile file, string path)
        {                      
            try
            {
                if (file.Length > 0)
                {
                    string filename = GetUniqueFileName(file);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return filename;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        private string GetUniqueFileName(IFormFile myFile)
        { 
            var fileName = Path.GetFileName(myFile.FileName);
            return Path.GetFileNameWithoutExtension(fileName)
                        + "_"
                        + Guid.NewGuid().ToString().Substring(0, 4)
                        + Path.GetExtension(fileName);
        }
    }
}
