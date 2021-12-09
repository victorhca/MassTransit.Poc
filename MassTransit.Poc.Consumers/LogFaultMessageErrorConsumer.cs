using MassTransit.Poc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class LogFaultMessageErrorConsumer : IConsumer<Fault<IMessageErrorType>>
    {
        public Task Consume(ConsumeContext<Fault<IMessageErrorType>> context)
        {
            Console.WriteLine($"Consumidor de mensagem com erro. Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
