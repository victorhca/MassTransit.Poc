using MassTransit.Poc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class MessageRedeliveryConsumer : IConsumer<IMessageRedeliveryType>
    {
        public Task Consume(ConsumeContext<IMessageRedeliveryType> context)
        {
            Console.WriteLine($"Consumidor de mensagem com redelivery. Id: {context.Message.Id}");
            throw new ArithmeticException();
        }
    }
}
