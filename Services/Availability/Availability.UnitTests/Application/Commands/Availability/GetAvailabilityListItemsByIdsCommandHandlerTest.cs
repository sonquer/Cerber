using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetAvailabilityListItemsByIdsCommandHandlerTest
    {
        [Fact]
        public async Task GetAvailabilityListItemsByIdsCommandHandler_Handle_AvailabilityListItemsReturned()
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
            
            var getAvailabilityListItemsByIdsCommandHandler = new GetAvailabilityListItemsByIdsCommandHandler(availabilityRecordRepository.Object,
                mapper);

            var result = await getAvailabilityListItemsByIdsCommandHandler.Handle(new GetAvailabilityListItemsByIdsCommand(new List<Guid>
                {
                    Guid.NewGuid()
                }),
                CancellationToken.None);
            
            Assert.Equal("test", result.First().Name);
            Assert.Equal("http://google.com/", result.First().Url);
            Assert.Equal("ST_OK", result.First().Status);
        }
    }
}