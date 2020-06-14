using System;
using System.Collections.Generic;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;

namespace Availability.Api.Application.Dtos
{
    public class AvailabilityRecordDto
    {
        public Guid Id { get; set; }
        
        public string Url { get; set; }
        
        public int ExpectedStatusCode { get; set; }
        
        public string ExpectedResponse { get; set; }
        
        public List<AvailabilityLog> AvailabilityLogs { get; set; }
        
        public int LogLifetimeThresholdInHours { get; set; }
    }
}