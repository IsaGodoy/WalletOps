using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Domain.Entities;

namespace WalletOps.Infrastructure.Persistence.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.ToTable("Transfers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);
            builder.Property(x => x.Currency)
                .IsRequired();
            builder.Property(x => x.Status)
                .IsRequired();
            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(x => x.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(x => x.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
