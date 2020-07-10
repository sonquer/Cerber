using System;
using System.Diagnostics;
using System.Net;
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
            return await GetValueWithTimeout(url, 10_000);
        }

        private async Task<AvailabilityResponseModel> GetValueWithTimeout(string url, int milliseconds)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.CancelAfter(milliseconds);
            token.ThrowIfCancellationRequested();

            var workerTask = Task.Run(async () =>
            {
                var sw = Stopwatch.StartNew();

                try {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("User-Agent", "Cerber-AvailabilityService");

                    var client = _clientFactory.CreateClient();

                    var response = await client.SendAsync(request, token);
                    sw.Stop();

                    var responseString = await response.Content.ReadAsStringAsync();

                    return new AvailabilityResponseModel(response.StatusCode, sw.ElapsedMilliseconds, responseString);
                }
                catch {
                    sw.Stop();
                    return new AvailabilityResponseModel(HttpStatusCode.InternalServerError, sw.ElapsedMilliseconds, null);
                }
            }, token);

            try
            {
                return await workerTask;
            }
            catch (OperationCanceledException)
            {
                return new AvailabilityResponseModel(HttpStatusCode.RequestTimeout, milliseconds, null);
            }
        }
    }
}
