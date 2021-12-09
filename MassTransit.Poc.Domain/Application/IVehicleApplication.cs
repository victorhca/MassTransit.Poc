using MassTransit.Poc.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Domain.Application
{
    public interface IVehicleApplication
    {
        Task OrchestrateFanout(NewVehicleDto data);
        Task OrchestrateFanoutRetry(NewVehicleDto data);
        Task OrchestrateFanoutRedelivery(NewVehicleDto data);
        Task OrchestrateFanoutConsumerError(NewVehicleDto data);
        Task OrchestrateDirect(NewVehicleDto data, string routingKey);
        Task OrchestrateTopic(NewVehicleDto data, string routingKey);
    }
}
