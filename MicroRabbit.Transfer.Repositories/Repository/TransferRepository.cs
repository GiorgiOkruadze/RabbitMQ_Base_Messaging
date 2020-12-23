using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository.Abstraction;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _dbContext = default;

        public TransferRepository(TransferDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(TransferLog item)
        {
            await _dbContext.TransferLogs.AddAsync(item);
            return await SaveAsync();
        }

        public async Task<IEnumerable<TransferLog>> ReadAsync()
        {
            return await _dbContext.TransferLogs.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync() >= 0;
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
