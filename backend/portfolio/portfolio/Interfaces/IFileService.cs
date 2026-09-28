namespace portfolio.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string subDirectory);
        Task DeleteFileAsync(string filePath);
        bool IsImageFile(IFormFile file);
        string GetFileUrl(string filePath);
    }
}
