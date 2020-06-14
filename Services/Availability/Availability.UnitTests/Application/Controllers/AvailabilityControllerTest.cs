using System;
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
        public async Task AvailabilityController_GetAvailabilityListItems_AvailabilityListItemsReturned()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(e => e.Send(It.IsAny<GetAvailabilityListItemsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AvailabilityListItemDto>
                {
                    new AvailabilityListItemDto
                    {
                        Name = "test name",
                        Url = "http://google.com/",
                        Status = "ST_OK"
                    }
                });
            
            var availabilityController = new AvailabilityController(mediator.Object);

            var response = await availabilityController
                .GetAvailabilityListItems(CancellationToken.None)
                .ConfigureAwait(false);

            var objectResult =  Assert.IsAssignableFrom<OkObjectResult>(response);
            var recordDtos = (List<AvailabilityListItemDto>) objectResult.Value;
            
            Assert.Equal("http://google.com/", recordDtos.First().Url);
            Assert.Equal("test name", recordDtos.First().Name);
            Assert.Equal("ST_OK", recordDtos.First().Status);
        }
        
        [Fact]
        public async Task AvailabilityController_GetAvailabilityListItemsByIds_AvailabilityListItemsReturned()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(e => e.Send(It.IsAny<GetAvailabilityListItemsByIdsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AvailabilityListItemDto>
                {
                    new AvailabilityListItemDto
                    {
                        Name = "test name",
                        Url = "http://google.com/",
                        Status = "ST_OK"
                    }
                });
            
            var availabilityController = new AvailabilityController(mediator.Object);

            var response = await availabilityController
                .GetAvailabilityListItemsByIds(new List<Guid> { Guid.Empty }, CancellationToken.None)
                .ConfigureAwait(false);

            var objectResult =  Assert.IsAssignableFrom<OkObjectResult>(response);
            var recordDtos = (List<AvailabilityListItemDto>) objectResult.Value;
            
            Assert.Equal("http://google.com/", recordDtos.First().Url);
            Assert.Equal("test name", recordDtos.First().Name);
            Assert.Equal("ST_OK", recordDtos.First().Status);
        }
        
        [Fact]
        public async Task AvailabilityController_GetAvailabilityRecordById_AvailabilityRecordReturned()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(e => e.Send(It.IsAny<GetAvailabilityRecordByIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AvailabilityRecordDto
                    {
                        Id = Guid.Empty,
                        Name = "test name",
                        Url = "http://google.com/",
                        ExpectedResponse = "{}",
                        ExpectedStatusCode = 200,
                        Status = "ST_OK",
                        LogLifetimeThresholdInHours = 2,
                        AvailabilityLogs = new List<AvailabilityLogDto>
                        {
                            new AvailabilityLogDto
                            {
                                ResponseTime = 25,
                                StatusCode = 200,
                                Body = "{}",
                                CreatedAt = DateTime.MinValue
                            }
                        }
                    });
            
            var availabilityController = new AvailabilityController(mediator.Object);

            var response = await availabilityController
                .GetAvailabilityRecordById(Guid.Empty, CancellationToken.None)
                .ConfigureAwait(false);

            var objectResult =  Assert.IsAssignableFrom<OkObjectResult>(response);
            var recordDto = (AvailabilityRecordDto) objectResult.Value;
            
            Assert.Equal(Guid.Empty, recordDto.Id);
            Assert.Equal("http://google.com/", recordDto.Url);
            Assert.Equal("test name", recordDto.Name);
            Assert.Equal("ST_OK", recordDto.Status);
            Assert.Equal("{}", recordDto.ExpectedResponse);
            Assert.Equal(200, recordDto.ExpectedStatusCode);
            Assert.Equal(2, recordDto.LogLifetimeThresholdInHours);
            Assert.Single(recordDto.AvailabilityLogs);
            Assert.Equal(25, recordDto.AvailabilityLogs.First().ResponseTime);
            Assert.Equal(200, recordDto.AvailabilityLogs.First().StatusCode);
            Assert.Equal("{}", recordDto.AvailabilityLogs.First().Body);
            Assert.Equal(DateTime.MinValue, recordDto.AvailabilityLogs.First().CreatedAt);
        }
    }
}