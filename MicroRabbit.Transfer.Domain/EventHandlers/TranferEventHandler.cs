using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class TranferEventHandler : IEventHandler<TransferCreatedEvent>
    {
        public async Task HandleAsync(TransferCreatedEvent eventArg)
        {
            var item = eventArg;  
        }
    }
}
