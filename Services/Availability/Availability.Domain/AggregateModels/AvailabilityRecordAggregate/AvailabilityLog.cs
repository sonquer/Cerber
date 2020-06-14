using System;

namespace Availability.Domain.AggregateModels.AvailabilityRecordAggregate
{
    public class AvailabilityLog
    {
        public DateTime CreatedAt { get; protected set; }
        
        public int StatusCode { get; protected set; }
        
        public string Body { get; protected set; }
        
        public long ResponseTime { get; protected set; }

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

        protected AvailabilityLog()
        {
        }
    }
}