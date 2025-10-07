using Microsoft.EntityFrameworkCore;
using ProjectsRequirements_API.Models;

namespace ProjectsRequirements_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();

        public DbSet<Iteration> Iterations => Set<Iteration>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map Users (if table already exists with different column names)
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                      .UseIdentityColumn(1, 1);

                entity.Property(u => u.PasswordHash)
                      .HasColumnName("Password_Hash")
                      .HasColumnType("varbinary(8000)");

                entity.Property(u => u.CreatedAt)
                      .HasColumnName("Created_At")
                      .HasColumnType("datetime2")
                      .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            // Map Projects
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                      .UseIdentityColumn(1, 1);

                entity.Property(p => p.Title)
                      .HasMaxLength(400)
                      .IsRequired();

                entity.Property(p => p.CreatedAt)
                      .HasColumnName("Created_At")
                      .HasColumnType("datetime2")
                      .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.Property(p => p.CompletedAt)
                      .HasColumnName("Completed_At")
                      .HasColumnType("datetime2");

                entity.Property(p => p.CreatedBy)
                      .HasColumnName("Created_By")
                      .IsRequired();
            });

            modelBuilder.Entity<Iteration>(entity =>
            {
                entity.ToTable("Iterations");
                entity.HasKey(i => i.Id);

                entity.Property(i => i.ProjectId).HasColumnName("Project_Id");
                entity.Property(i => i.VersionNumber).HasColumnName("Version_Number");
                entity.Property(i => i.CompiledAt).HasColumnName("Compiled_At").HasColumnType("datetime2(3)");
                entity.Property(i => i.CreatedBy).HasColumnName("Created_By");
                entity.Property(i => i.Accepted).HasColumnName("Accepted");
                entity.Property(i => i.Description).HasColumnName("Description"); // nvarchar(max)
            });

        }
    }
}
