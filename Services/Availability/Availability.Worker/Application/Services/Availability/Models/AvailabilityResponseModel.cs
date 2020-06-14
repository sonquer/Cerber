using System.Net;

namespace Availability.Worker.Application.Services.Availability.Models
{
    public class AvailabilityResponseModel
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        
        public long ResponseTime { get; set; }
        
        public string Body { get; set; }

        public AvailabilityResponseModel(HttpStatusCode httpStatusCode, 
            long responseTime,
            string body)
        {
            ResponseTime = responseTime;
            HttpStatusCode = httpStatusCode;
            Body = body;
        }
    }
}