using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Availability.Api.Application.Commands.Availability;
using Availability.Api.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Availability.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AvailabilityController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> CreateAvailabilityRecord([FromBody] AvailabilityRecordDto availabilityRecordDto, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new CreateAvailabilityRecordCommand(User,
                availabilityRecordDto.Url,
                availabilityRecordDto.ExpectedStatusCode,
                availabilityRecordDto.ExpectedResponse,
                availabilityRecordDto.LogLifetimeThresholdInHours), cancellationToken).ConfigureAwait(false);

            return NoContent();
        }
        
        [HttpGet("List")]
        [ProducesResponseType(typeof(List<AvailabilityRecordDto>), 200)]
        public async Task<IActionResult> GetAvailabilityRecords(CancellationToken cancellationToken)
        {
            var recordDtos = await _mediator.Send(new GetAvailabilityRecordsCommand(User), cancellationToken)
                .ConfigureAwait(false);
            
            return Ok(recordDtos);
        }
    }
}