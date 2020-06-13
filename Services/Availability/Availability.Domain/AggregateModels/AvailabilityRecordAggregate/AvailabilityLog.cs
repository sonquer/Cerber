using System;

namespace Availability.Domain.AggregateModels.AvailabilityRecordAggregate
{
    public class AvailabilityLog
    {
        public DateTime CreatedAt { get; set; }
        
        public int StatusCode { get; set; }
        
        public string Body { get; set; }
        
        public long ResponseTime { get; set; }

        public AvailabilityLog(DateTime createdAt,
            int statusCode, 
            string body, 
            long responseTime)
        {
            CreatedAt = createdAt;
            StatusCode = statusCode;
            Body = body;
            ResponseTime = responseTime;
        }
    }
}