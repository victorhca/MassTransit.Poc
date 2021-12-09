using MassTransit.Poc.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Consumers
{
    public class VehicleEconomicConsumer : IConsumer<IOrchestratorTopicType>
    {
        public Task Consume(ConsumeContext<IOrchestratorTopicType> context)
        {
            Console.WriteLine($"Consumidor de veículo economico topic. Id: {context.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
