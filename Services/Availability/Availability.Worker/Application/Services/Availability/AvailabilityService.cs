using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Availability.Worker.Application.Services.Availability.Models;

namespace Availability.Worker.Application.Services.Availability
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IHttpClientFactory _clientFactory;

        public AvailabilityService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        public async Task<AvailabilityResponseModel> Request(string url, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Cerber-AvailabilityService");

            var client = _clientFactory.CreateClient();

            var sw = Stopwatch.StartNew();
            var response = await client.SendAsync(request, cancellationToken);
            sw.Stop();

            var responseString = await response.Content.ReadAsStringAsync();

            return new AvailabilityResponseModel(response.StatusCode, sw.ElapsedMilliseconds, responseString);
        }
    }
}