using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data.Configurations
{
    public class AuthFailedAttemptConfiguration : IEntityTypeConfiguration<AuthFailedAttempt>
    {
        public void Configure(EntityTypeBuilder<AuthFailedAttempt> builder)
        {
            builder.ToTable("AuthFailedAttempts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AttemptType)
                   .IsRequired();

            builder.Property(a => a.SubjectIdentifier)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(a => a.IpAddress)
                   .IsRequired()
                   .HasMaxLength(45); // IPv6 ready

            builder.Property(a => a.AttemptTime)
                   .IsRequired();

            // ایندکس‌ها برای بهینه‌سازی گزارش‌ها و چک امنیتی
            builder.HasIndex(a => a.SubjectIdentifier);
            builder.HasIndex(a => a.IpAddress);
            builder.HasIndex(a => a.AttemptTime);
        }
    }
}
