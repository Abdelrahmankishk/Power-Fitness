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
