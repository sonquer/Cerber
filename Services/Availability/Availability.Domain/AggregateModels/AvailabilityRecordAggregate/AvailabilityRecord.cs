using System;
using System.Collections.Generic;
using System.Linq;
using Availability.Domain.SeedWork;

namespace Availability.Domain.AggregateModels.AvailabilityRecordAggregate
{
    public class AvailabilityRecord : Entity, IAggregateRoot
    {
        public Guid AccountId { get; protected set; }
        
        public string Name { get; protected set; }

        public string Url { get; protected set; }
        
        public int ExpectedStatusCode { get; protected set; }
        
        public string ExpectedResponse { get; protected set; }

        public bool HasExpectedResponse => string.IsNullOrWhiteSpace(ExpectedResponse) == false;
        
        public List<AvailabilityLog> AvailabilityLogs { get; protected set; }
        
        public int LogLifetimeThresholdInHours { get; protected set; }

        public string Status => GetStatus();

        public AvailabilityRecord(Guid accountId,
            string name,
            string url,
            int expectedStatusCode,
            string expectedResponse,
            int logLifetimeThresholdInHours)
        {
            PartitionKey = Guid.NewGuid();
            Id = Guid.NewGuid();
            AccountId = accountId;
            Name = name;
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
            var availabilityLog = new AvailabilityLog(DateTime.UtcNow, statusCode, body, responseTime);
            
            AvailabilityLogs.Add(availabilityLog);
            
            return availabilityLog;
        }

        public void ClearOutdatedLogs()
        {
            AvailabilityLogs.RemoveAll(e => DateTime.UtcNow >= e.CreatedAt.AddHours(LogLifetimeThresholdInHours));
        }

        private string GetStatus()
        {
            if (AvailabilityLogs.Any() == false)
            {
                return "ST_OK";
            }

            var newestRecord = AvailabilityLogs
                .OrderByDescending(e => e.CreatedAt)
                .First();

            if (newestRecord.Body != ExpectedResponse || newestRecord.StatusCode != ExpectedStatusCode)
            {
                return "ST_ERROR";
            }

            return "ST_OK";
        }
    }
}
