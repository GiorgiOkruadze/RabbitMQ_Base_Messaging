using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Application.Services.Abstractions;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
    {
        private readonly ITransferService _transferService = default;

        public TransferEventHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public async Task HandleAsync(TransferCreatedEvent eventArg)
        {
            var log = new TransferLog()
            {
                FromAccount = eventArg.From,
                ToAccount = eventArg.To,
                TransferAmount = eventArg.Amount,
            };
            await _transferService.CreateAsync(log);
        }
    }
}
