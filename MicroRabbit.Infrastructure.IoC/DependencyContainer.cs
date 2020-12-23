using MediatR;
using MicroRabbit.Banging.Domain.CommandHandlers;
using MicroRabbit.Banging.Domain.Commands;
using MicroRabbit.Banging.Domain.Interfaces;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infostructure.Bus;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Application.Services.Abstractions;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Data.Repository.Abstraction;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MicroRabbit.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain Bus
            services.AddTransient<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });

            //Application Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransferService, TransferService>();

            // Handlers
            services.AddScoped<TransferEventHandler>();
            services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

            //Data Banking
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<BankingDbContext>();
            
            //data Transfer
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<TransferDbContext>();
        }
    }
}
