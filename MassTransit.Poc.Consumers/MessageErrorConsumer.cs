using MassTransit.Poc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class MessageErrorConsumer : IConsumer<IMessageErrorType>
    {
        public Task Consume(ConsumeContext<IMessageErrorType> context)
        {
            Console.WriteLine($"Consumidor de mensagem para gerar registro de erro. VehicleId: {context.Message.Id}");
            throw new ArithmeticException();
        }
    }
}
