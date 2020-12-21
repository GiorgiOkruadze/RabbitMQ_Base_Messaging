using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Data.Repository.Abstraction
{
    public interface ITransferRepository
    {
        Task<IEnumerable<TransferLog>> ReadAsync();
    }
}
