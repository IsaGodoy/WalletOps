using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Application.DTOs;
using WalletOps.Domain.Entities;

namespace WalletOps.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
        Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
