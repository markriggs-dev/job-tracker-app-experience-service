using Microsoft.EntityFrameworkCore;
using ExperienceService.Core.Interfaces;
using ExperienceService.Core.Models;
using ExperienceService.Infrastructure.Data;

namespace ExperienceService.Infrastructure.Repositories;

public class ExperienceProfileRepository : IExperienceProfileRepository
{
    private readonly ExperienceServiceDbContext _db;

    public ExperienceProfileRepository(ExperienceServiceDbContext db) => _db = db;

    public async Task<IEnumerable<ExperienceProfile>> GetAllByUserAsync(string userId) =>
        await _db.ExperienceProfiles
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.UploadedAt)
            .ToListAsync();

    public async Task<ExperienceProfile?> GetByIdAsync(Guid id, string userId) =>
        await _db.ExperienceProfiles.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

    public async Task<ExperienceProfile> CreateAsync(ExperienceProfile profile)
    {
        _db.ExperienceProfiles.Add(profile);
        await _db.SaveChangesAsync();
        return profile;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        var profile = await _db.ExperienceProfiles.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        if (profile is null) return false;
        _db.ExperienceProfiles.Remove(profile);
        await _db.SaveChangesAsync();
        return true;
    }
}
