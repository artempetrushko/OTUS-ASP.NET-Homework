using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data.Configurations
{
    public class PromocodeConfiguration : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.Property(p => p.Code).IsRequired().HasMaxLength(100);
            builder.Property(p => p.ServiceInfo).HasMaxLength(200);
            builder.Property(p => p.PartnerName).HasMaxLength(50);
            builder
                .HasOne(p => p.Preference)
                .WithMany()
                .HasForeignKey(p => p.PreferenceId);
            builder
                .HasOne(p => p.Customer)
                .WithMany(c => c.PromoCodes)
                .HasForeignKey(p => p.CustomerId);
        }
    }
}
