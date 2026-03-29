using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly IUnitOfWork _unitOfWork;

        public TransferService(IAccountRepository accountRepository, ITransferRepository transferRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(CreateTransferRequest request, CancellationToken cancellationToken = default)
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
    }
}
