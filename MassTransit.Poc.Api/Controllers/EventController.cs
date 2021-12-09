using MassTransit.Poc.Domain.Application;
using MassTransit.Poc.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Poc.Api.Controllers
{
    [Route("api/v1/event")]
    public class EventController : Controller
    {
        /// <summary>
        /// Enviar um evento usando exchange fanout e com 3 consumidores, 2 com sucesso e 1 com falha
        /// </summary>
        [HttpPost("consumer")]
        public async Task<IActionResult> OrchestrateFanout([FromBody] NewVehicleDto data, [FromServices] IVehicleApplication service)
        {
            await service.OrchestrateFanout(data);
            return Ok();
        }

        /// <summary>
        /// Enviar um evento para forçar o retry
        /// </summary>
        [HttpPost("retry")]
        public async Task<IActionResult> OrchestrateFanoutRetry([FromBody] NewVehicleDto data, [FromServices] IVehicleApplication service)
        {
            await service.OrchestrateFanoutRetry(data);
            return Ok();
        }

        /// <summary>
        /// Enviar um evento para forçar o redelivery
        /// </summary>
        [HttpPost("redelivery")]
        public async Task<IActionResult> OrchestrateFanoutRedelivery([FromBody] NewVehicleDto data, [FromServices] IVehicleApplication service)
        {
            await service.OrchestrateFanoutRedelivery(data);
            return Ok();
        }

        /// <summary>
        /// Enviar evento para forçar exceção com consumidor de erros
        /// </summary>
        [HttpPost("consumerErro")]
        public async Task<IActionResult> OrchestrateFanoutConsumerError([FromBody] NewVehicleDto data, [FromServices] IVehicleApplication service)
        {
            await service.OrchestrateFanoutConsumerError(data);
            return Ok();
        }

        /// <summary>
        /// Enviar um evento para exchange Direct com 2 consumers. RoutingKey: directone ou directtwo
        /// </summary>
        [HttpPost("orchestrateDirect")]
        public async Task<IActionResult> OrchestrateDirect([FromBody] NewVehicleDto data, [FromHeader] string routingKey, [FromServices] IVehicleApplication service)
        {
            await service.OrchestrateDirect(data, routingKey);
            return Ok();
        }
        /// <summary>
        /// Enviar um evento para exchange Topic com vários consumidores. RoutingKey: modelo(Uno ou Onix).estado(MG ou SP).economia(eco ou turbo)
        /// </summary>
        [HttpPost("orchestrateTopic")]
        public async Task<IActionResult> OrchestrateTopic([FromBody] NewVehicleDto data, [FromHeader] string routingKey, [FromServices] IVehicleApplication service)
        {
            await service.OrchestrateTopic(data, routingKey);
            return Ok();
        }
    }
}
