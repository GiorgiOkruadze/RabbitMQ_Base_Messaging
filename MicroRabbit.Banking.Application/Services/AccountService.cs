using MediatR;
using MicroRabbit.Banging.Domain.Commands;
using MicroRabbit.Banging.Domain.Events;
using MicroRabbit.Banging.Domain.Interfaces;
using MicroRabbit.Banging.Domain.Models;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo = default;
        private readonly IEventBus _bus = default;
        private readonly IMediator _mediator = default;

        public AccountService(IAccountRepository accountRepo, IEventBus bus, IMediator mediator)
        {
            _bus = bus;
            _mediator = mediator;
            _accountRepo = accountRepo;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _accountRepo.GetAccounts();
        }

        public async Task TransferAsync(AccountTransfer accountTransfer)
        {
            var request = new CreateTransferCommand() 
            { 
                From = accountTransfer.FromAccount, 
                To = accountTransfer.ToAccount,
                Amount = accountTransfer.TransferAmount
            };

            try
            {
                await _mediator.Send(request);
            }
            catch(Exception ex)
            {
                var sms = ex.Message;
            }
        }
    }
}
