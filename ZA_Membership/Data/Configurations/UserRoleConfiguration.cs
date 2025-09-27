using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Table Name
            builder.ToTable("UserRoles");

            // Primary Key
            builder.HasKey(ur => ur.Id);

            // Properties
            builder.Property(ur => ur.UserId)
                   .IsRequired();

            builder.Property(ur => ur.RoleId)
                   .IsRequired();

            builder.Property(ur => ur.AssignedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasOne(ur => ur.User)
                   .WithMany(u => u.UserRoles)
                   .HasForeignKey(ur => ur.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(ur => ur.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Optional: Unique Constraint to prevent duplicate User-Role pairs
            builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
                   .IsUnique();
        }
    }
}
