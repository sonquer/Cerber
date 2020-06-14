using System;
using Availability.Api.Application.Dtos;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class GetAvailabilityRecordByIdCommand : IRequest<AvailabilityRecordDto>
    {
        public Guid Id { get; set; }

        public GetAvailabilityRecordByIdCommand(Guid id)
        {
            Id = id;
        }
    }
}