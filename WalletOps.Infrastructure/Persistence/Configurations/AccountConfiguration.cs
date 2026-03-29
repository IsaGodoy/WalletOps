using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Domain.Entities;

namespace WalletOps.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AccountNumber)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Balance)
                .HasPrecision(18, 2);
            builder.Property(x => x.OverdraftLimit)
                .HasPrecision(18, 2);
            builder.Property(x => x.Currency)
                .IsRequired();
            builder.Property(x => x.Status)
                .IsRequired();
            builder.HasIndex(x => x.AccountNumber)
                .IsUnique();
        }
    }
}
