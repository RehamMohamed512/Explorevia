using Explorevia.IRepository;

namespace Explorevia.Repository
{
    public class FileUploadService: IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
               return null;
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var relativePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");
            return "/" + relativePath;
        }
    }
}
