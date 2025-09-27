using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                   .IsRequired()
                   .IsUnicode()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .IsUnicode()
                   .HasMaxLength(200);

            builder.Property(u => u.NationalCode)
                   .IsUnicode()
                   .HasMaxLength(10);

            builder.Property(u => u.PhoneNumber)
                   .IsUnicode()
                   .HasMaxLength(11);

            builder.Property(u => u.FirstName)
                   .HasMaxLength(50);

            builder.Property(u => u.LastName)
                   .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(u => u.IsVerify)
                   .HasDefaultValue(false);

            builder.Property(u => u.IsDelete)
                   .HasDefaultValue(false);

            builder.Property(u => u.EmailConfirmed)
                   .HasDefaultValue(false);

            builder.Property(u => u.PhoneNumberConfirmed)
                   .HasDefaultValue(false);

            builder.Property(u => u.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasMany(u => u.UserRoles)
                   .WithOne(ur => ur.User)
                   .HasForeignKey(ur => ur.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserTokens)
                   .WithOne(ut => ut.User)
                   .HasForeignKey(ut => ut.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserActivities)
                   .WithOne(ua => ua.User)
                   .HasForeignKey(ua => ua.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.Addresses)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.AuthBlockList)
                   .WithOne(ab => ab.User)
                   .HasForeignKey(ab => ab.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}