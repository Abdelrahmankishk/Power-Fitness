using GymManagementSystem.BLL.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem.BLL.Services.Attachment;

public class AttachmentService(ILogger<AttachmentService> _logger, IWebHostEnvironment _env) : IAttachmentService
{
    private readonly long _MaxFileSize = 5 * 1024 * 1024;
    private readonly ILogger<AttachmentService> logger = _logger;
    private readonly IWebHostEnvironment env = _env;
    private readonly string[] allowedExtensions = { ".png", ".jpeg", ".jpg" };

    public Result Delete(string fileName, string folderName)
    {
        var fullPath = Path.Combine(env.ContentRootPath, folderName,fileName);
        try
        {
            if (!File.Exists(fullPath))
                return Result.NotFound("File Not Found");
            File.Delete(fullPath);
        }
        catch (Exception ex) {
            logger.LogError(ex, $"Failed to Delete File {fileName}");
            return Result.Fail(ex.Message);
        }
        return Result.OK();
    }

    public (Stream stream, string contentType)? GetFile(string fileName, string foldeFileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(foldeFileName))
            return null;

        var FullPath = Path.Combine(env.ContentRootPath, foldeFileName,fileName);
        if (!File.Exists(FullPath))
            return null;

        var stream = new FileStream(FullPath, FileMode.Open, FileAccess.Read);
        
        var ext = Path.GetExtension(FullPath).ToLower();
        var contentType = ext switch { 
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            _ => "appliaction/octet-stream"
        };
        return (stream, contentType);
    }

    public async Task<Result<string?>> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
    {
        if (fileStream == null || !fileStream.CanRead || fileStream.Length == 0)
            return Result<string?>.Fail("Can Not Read File");

        if (fileStream.Length > _MaxFileSize)
        {
            logger.LogError($"File Size is very Large: {fileStream.Length} Bytes");
            return Result<string?>.Fail("File Size is Greater tha 5 MB");
        }
        var ext = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(ext) || !allowedExtensions.Contains(ext))
        {
            logger.LogError($"File Extention {ext} Not Allowed");
            return Result<string?>.Fail("Error: Allowed Extentions .jpg .jpeg .png");
        }

        var uploadFolder = Path.Combine(env.ContentRootPath, folderName);
        Directory.CreateDirectory(uploadFolder);

        var storedFileName = $"{Guid.NewGuid()}{fileName}";
        var filePath = Path.Combine(uploadFolder, storedFileName);

        try
        {
            using var FileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await fileStream.CopyToAsync(FileStream, ct);
            return Result<string?>.OK(storedFileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to upload File {fileName}");
            return Result<string?>.Fail("Failed to Upload File");
        }
    }
}
