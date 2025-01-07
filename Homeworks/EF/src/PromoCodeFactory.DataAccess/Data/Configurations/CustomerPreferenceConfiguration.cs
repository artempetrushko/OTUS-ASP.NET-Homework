using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data.Configurations
{
    public class CustomerPreferenceConfiguration : IEntityTypeConfiguration<CustomerPreference>
    {
        public void Configure(EntityTypeBuilder<CustomerPreference> builder)
        {
            builder
                .HasOne(cp => cp.Customer)
                .WithMany(c => c.CustomerPreferences)
                .HasForeignKey(cp => cp.CustomerId);
            builder
                .HasOne(cp => cp.Preference)
                .WithMany()
                .HasForeignKey(cp => cp.PreferenceId);
            builder.HasKey(cp => new { cp.CustomerId, cp.PreferenceId });
        }
    }
}
