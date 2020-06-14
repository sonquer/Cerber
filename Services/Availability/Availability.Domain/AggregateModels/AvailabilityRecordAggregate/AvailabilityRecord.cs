using System;
using System.Collections.Generic;
using Availability.Domain.SeedWork;

namespace Availability.Domain.AggregateModels.AvailabilityRecordAggregate
{
    public class AvailabilityRecord : Entity, IAggregateRoot
    {
        public Guid AccountId { get; protected set; }

        public string Url { get; protected set; }
        
        public int ExpectedStatusCode { get; protected set; }
        
        public string ExpectedResponse { get; protected set; }

        public bool HasExpectedResponse => string.IsNullOrWhiteSpace(ExpectedResponse) == false;
        
        public List<AvailabilityLog> AvailabilityLogs { get; protected set; }
        
        public int LogLifetimeThresholdInHours { get; protected set; }

        public AvailabilityRecord(Guid accountId,
            string url,
            int expectedStatusCode,
            string expectedResponse,
            int logLifetimeThresholdInHours)
        {
            PartitionKey = Guid.NewGuid();
            Id = Guid.NewGuid();
            AccountId = accountId;
            Url = url;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponse = expectedResponse;
            AvailabilityLogs = new List<AvailabilityLog>();
            LogLifetimeThresholdInHours = logLifetimeThresholdInHours;
        }

        protected AvailabilityRecord()
        {
        }

        public AvailabilityLog AppendLog(int statusCode, string body, long responseTime)
        {
            if (AvailabilityLogs is null)
            {
                AvailabilityLogs = new List<AvailabilityLog>();
            }
            
            var availabilityLog = new AvailabilityLog(DateTime.UtcNow, statusCode, body, responseTime);
            
            AvailabilityLogs.Add(availabilityLog);
            
            return availabilityLog;
        }

        public void ClearOutdatedLogs()
        {
            AvailabilityLogs.RemoveAll(e => DateTime.UtcNow >= e.CreatedAt.AddHours(LogLifetimeThresholdInHours));
        }
    }
}
