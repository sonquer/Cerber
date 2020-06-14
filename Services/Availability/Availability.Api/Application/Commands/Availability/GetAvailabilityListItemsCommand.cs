using System.Collections.Generic;
using System.Security.Claims;
using Availability.Api.Application.Dtos;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class GetAvailabilityListItemsCommand : IRequest<List<AvailabilityListItemDto>>
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public GetAvailabilityListItemsCommand(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsPrincipal = claimsPrincipal;
        }
    }
}