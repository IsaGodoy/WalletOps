using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Domain.Enums;

namespace WalletOps.Application.DTOs
{
    public class TransferListItemResponse
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public TransferStatus Status { get; set; }
    }
}
