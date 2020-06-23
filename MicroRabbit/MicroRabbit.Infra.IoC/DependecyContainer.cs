using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandles;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.EventHandles;
using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TransferCreatedEvent = MicroRabbit.Transfer.Domain.Events.TransferCreatedEvent;

namespace MicroRabbit.Infra.IoC
{
    public class DependecyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {

            //Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });

            //Subscriptions
            

            //Domain Banking Commands
            services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();


            //Application Services
            services.AddTransient<IAccountService, AccountService>();

            //Data Repository
            services.AddTransient<IAccountRepository, AccountRepository>();

            //Data
            services.AddTransient<BankingDbContext>();



        }


        public static void RegisterTransferServices(IServiceCollection services)
        {

            //Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });

            services.AddTransient<TransferEventHandler>();
            //Domain Events
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

            //Application Services

            services.AddTransient<ITransferService, TransferService>();

            //Data Repository

            services.AddTransient<ITransferRepository, TransferRepository>();

            //Data

            services.AddTransient<TransferDbContext>();


        }
    }
}
