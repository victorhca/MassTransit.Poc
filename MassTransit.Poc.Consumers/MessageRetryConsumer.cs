using MassTransit.Poc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class MessageRetryConsumer : IConsumer<IMessageRetryType>
    {
        public Task Consume(ConsumeContext<IMessageRetryType> context)
        {
            Console.WriteLine($"Consumidor de mensagem com retry. Id: {context.Message.Id}");
            throw new ArithmeticException();
        }
    }
}
