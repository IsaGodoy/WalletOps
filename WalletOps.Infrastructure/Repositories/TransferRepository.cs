using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WalletOps.Application.Interfaces;
using WalletOps.Domain.Entities;
using WalletOps.Infrastructure.Persistence;

namespace WalletOps.Infrastructure.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly AppDbContext _context;

        public TransferRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transfer transfer, CancellationToken cancellationToken = default)
        {
            await _context.Transfers.AddAsync(transfer, cancellationToken);
        }

        public async Task<List<Transfer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Transfers.ToListAsync(cancellationToken);
        }
    }
}
