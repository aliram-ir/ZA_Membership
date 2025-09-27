using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data.Configurations
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            // Table Name
            builder.ToTable("UserTokens");

            // Primary Key
            builder.HasKey(ut => ut.Id);

            // Properties
            builder.Property(ut => ut.UserId)
                   .IsRequired();

            builder.Property(ut => ut.Token)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(ut => ut.TokenType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(ut => ut.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(ut => ut.ExpiresAt)
                   .IsRequired();

            builder.Property(ut => ut.IsRevoked)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(ut => ut.DeviceInfo)
                   .HasMaxLength(200);

            builder.Property(ut => ut.IpAddress)
                   .HasMaxLength(45); // IPv6

            // Relationships
            builder.HasOne(ut => ut.User)
                   .WithMany(u => u.UserTokens)
                   .HasForeignKey(ut => ut.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Optional: Unique constraint to prevent duplicate active tokens of same type for a user
            builder.HasIndex(ut => new { ut.UserId, ut.TokenType, ut.Token })
                   .IsUnique();
        }
    }
}
