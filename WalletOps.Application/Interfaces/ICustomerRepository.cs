using WalletOps.Domain.Entities;

namespace WalletOps.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<bool> ExistsByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Customer?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
