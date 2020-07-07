namespace Availability.Api.Application.Dtos
{
    public class CreateAvailabilityRecordDto
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public int ExpectedStatusCode { get; set; }

        public string ExpectedResponse { get; set; }

        public int LogLifetimeThresholdInHours { get; set; }
    }
}
