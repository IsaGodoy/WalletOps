using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Domain.Entities;

namespace WalletOps.Infrastructure.Persistence.Configurations
{
    public class OperatorConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> builder)
        {
            builder.ToTable("Operators");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FullName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.EmployeeCode)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.UserId)
                .HasMaxLength(450)
                .IsRequired();
            builder.HasIndex(x => x.EmployeeCode)
                .IsUnique();
            builder.HasIndex(x => x.UserId)
                .IsUnique();
        }
    }
}
