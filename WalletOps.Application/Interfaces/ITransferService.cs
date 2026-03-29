using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Application.DTOs;

namespace WalletOps.Application.Interfaces
{
    public interface ITransferService
    {
        Task ExecuteAsync(CreateTransferRequest request, CancellationToken cancellationToken = default);
    }
}
