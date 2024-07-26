using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostmanTester.Application.Models.DataObjects;
using PostmanTester.Domain.Entities;

namespace PostmanTester.Persistance.Contexts
{
    public class PostmanTesterDbContext : DbContext
    {
        public PostmanTesterDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id)
                .HasColumnType("int")
                .UseIdentityColumn()
                .IsRequired();

                entity.Property(u => u.Email).HasColumnType("nvarchar(32)");
                entity.Property(u => u.FirstName).HasColumnType("nvarchar(32)");
                entity.Property(u => u.LastName).HasColumnType("nvarchar(32)");
                entity.Property(u => u.PasswordSalt).HasColumnType("varbinary").HasMaxLength(500);
                entity.Property(u => u.PasswordHash).HasColumnType("varbinary").HasMaxLength(500);
                entity.Property(u => u.Phone).HasColumnType("nvarchar(32)");
                entity.Property(u => u.Address).HasColumnType("nvarchar(128)");
                entity.Property(u => u.Status).HasColumnType("tinyint");
                entity.Property(u => u.CreatedDate).HasColumnType("datetime2");
                entity.Property(u => u.UpdatedDate).HasColumnType("datetime2");
                entity.Property(a => a.DeletedDate).HasColumnType("datetime2");
                entity.Property(u => u.CreatedBy).HasColumnType("int");
                entity.Property(u => u.UpdatedBy).HasColumnType("int");
                entity.Property(a => a.DeletedBy).HasColumnType("int");

                entity.HasMany(u => u.ApiKeys).WithOne(a => a.User).HasForeignKey(a => a.UserId);
                entity.HasOne(u => u.CreatedUser).WithMany(u => u.UserCreatedUsers).HasForeignKey(u => u.CreatedBy);
                entity.HasOne(u => u.UpdatedUser).WithMany(u => u.UserUpdatedUsers).HasForeignKey(u => u.UpdatedBy);
                entity.HasOne(u => u.DeletedUser).WithMany(u => u.UserDeletedUsers).HasForeignKey(u => u.DeletedBy);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(r => r.Id)
                .HasColumnType("smallint")
                .UseIdentityColumn()
                .IsRequired();

                entity.Property(r => r.Name).HasColumnType("nvarchar(32)");
                entity.Property(r => r.Status).HasColumnType("tinyint");
                entity.Property(r => r.CreatedDate).HasColumnType("datetime2");
                entity.Property(r => r.UpdatedDate).HasColumnType("datetime2");
                entity.Property(a => a.DeletedDate).HasColumnType("datetime2");
                entity.Property(r => r.CreatedBy).HasColumnType("int");
                entity.Property(r => r.UpdatedBy).HasColumnType("int");
                entity.Property(a => a.DeletedBy).HasColumnType("int");

                entity.HasMany(r => r.Users).WithOne(u => u.Role).HasForeignKey(u => u.RoleId);
                entity.HasOne(r => r.CreatedUser).WithMany(u => u.RoleCreatedUsers).HasForeignKey(r => r.CreatedBy);
                entity.HasOne(r => r.UpdatedUser).WithMany(u => u.RoleUpdatedUsers).HasForeignKey(r => r.UpdatedBy);
                entity.HasOne(r => r.DeletedUser).WithMany(u => u.RoleDeletedUsers).HasForeignKey(r => r.DeletedBy);
            });

            modelBuilder.Entity<ApiKey>(entity =>
            {
                entity.Property(a => a.Id)
                .HasColumnType("int")
                .IsRequired();

                entity.Property(a => a.Name).HasColumnType("nvarchar(64)");
                entity.Property(a => a.Status).HasColumnType("tinyint");
                entity.Property(a => a.CreatedDate).HasColumnType("datetime2");
                entity.Property(a => a.UpdatedDate).HasColumnType("datetime2");
                entity.Property(a => a.DeletedDate).HasColumnType("datetime2");
                entity.Property(a => a.CreatedBy).HasColumnType("int");
                entity.Property(a => a.UpdatedBy).HasColumnType("int");
                entity.Property(a => a.DeletedBy).HasColumnType("int");

                entity.HasOne(r => r.CreatedUser).WithMany(u => u.ApiKeyCreatedUsers).HasForeignKey(r => r.CreatedBy);
                entity.HasOne(r => r.UpdatedUser).WithMany(u => u.ApiKeyUpdatedUsers).HasForeignKey(r => r.UpdatedBy);
                entity.HasOne(r => r.DeletedUser).WithMany(u => u.ApiKeyDeletedUsers).HasForeignKey(r => r.DeletedBy);
            });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ApiKey> ApiKeys { get; set; }
    }
}
