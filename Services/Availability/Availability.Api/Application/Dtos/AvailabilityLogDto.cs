using System;

namespace Availability.Api.Application.Dtos
{
    public class AvailabilityLogDto
    {
        public DateTime CreatedAt { get; set; }
        
        public int StatusCode { get; set; }
        
        public string Body { get; set; }
        
        public long ResponseTime { get; set; }
    }
}