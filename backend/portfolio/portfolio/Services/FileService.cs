using Microsoft.Extensions.Options;
using portfolio.Interfaces;

namespace portfolio.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string[] _allowedExtensions;
        private readonly long _maxFileSize;

        public FileService(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            // Load settings from configuration
            _allowedExtensions = _configuration.GetSection("UploadSettings:AllowedExtensions").Get<string[]>()
                ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };

            // Fix: Properly handle nullable int with GetValue<int?> and null-coalescing
            var maxFileSizeMB = _configuration.GetValue<int?>("UploadSettings:MaxFileSizeMB");
            _maxFileSize = (maxFileSizeMB ?? 10) * 1024 * 1024;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subDirectory)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");

            // Check file size
            if (file.Length > _maxFileSize)
                throw new ArgumentException($"File size exceeds maximum allowed size of {_maxFileSize / 1024 / 1024}MB");

            if (!IsImageFile(file))
                throw new ArgumentException($"Only image files are allowed. Allowed formats: {string.Join(", ", _allowedExtensions)}");

            // Ensure wwwroot exists
            var wwwrootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(wwwrootPath))
            {
                // Create wwwroot if it doesn't exist
                wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (!Directory.Exists(wwwrootPath))
                    Directory.CreateDirectory(wwwrootPath);
            }

            var uploadPath = Path.Combine(wwwrootPath, "uploads", subDirectory);

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Generate unique filename to prevent collisions
            var originalFileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(originalFileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for database storage (using forward slashes for web compatibility)
            return Path.Combine("uploads", subDirectory, fileName).Replace("\\", "/");
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var wwwrootPath = _environment.WebRootPath;
                if (string.IsNullOrEmpty(wwwrootPath))
                {
                    wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                // Normalize path separators
                var normalizedPath = filePath.Replace("/", Path.DirectorySeparatorChar.ToString());
                var fullPath = Path.Combine(wwwrootPath, normalizedPath);

                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - we don't want to fail the operation if file deletion fails
                Console.WriteLine($"Error deleting file {filePath}: {ex.Message}");
            }
        }

        public bool IsImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }

        public string GetFileUrl(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            try
            {
                var request = _httpContextAccessor.HttpContext?.Request;
                if (request == null)
                    return filePath;

                var normalizedPath = filePath.Replace("\\", "/");
                var baseUrl = $"{request.Scheme}://{request.Host}";
                return $"{baseUrl}/{normalizedPath}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating file URL for {filePath}: {ex.Message}");
                return filePath;
            }
        }

        public async Task<string> UpdateFileAsync(IFormFile newFile, string oldFilePath, string subDirectory)
        {
            // Delete old file if exists
            if (!string.IsNullOrEmpty(oldFilePath))
            {
                await DeleteFileAsync(oldFilePath);
            }

            // Save new file
            if (newFile != null && newFile.Length > 0)
            {
                return await SaveFileAsync(newFile, subDirectory);
            }

            return oldFilePath;
        }
    }
}