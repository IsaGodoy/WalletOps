using Microsoft.EntityFrameworkCore;
using WalletOps.Application.Interfaces;
using WalletOps.Domain.Entities;
using WalletOps.Infrastructure.Persistence;

namespace WalletOps.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            _context.Accounts.Add(account);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.AnyAsync(x => x.AccountNumber == accountNumber, cancellationToken);
        }

        public async Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.ToListAsync(cancellationToken);
        }

        public async Task<List<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.Where(x => x.CustomerId == customerId).ToListAsync(cancellationToken);
        }

        public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
        {
            _context.Accounts.Update(account);
            return Task.CompletedTask;
        }
    }
}
