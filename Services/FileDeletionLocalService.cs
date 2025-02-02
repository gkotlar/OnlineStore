using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using OnlineStore.Interfaces;

namespace OnlineStore.Services
{
    public class FileDeletionLocalService : FileDeletionService
    {
        public bool DeleteFile(string fileName, string path)
        {
            try
            {
                if (fileName != null && path !=null)
                {
                    if (Directory.Exists(path))
                    {
                        string filePath = Path.Combine(path, fileName);

                        if (System.IO.File.Exists(filePath))
                        { 
                            System.IO.File.Delete(filePath);
                            return true;
                        }

                        return false;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Deletion Failed", ex);
            }

        }
    }
}
