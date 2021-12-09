using MassTransit;
using MassTransit.Poc.Domain.Events;
using System;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class MessageSimpleConsumer : IConsumer<IOrchestratorFanoutType>
    {
        public Task Consume(ConsumeContext<IOrchestratorFanoutType> context)
        {
            Console.WriteLine($"Consumidor de mensagem simples. Id: {context.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
