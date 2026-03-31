using System;
using System.Collections.Generic;
using System.Text;

namespace WalletOps.Application.DTOs
{
    public class CreateAccountRequest
    {
        public Guid CustomerId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public int Currency { get; set; }
        public decimal OverdraftLimit { get; set; }
    }
}
