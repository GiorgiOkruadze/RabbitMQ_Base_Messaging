using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Application.Services.Abstractions
{
    public interface ITransferService
    {
        Task<IEnumerable<TransferLog>> GetTransferLogs();

        Task<bool> CreateAsync(TransferLog item);
    }
}
