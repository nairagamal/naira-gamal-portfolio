using Microsoft.EntityFrameworkCore;
using portfolio.Entities;

namespace portfolio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectSkill> ProjectSkills { get; set; }
        public DbSet<ProjectFeature> ProjectFeatures { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<ExperienceBulletPoint> ExperienceBulletPoints { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global query filter for soft delete
            modelBuilder.Entity<Project>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<ProjectSkill>().HasQueryFilter(ps => !ps.IsDeleted);
            modelBuilder.Entity<ProjectFeature>().HasQueryFilter(pf => !pf.IsDeleted);
            modelBuilder.Entity<Experience>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<ExperienceBulletPoint>().HasQueryFilter(ebp => !ebp.IsDeleted);
            modelBuilder.Entity<Testimonial>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<ContactMessage>().HasQueryFilter(c => !c.IsDeleted);

            // Configure relationships
            modelBuilder.Entity<ProjectSkill>()
                .HasOne(ps => ps.Project)
                .WithMany(p => p.Skills)
                .HasForeignKey(ps => ps.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectFeature>()
                .HasOne(pf => pf.Project)
                .WithMany(p => p.Features)
                .HasForeignKey(pf => pf.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExperienceBulletPoint>()
               .HasOne(ebp => ebp.Experience)
               .WithMany(e => e.BulletPoints)
               .HasForeignKey(ebp => ebp.ExperienceId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}