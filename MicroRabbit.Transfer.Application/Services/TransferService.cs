using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Application.Services.Abstractions;
using MicroRabbit.Transfer.Data.Repository.Abstraction;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepo = default;
        private readonly IEventBus _bus = default;

        public TransferService(ITransferRepository transferRepo,IEventBus bus)
        {
            _bus = bus;
            _transferRepo = transferRepo;
        }

        public async Task<bool> CreateAsync(TransferLog item)
        {
            return await _transferRepo.CreateAsync(item);
        }

        public async Task<IEnumerable<TransferLog>> GetTransferLogs()
        {
            return await _transferRepo.ReadAsync();
        }
    }
}
