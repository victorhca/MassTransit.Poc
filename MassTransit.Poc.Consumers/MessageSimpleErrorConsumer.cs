using MassTransit.Poc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class MessageSimpleErrorConsumer : IConsumer<IOrchestratorFanoutType>
    {
        public Task Consume(ConsumeContext<IOrchestratorFanoutType> context)
        {
            Console.WriteLine($"Consumidor de mensagens forçando erro. Id: {context.Message.Id}");
            throw new ArithmeticException();
        }
    }
}
