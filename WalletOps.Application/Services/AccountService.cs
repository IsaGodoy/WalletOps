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

        public async Task<Guid> CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                throw new InvalidOperationException("Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.AccountNumber))
            {
                throw new InvalidOperationException("Account number is required.");
            }

            if (request.OverdraftLimit < 0)
            {
                throw new InvalidOperationException("Overdraft limit cannot be negative.");
            }

            bool customerExists = await _customerRepository.ExistsByIdAsync(request.CustomerId, cancellationToken);

            if (!customerExists)
                throw new InvalidOperationException($"Customer with ID {request.CustomerId} does not exist.");

            bool accountNumberExists = await _accountRepository.ExistsByAccountNumberAsync(request.AccountNumber, cancellationToken);

            if (accountNumberExists)
                throw new InvalidOperationException($"Account number {request.AccountNumber} already exists.");

            if (!Enum.IsDefined(typeof(Currency), request.Currency))
                throw new InvalidOperationException("Invalid currency.");

            var account = new Account
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                AccountNumber = request.AccountNumber,
                Currency = (Currency)request.Currency,
                OverdraftLimit = request.OverdraftLimit,
                Balance = 0,
                Status = AccountStatus.Active
            };

            await _accountRepository.AddAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return account.Id;
        }

        public async Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _accountRepository.GetAllAsync(cancellationToken);
        }

        public async Task<List<Account>> GetMyAccountsAsync(string userId, CancellationToken cancellationToken = default)
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
            
            return await _accountRepository.GetByCustomerIdAsync(customer.Id, cancellationToken);
        }
    }
}
