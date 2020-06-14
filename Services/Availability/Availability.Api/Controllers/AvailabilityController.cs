using System;
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
                availabilityRecordDto.Name,
                availabilityRecordDto.Url,
                availabilityRecordDto.ExpectedStatusCode,
                availabilityRecordDto.ExpectedResponse,
                availabilityRecordDto.LogLifetimeThresholdInHours), cancellationToken).ConfigureAwait(false);

            return NoContent();
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(List<AvailabilityListItemDto>), 200)]
        public async Task<IActionResult> GetAvailabilityListItems(CancellationToken cancellationToken)
        {
            var recordDtos = await _mediator.Send(new GetAvailabilityListItemsCommand(User), cancellationToken)
                .ConfigureAwait(false);
            
            return Ok(recordDtos);
        }
        
        [HttpPost("List")]
        [ProducesResponseType(typeof(List<AvailabilityListItemDto>), 200)]
        public async Task<IActionResult> GetAvailabilityListItemsByIds(List<Guid> ids, CancellationToken cancellationToken)
        {
            var recordDtos = await _mediator
                .Send(new GetAvailabilityListItemsByIdsCommand(ids), cancellationToken)
                .ConfigureAwait(false);
            
            return Ok(recordDtos);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AvailabilityRecordDto), 200)]
        public async Task<IActionResult> GetAvailabilityRecordById(Guid id, CancellationToken cancellationToken)
        {
            var record = await _mediator
                .Send(new GetAvailabilityRecordByIdCommand(id), cancellationToken)
                .ConfigureAwait(false);
            
            return Ok(record);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAvailabilityRecord(Guid id, CancellationToken cancellationToken)
        {
            await _mediator
                .Publish(new DeleteAvailabilityRecordCommand(id, User), cancellationToken)
                .ConfigureAwait(false);

            return NoContent();
        }
    }
}