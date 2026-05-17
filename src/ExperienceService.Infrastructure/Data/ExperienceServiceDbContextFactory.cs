using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExperienceService.Infrastructure.Data;

public class ExperienceServiceDbContextFactory : IDesignTimeDbContextFactory<ExperienceServiceDbContext>
{
    public ExperienceServiceDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ExperienceServiceDbContext>()
            .UseNpgsql("Host=localhost;Database=jobtracker;Username=jobtracker;Password=jobtracker")
            .Options;
        return new ExperienceServiceDbContext(options);
    }
}
