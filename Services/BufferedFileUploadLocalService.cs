
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using OnlineStore.Interfaces;

namespace OnlineStore.Services
{
    public class BufferedFileUploadLocalService : IBufferedFileUploadService
    {
        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    string filename = GetUniqueFileName(file);
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        public string GetUniqueFileName(IFormFile myFile)
        {
            if (myFile.FileName != null)
            {
                var fileName = Path.GetFileName(myFile.FileName);
                return Path.GetFileNameWithoutExtension(fileName)
                          + "_"
                          + Guid.NewGuid().ToString().Substring(0, 4)
                          + Path.GetExtension(fileName);
            }
            else
            {
                return null;
            }
        }
    }
}
