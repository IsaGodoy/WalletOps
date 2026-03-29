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

        public async Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.ToListAsync(cancellationToken);
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
