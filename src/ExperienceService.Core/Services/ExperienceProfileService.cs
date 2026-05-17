using Microsoft.AspNetCore.Http;
using ExperienceService.Core.DTOs;
using ExperienceService.Core.Interfaces;
using ExperienceService.Core.Models;

namespace ExperienceService.Core.Services;

public class ExperienceProfileService
{
    private readonly IExperienceProfileRepository _repo;
    private readonly IStorageService _storage;

    public ExperienceProfileService(IExperienceProfileRepository repo, IStorageService storage)
    {
        _repo = repo;
        _storage = storage;
    }

    public async Task<IEnumerable<ExperienceProfileResponse>> GetAllAsync(string userId)
    {
        var profiles = await _repo.GetAllByUserAsync(userId);
        return profiles.Select(MapToResponse);
    }

    public async Task<ExperienceProfileResponse> UploadAsync(string userId, string profileName, IFormFile file)
    {
        var profileId = Guid.NewGuid();
        var storageKey = $"{userId}/{profileId}/{file.FileName}";

        using var stream = file.OpenReadStream();
        await _storage.UploadAsync(storageKey, stream, file.Length, file.ContentType);

        var profile = new ExperienceProfile
        {
            Id = profileId,
            UserId = userId,
            ProfileName = profileName,
            FileName = file.FileName,
            StorageKey = storageKey,
            ContentType = file.ContentType,
            FileSizeBytes = file.Length,
            UploadedAt = DateTimeOffset.UtcNow
        };

        var created = await _repo.CreateAsync(profile);
        return MapToResponse(created);
    }

    public async Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(Guid id, string userId)
    {
        var profile = await _repo.GetByIdAsync(id, userId);
        if (profile is null) return null;

        var stream = await _storage.DownloadAsync(profile.StorageKey);
        return (stream, profile.FileName, profile.ContentType);
    }

    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        var profile = await _repo.GetByIdAsync(id, userId);
        if (profile is null) return false;

        await _storage.DeleteAsync(profile.StorageKey);
        return await _repo.DeleteAsync(id, userId);
    }

    private static string FormatFileSize(long bytes) => bytes switch
    {
        < 1024 => $"{bytes} B",
        < 1024 * 1024 => $"{bytes / 1024.0:F1} KB",
        _ => $"{bytes / (1024.0 * 1024):F1} MB"
    };

    private static ExperienceProfileResponse MapToResponse(ExperienceProfile p) => new(
        p.Id, p.ProfileName, p.FileName, p.ContentType,
        p.FileSizeBytes, FormatFileSize(p.FileSizeBytes), p.UploadedAt);
}
