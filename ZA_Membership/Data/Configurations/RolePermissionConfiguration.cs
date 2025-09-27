using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            // Table Name
            builder.ToTable("RolePermissions");

            // Primary Key
            builder.HasKey(rp => rp.Id);

            // Properties
            builder.Property(rp => rp.RoleId)
                   .IsRequired();

            builder.Property(rp => rp.PermissionId)
                   .IsRequired();

            // Relationships
            builder.HasOne(rp => rp.Role)
                   .WithMany(r => r.RolePermissions)
                   .HasForeignKey(rp => rp.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.Permission)
                   .WithMany(p => p.RolePermissions)
                   .HasForeignKey(rp => rp.PermissionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Optional: Unique Constraint to prevent duplicate Role-Permission pairs
            builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                   .IsUnique();
        }
    }
}
