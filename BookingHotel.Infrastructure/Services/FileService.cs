using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BookingHotel.Application.Interfaces.Services;

namespace BookingHotel.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folderName)
    {
        if (file == null) return string.Empty;

        var wwwRootPath = _webHostEnvironment.WebRootPath;
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var path = Path.Combine(wwwRootPath, "uploads", folderName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var fullPath = Path.Combine(path, fileName);
        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/uploads/{folderName}/{fileName}";
    }

    public void DeleteFile(string fileName)
    {
        var wwwRootPath = _webHostEnvironment.WebRootPath;
        var path = Path.Combine(wwwRootPath, fileName.TrimStart('/'));
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
