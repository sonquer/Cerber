using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Availability.Api.Application.Commands.Availability;
using Availability.Api.Application.Dtos;
using Availability.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Availability.UnitTests.Application.Controllers
{
    public class AvailabilityControllerTest
    {
        [Fact]
        public async Task AvailabilityController_CreateAvailabilityRecord_AvailabilityRecordCreated()
        {
            var mediator = new Mock<IMediator>();
            var availabilityController = new AvailabilityController(mediator.Object);

            var response = await availabilityController
                .CreateAvailabilityRecord(new AvailabilityRecordDto(), CancellationToken.None)
                .ConfigureAwait(false);

            Assert.IsAssignableFrom<NoContentResult>(response);
        }
        
        [Fact]
        public async Task AvailabilityController_GetAvailabilityRecords_AvailabilityRecordReturned()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(e => e.Send(It.IsAny<GetAvailabilityRecordsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AvailabilityRecordDto>
                {
                    new AvailabilityRecordDto
                    {
                        Url = "http://google.com/",
                        ExpectedResponse = "{}",
                        ExpectedStatusCode = 200,
                        LogLifetimeThresholdInHours = 2
                    }
                });
            
            var availabilityController = new AvailabilityController(mediator.Object);

            var response = await availabilityController
                .GetAvailabilityRecords(CancellationToken.None)
                .ConfigureAwait(false);

            var objectResult =  Assert.IsAssignableFrom<OkObjectResult>(response);
            var recordDtos = (List<AvailabilityRecordDto>) objectResult.Value;
            
            Assert.Equal("http://google.com/", recordDtos.First().Url);
            Assert.Equal("{}", recordDtos.First().ExpectedResponse);
            Assert.Equal(200, recordDtos.First().ExpectedStatusCode);
            Assert.Equal(2, recordDtos.First().LogLifetimeThresholdInHours);
        }
    }
}