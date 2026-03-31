using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Application.DTOs;
using WalletOps.Application.Interfaces;
using WalletOps.Domain.Entities;
using WalletOps.Domain.Enums;

namespace WalletOps.Application.Services
{
    public class AccountService : IAccountService
    {
        public readonly IAccountRepository _accountRepository;
        public readonly ICustomerRepository _customerRepository;
        public readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
        {
            if(request == null)
                throw new InvalidOperationException("Request cannot be null.");

            bool customerExists = await _customerRepository.ExistsByIdAsync(request.CustomerId);

            if(!customerExists)
                throw new InvalidOperationException($"Customer with ID {request.CustomerId} does not exist.");

            bool accountNumberExists = await _accountRepository.ExistsByAccountNumberAsync(request.AccountNumber);

            if(accountNumberExists)
                throw new InvalidOperationException($"Account number {request.AccountNumber} already exists.");

            var account = new Account
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                AccountNumber = request.AccountNumber,
                Currency = Currency.PYG,
                OverdraftLimit = request.OverdraftLimit,
                Balance = 0,
                Status = AccountStatus.Active
            };

            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _accountRepository.GetAllAsync(cancellationToken);
        }
    }
}
