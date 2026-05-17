using Microsoft.EntityFrameworkCore;
using ExperienceService.Core.Models;

namespace ExperienceService.Infrastructure.Data;

public class ExperienceServiceDbContext : DbContext
{
    public ExperienceServiceDbContext(DbContextOptions<ExperienceServiceDbContext> options) : base(options) { }

    public DbSet<ExperienceProfile> ExperienceProfiles => Set<ExperienceProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExperienceProfile>(e =>
        {
            e.ToTable("experience_profiles");
            e.HasKey(x => x.Id);
            e.Property(x => x.UserId).HasMaxLength(256).IsRequired();
            e.Property(x => x.ProfileName).HasMaxLength(256).IsRequired();
            e.Property(x => x.FileName).HasMaxLength(512).IsRequired();
            e.Property(x => x.StorageKey).HasMaxLength(1024).IsRequired();
            e.Property(x => x.ContentType).HasMaxLength(128).IsRequired();
            e.HasIndex(x => x.UserId);
        });
    }
}
