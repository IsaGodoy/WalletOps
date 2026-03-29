using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletOps.Domain.Entities;

namespace WalletOps.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FullName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.DocumentNumber)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.UserId)
                .HasMaxLength(450);
            builder.HasIndex(x => x.DocumentNumber)
                .IsUnique();
        }
    }
}
