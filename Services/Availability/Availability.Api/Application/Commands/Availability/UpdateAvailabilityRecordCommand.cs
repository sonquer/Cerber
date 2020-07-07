using System;
using System.Security.Claims;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class UpdateAvailabilityRecordCommand : INotification
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int ExpectedStatusCode { get; set; }

        public string ExpectedResponse { get; set; }

        public int LogLifetimeThresholdInHours { get; set; }

        public UpdateAvailabilityRecordCommand(ClaimsPrincipal claimsPrincipal,
            Guid id,
            string name,
            string url,
            int expectedStatusCode,
            string expectedResponse,
            int logLifetimeThresholdInHours)
        {
            Id = id;
            ClaimsPrincipal = claimsPrincipal;
            Name = name;
            Url = url;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponse = expectedResponse;
            LogLifetimeThresholdInHours = logLifetimeThresholdInHours;
        }
    }
}
