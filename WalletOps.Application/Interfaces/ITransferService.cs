using WalletOps.Application.DTOs;

namespace WalletOps.Application.Interfaces
{
    public interface ITransferService
    {
        Task ExecuteAsync(CreateTransferRequest request, string? userId, bool isCustomer, CancellationToken cancellationToken = default);
        Task<List<TransferListItemResponse>> GetAllAsync(string? userId, bool isCustomer, CancellationToken cancellationToken = default);
    }
}
