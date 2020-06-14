using System.Collections.Generic;
using System.Security.Claims;
using Availability.Api.Application.Dtos;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class GetAvailabilityRecordsCommand : IRequest<List<AvailabilityRecordDto>>
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public GetAvailabilityRecordsCommand(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsPrincipal = claimsPrincipal;
        }
    }
}