using WalletOps.Application.DTOs;
using WalletOps.Domain.Entities;

namespace WalletOps.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Guid> CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken = default);
        Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Account>> GetMyAccountsAsync(string userId, CancellationToken cancellationToken = default);
    }
}
