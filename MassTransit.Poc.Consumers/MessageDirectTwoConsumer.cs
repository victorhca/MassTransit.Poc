using MassTransit.Poc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class MessageDirectTwoConsumer : IConsumer<IOrchestratorDirectType>
    {
        public Task Consume(ConsumeContext<IOrchestratorDirectType> context)
        {
            Console.WriteLine($"Consumidor 2 da exchange Direct. Id: {context.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
