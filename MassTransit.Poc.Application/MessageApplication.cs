using AutoMapper;
using MassTransit.Poc.Domain.Application;
using MassTransit.Poc.Domain.Dto;
using MassTransit.Poc.Domain.Events;
using MassTransit.Poc.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Application
{
    public class MessageApplication : IVehicleApplication
    {
        private readonly IPublishEndpoint _publisher;
        private readonly IMapper _mapper;

        public MessageApplication(IPublishEndpoint publisher, IMapper mapper)
        {
            _publisher = publisher;
            _mapper = mapper;
        }


        public async Task OrchestrateFanout(NewVehicleDto data)
        {
            var newVehicle = _mapper.Map<NewVehicle>(data);
            await _publisher.Publish<IOrchestratorFanoutType>(new
            {
                Id = newVehicle.VehicleId
            });

        }
        public async Task OrchestrateFanoutRetry(NewVehicleDto data)
        {
            var newVehicle = _mapper.Map<NewVehicle>(data);
            await _publisher.Publish<IMessageRetryType>(new
            {
                Id = newVehicle.VehicleId
            });

        }

        public async Task OrchestrateFanoutRedelivery(NewVehicleDto data)
        {
            var newVehicle = _mapper.Map<NewVehicle>(data);
            await _publisher.Publish<IMessageRedeliveryType>(new
            {
                Id = newVehicle.VehicleId
            });
        }
        public async Task OrchestrateFanoutConsumerError(NewVehicleDto data)
        {
            var newVehicle = _mapper.Map<NewVehicle>(data);
            await _publisher.Publish<IMessageErrorType>(new
            {
                Id = newVehicle.VehicleId
            });
        }
        public async Task OrchestrateDirect(NewVehicleDto data, string routingKey)
        {
            var newVehicle = _mapper.Map<NewVehicle>(data);
            await _publisher.Publish<IOrchestratorDirectType>(new
            {
                Id = newVehicle.VehicleId
            }, e => e.TrySetRoutingKey(routingKey));
        }
        public async Task OrchestrateTopic(NewVehicleDto data, string routingKey)
        {
            var newVehicle = _mapper.Map<NewVehicle>(data);
            await _publisher.Publish<IOrchestratorTopicType>(new
            {
                Id = newVehicle.VehicleId
            }, e => e.TrySetRoutingKey(routingKey));
        }
    }
}
