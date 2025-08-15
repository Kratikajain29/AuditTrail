using Microsoft.EntityFrameworkCore;
using AuditTrailAPI.Models;

namespace AuditTrailAPI.Data
{
    public class AuditDbContext : DbContext
    {
        public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EntityName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Action).IsRequired();
                entity.Property(e => e.ChangedFieldsJson).IsRequired();
                entity.Property(e => e.BeforeObjectJson);
                entity.Property(e => e.AfterObjectJson);
            });
        }
    }
} 