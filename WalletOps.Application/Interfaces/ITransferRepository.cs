using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Domain.Entities;

namespace WalletOps.Application.Interfaces
{
    public interface ITransferRepository
    {
        Task AddAsync(Transfer transfer, CancellationToken cancellationToken = default);
    }
}
