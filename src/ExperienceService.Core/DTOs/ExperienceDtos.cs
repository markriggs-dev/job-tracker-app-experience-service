namespace ExperienceService.Core.DTOs;

public record ExperienceProfileResponse(
    Guid Id,
    string ProfileName,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    string FileSizeDisplay,
    DateTimeOffset UploadedAt
);

public record UploadExperienceProfileRequest(string ProfileName);
