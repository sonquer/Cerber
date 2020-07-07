using System;
using Availability.Domain.SeedWork;

namespace Availability.Domain.AggregateModels.AvailabilityRecordAggregate
{
    public class AvailabilityLog : Entity
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
            Id = Guid.NewGuid();
            CreatedAt = createdAt;
            StatusCode = statusCode;
            Body = body?.Trim();
            ResponseTime = responseTime;
        }

        protected AvailabilityLog()
        {
        }
    }
}