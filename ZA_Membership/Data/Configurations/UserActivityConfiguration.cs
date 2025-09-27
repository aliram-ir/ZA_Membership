using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data.Configurations
{
    public class UserActivityConfiguration : IEntityTypeConfiguration<UserActivity>
    {
        public void Configure(EntityTypeBuilder<UserActivity> builder)
        {
            // Table Name
            builder.ToTable("UserActivities");

            // Primary Key
            builder.HasKey(ua => new { ua.UserId, ua.ActivityTime }); // اگر بخواییم ترکیب UserId و ActivityTime یکتا باشد

            // Properties
            builder.Property(ua => ua.ActivityType)
                   .IsRequired()
                   .HasConversion<int>(); // Enum به int در دیتابیس

            builder.Property(ua => ua.UsernameAttempted)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(ua => ua.IpAddress)
                   .IsRequired()
                   .HasMaxLength(45); // IPv6 هم پشتیبانی شود

            builder.Property(ua => ua.UserAgent)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(ua => ua.Browser)
                   .HasMaxLength(100);

            builder.Property(ua => ua.OperatingSystem)
                   .HasMaxLength(100);

            builder.Property(ua => ua.Device)
                   .HasMaxLength(100);

            builder.Property(ua => ua.IsSuccessful)
                   .IsRequired();

            builder.Property(ua => ua.ActivityTime)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(ua => ua.Details)
                   .HasMaxLength(1000);

            // Relationships
            builder.HasOne(ua => ua.User)
                   .WithMany(u => u.UserActivities)
                   .HasForeignKey(ua => ua.UserId)
                   .OnDelete(DeleteBehavior.SetNull); // اگر کاربر حذف شد، فعالیت‌ها نگه داشته شوند
        }
    }
}
