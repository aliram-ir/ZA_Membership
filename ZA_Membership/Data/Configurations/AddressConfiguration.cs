using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AddressTitle)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.Province)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.Town)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.Street)
                   .HasMaxLength(200);

            builder.Property(a => a.Street2)
                   .HasMaxLength(200);

            builder.Property(a => a.District)
                   .HasMaxLength(100);

            builder.Property(a => a.Floor)
                   .HasMaxLength(20);

            builder.Property(a => a.SideFloor)
                   .HasMaxLength(20);

            builder.Property(a => a.Number)
                   .HasMaxLength(20);

            builder.Property(a => a.PostalCode)
                   .HasMaxLength(10);

            builder.Property(a => a.BuildingName)
                   .HasMaxLength(200);

            builder.Property(a => a.Description)
                   .HasMaxLength(500);

            builder.Property(a => a.IsDefault)
                   .IsRequired();

            // رابطه با User
            builder.HasOne(a => a.User)
                   .WithMany(u => u.Addresses) // باید در کلاس User: ICollection<Address> Addresses {get;set;} اضافه بشه
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
