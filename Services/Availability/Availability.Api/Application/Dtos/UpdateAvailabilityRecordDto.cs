using System;

namespace Availability.Api.Application.Dtos
{
    public class UpdateAvailabilityRecordDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int ExpectedStatusCode { get; set; }

        public string ExpectedResponse { get; set; }

        public int LogLifetimeThresholdInHours { get; set; }
    }
}
