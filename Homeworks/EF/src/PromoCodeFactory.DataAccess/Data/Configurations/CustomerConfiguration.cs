using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.FirstName).HasMaxLength(50);
            builder.Property(c => c.LastName).HasMaxLength(50);
            builder.Property(c => c.Email).HasMaxLength(100);
            builder
                .HasMany(c => c.CustomerPreferences)
                .WithOne(cp => cp.Customer)
                .HasForeignKey(cp => cp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
