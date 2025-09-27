using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data.Configurations
{
    public class AuthBlockListConfiguration : IEntityTypeConfiguration<AuthBlockList>
    {
        public void Configure(EntityTypeBuilder<AuthBlockList> builder)
        {
            builder.ToTable("AuthBlockList");

            builder.HasKey(b => b.UserId); // ❗ پیشنهاد: اگر قرار است ردیف‌های چندگانه ذخیره شوند، بهتر است یک Id کلید اصلی تعریف کنید

            // اگر می‌خواهید کلید اصلی مستقل باشد:
            // builder.HasKey(b => b.Id);

            builder.Property(b => b.IpAddress)
                   .HasMaxLength(50);

            builder.Property(b => b.Email)
                   .HasMaxLength(200);

            builder.Property(b => b.PhoneNumber)
                   .HasMaxLength(50);

            builder.Property(b => b.Reason)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(b => b.BlockExpiresAt)
                   .IsRequired(false);

            // ارتباط با User (اختیاری)
            builder.HasOne(b => b.User)
                   .WithMany() // در User نیازی به Navigation نیست، مگر بخواهید لیست Blockها را ببینید
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
