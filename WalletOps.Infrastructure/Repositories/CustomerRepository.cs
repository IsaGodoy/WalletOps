using Microsoft.EntityFrameworkCore;
using WalletOps.Application.Interfaces;
using WalletOps.Domain.Entities;
using WalletOps.Infrastructure.Persistence;

namespace WalletOps.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _context.Customers.Add(customer);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.AnyAsync(x => x.DocumentNumber == documentNumber, cancellationToken);
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers.ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Customer?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _context.Customers.Update(customer);
            return Task.CompletedTask;
        }
    }
}
