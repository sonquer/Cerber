using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Availability.Api.Application.Commands.Availability;
using Availability.Api.Application.Mappings;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Moq;
using Xunit;

namespace Availability.UnitTests.Application.Commands.Availability
{
    public class GetAvailabilityRecordByIdCommandHandlerTest
    {
        [Fact]
        public async Task GetAvailabilityRecordByIdCommandHandler_Handle_AvailabilityRecordReturned()
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<DefaultDomainMapping>();
            });
            
            var mapper = configuration.CreateMapper();
            
            var availabilityRecordRepository = new Mock<IAvailabilityRecordRepository>();
            availabilityRecordRepository.Setup(e => e.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AvailabilityRecord(Guid.Empty, 
                    "test", 
                    "http://google.com/", 
                    200, 
                    "{}", 
                    2));
            
            var getAvailabilityRecordByIdCommandHandler = new GetAvailabilityRecordByIdCommandHandler(availabilityRecordRepository.Object,
                mapper);

            var result = await getAvailabilityRecordByIdCommandHandler.Handle(new GetAvailabilityRecordByIdCommand(Guid.Empty),
                CancellationToken.None);
            
            Assert.Equal("test", result.Name);
            Assert.Equal("http://google.com/", result.Url);
            Assert.Equal(200, result.ExpectedStatusCode);
            Assert.Equal("{}", result.ExpectedResponse);
            Assert.Equal(2, result.LogLifetimeThresholdInHours);
            Assert.Equal("ST_OK", result.Status);
        }
    }
}