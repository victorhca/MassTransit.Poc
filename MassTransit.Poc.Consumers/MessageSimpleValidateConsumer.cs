using MassTransit.Poc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class MessageSimpleValidateConsumer : IConsumer<IOrchestratorFanoutType>
    {
        public Task Consume(ConsumeContext<IOrchestratorFanoutType> context)
        {
            Console.WriteLine($"Consumidor de validação da mensagem simples. Id: {context.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
