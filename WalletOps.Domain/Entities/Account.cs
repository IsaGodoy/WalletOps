using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Domain.Enums;

namespace WalletOps.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public Currency Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal OverdraftLimit { get; set; }
        public AccountStatus Status { get; set; }

        public decimal GetAvailableFunds()
        {
            return Balance + OverdraftLimit;
        }

        public bool IsActive()
        {
            return Status == AccountStatus.Active;
        }

        public bool CanDebit(decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            if (!IsActive())
            {
                return false;
            }

            return GetAvailableFunds() >= amount;
        }

        public void Debit(decimal amount)
        {
            if (!CanDebit(amount))
            {
                throw new InvalidOperationException("Insufficient funds or account is not active.");
            }

            Balance -= amount;
        }

        public void Credit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("The amount must be greater than zero.");
            }

            if (!IsActive())
            {
                throw new InvalidOperationException("The account is not active.");
            }

            Balance += amount;
        }
    }
}
