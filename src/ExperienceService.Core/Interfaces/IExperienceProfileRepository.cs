using ExperienceService.Core.Models;

namespace ExperienceService.Core.Interfaces;

public interface IExperienceProfileRepository
{
    Task<IEnumerable<ExperienceProfile>> GetAllByUserAsync(string userId);
    Task<ExperienceProfile?> GetByIdAsync(Guid id, string userId);
    Task<ExperienceProfile> CreateAsync(ExperienceProfile profile);
    Task<bool> DeleteAsync(Guid id, string userId);
}
