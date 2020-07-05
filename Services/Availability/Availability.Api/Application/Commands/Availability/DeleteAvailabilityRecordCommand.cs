using System;
using System.Security.Claims;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class DeleteAvailabilityRecordCommand : INotification
    {
        public Guid Id { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public DeleteAvailabilityRecordCommand(Guid id, ClaimsPrincipal claimsPrincipal)
        {
            Id = id;
            ClaimsPrincipal = claimsPrincipal;
        }
    }
}