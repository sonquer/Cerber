using System;

namespace Availability.Api.Application.Dtos
{
    public class AvailabilityListItemDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Url { get; set; }
        
        public string Status { get; set; }
    }
}