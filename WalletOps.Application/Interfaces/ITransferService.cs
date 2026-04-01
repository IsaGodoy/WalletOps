using WalletOps.Application.DTOs;

namespace WalletOps.Application.Interfaces
{
    public interface ITransferService
    {
        Task ExecuteAsync(CreateTransferRequest request, CancellationToken cancellationToken = default);
        Task<List<TransferListItemResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
