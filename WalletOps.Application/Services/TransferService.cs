using WalletOps.Application.DTOs;
using WalletOps.Application.Interfaces;
using WalletOps.Domain.Entities;
using WalletOps.Domain.Enums;

namespace WalletOps.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferService(
            IAccountRepository accountRepository,
            ITransferRepository transferRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _transferRepository = transferRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(
            CreateTransferRequest request,
            string? userId,
            bool isCustomer,
            CancellationToken cancellationToken = default)
        {
            if (request.Amount <= 0)
            {
                throw new InvalidOperationException("The amount must be greater than zero.");
            }

            if (request.FromAccountId == request.ToAccountId)
            {
                throw new InvalidOperationException("Source and destination accounts must be different.");
            }

            var fromAccount = await _accountRepository.GetByIdAsync(request.FromAccountId, cancellationToken);
            var toAccount = await _accountRepository.GetByIdAsync(request.ToAccountId, cancellationToken);

            if (fromAccount is null || toAccount is null)
            {
                throw new InvalidOperationException("One or both accounts were not found.");
            }

            if (fromAccount.Currency != toAccount.Currency)
            {
                throw new InvalidOperationException("Accounts must have the same currency.");
            }

            if (isCustomer)
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new InvalidOperationException("Authenticated user id was not found.");
                }
                var customer = await _customerRepository.GetByUserIdAsync(userId, cancellationToken);
                if (customer is null)
                {
                    throw new InvalidOperationException("The authenticated user is not linked to a customer.");
                }
                if (fromAccount.CustomerId != customer.Id)
                {
                    throw new InvalidOperationException("You can only transfer from your own accounts.");
                }
            }

            fromAccount.Debit(request.Amount);
            toAccount.Credit(request.Amount);

            var transfer = new Transfer
            {
                Id = Guid.NewGuid(),
                FromAccountId = fromAccount.Id,
                ToAccountId = toAccount.Id,
                Amount = request.Amount,
                Currency = fromAccount.Currency,
                Status = TransferStatus.Completed,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _accountRepository.UpdateAsync(fromAccount, cancellationToken);
            await _accountRepository.UpdateAsync(toAccount, cancellationToken);
            await _transferRepository.AddAsync(transfer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<TransferListItemResponse>> GetAllAsync(
            string? userId,
            bool isCustomer,
            CancellationToken cancellationToken = default)
        {
            var transfers = await _transferRepository.GetAllAsync(cancellationToken);

            if (isCustomer)
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new InvalidOperationException("Authenticated user id was not found.");
                }
                var customer = await _customerRepository.GetByUserIdAsync(userId, cancellationToken);
                if (customer is null)
                {
                    throw new InvalidOperationException("The authenticated user is not linked to a customer.");
                }
                var accounts = await _accountRepository.GetByCustomerIdAsync(customer.Id, cancellationToken);
                var accountIds = accounts.Select(a => a.Id).ToHashSet();
                transfers = transfers
                    .Where(t => accountIds.Contains(t.FromAccountId) || accountIds.Contains(t.ToAccountId))
                    .ToList();
            }

            return transfers.Select(t => new TransferListItemResponse
            {
                Id = t.Id,
                FromAccountId = t.FromAccountId,
                ToAccountId = t.ToAccountId,
                Amount = t.Amount,
                Currency = t.Currency,
                CreatedAtUtc = t.CreatedAtUtc,
                Status = t.Status
            }).ToList();
        }
    }
}
