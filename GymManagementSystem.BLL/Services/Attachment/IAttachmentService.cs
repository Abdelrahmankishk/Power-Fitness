using GymManagementSystem.BLL.Common;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.BLL.Services.Attachment;

public interface IAttachmentService
{
    Task<Result<string?>> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default);
}
