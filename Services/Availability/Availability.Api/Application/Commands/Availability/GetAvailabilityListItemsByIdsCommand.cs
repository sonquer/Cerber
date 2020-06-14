using System;
using System.Collections.Generic;
using Availability.Api.Application.Dtos;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class GetAvailabilityListItemsByIdsCommand : IRequest<List<AvailabilityListItemDto>>
    {
        public List<Guid> Ids { get; set; }

        public GetAvailabilityListItemsByIdsCommand(List<Guid> ids)
        {
            Ids = ids;
        }
    }
}