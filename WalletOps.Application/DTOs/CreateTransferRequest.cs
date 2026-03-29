using System;
using System.Collections.Generic;
using System.Text;

namespace WalletOps.Application.DTOs
{
    public class CreateTransferRequest
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
