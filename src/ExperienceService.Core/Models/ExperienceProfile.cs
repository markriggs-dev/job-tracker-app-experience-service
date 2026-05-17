namespace ExperienceService.Core.Models;

public class ExperienceProfile
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string ProfileName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
}
