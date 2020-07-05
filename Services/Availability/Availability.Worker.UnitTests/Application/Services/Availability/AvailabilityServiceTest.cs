using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Availability.Worker.Application.Services.Availability;
using Moq;
using Moq.Protected;
using Xunit;

namespace Availability.Worker.UnitTests.Application.Services.Availability
{
    public class AvailabilityServiceTest
    {
        [Fact]
        public async Task AvailabilityService_Request_RequestExecuted()
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'health':'Ok'}")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            var availabilityService = new AvailabilityService(mockFactory.Object);

            var response = await availabilityService.Request("http://google.com/", CancellationToken.None)
                .ConfigureAwait(false);
            
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
            Assert.Equal("{'health':'Ok'}",  response.Body);
        }
    }
}