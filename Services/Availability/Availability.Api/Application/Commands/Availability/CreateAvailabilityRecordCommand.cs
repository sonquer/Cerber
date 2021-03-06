using System.Security.Claims;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class CreateAvailabilityRecordCommand : INotification
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        
        public string Name { get; set; }

        public string Url { get; set; }
        
        public int ExpectedStatusCode { get; set; }
        
        public string ExpectedResponse { get; set; }
        
        public int LogLifetimeThresholdInHours { get; set; }

        public CreateAvailabilityRecordCommand(ClaimsPrincipal claimsPrincipal, 
            string name,
            string url, 
            int expectedStatusCode, 
            string expectedResponse, 
            int logLifetimeThresholdInHours)
        {
            ClaimsPrincipal = claimsPrincipal;
            Name = name;
            Url = url;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponse = expectedResponse;
            LogLifetimeThresholdInHours = logLifetimeThresholdInHours;
        }
    }
}