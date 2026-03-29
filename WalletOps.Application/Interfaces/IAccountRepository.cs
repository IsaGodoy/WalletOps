using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Domain.Entities;

namespace WalletOps.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Account account, CancellationToken cancellationToken = default);
    }
}
