using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Infrastructure;
using Availability.Infrastructure.Repositories;
using Availability.Worker.Application.Processors;
using Availability.Worker.Application.Services.Availability;
using Availability.Worker.Application.Services.Availability.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Availability.Worker.UnitTests.Application.Processors
{
    public class AvailabilityProcessorTest
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        public AvailabilityProcessorTest()
        {
            var options = new DbContextOptionsBuilder<AvailabilityContext>()
                .UseInMemoryDatabase(nameof(AvailabilityProcessorTest))
                .Options;
            
            var availabilityContext = new AvailabilityContext(options);
            
            _availabilityRecordRepository = new AvailabilityRecordRepository(availabilityContext);
        }
        
        [Fact]
        public async Task AvailabilityProcessor_ProcessAvailabilityRecord_RecordProcessed()
        {
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "http://google.com/", 
                200, 
                null, 
                2);

            await _availabilityRecordRepository.AddAsync(availabilityRecord, CancellationToken.None);
            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);
            
            var availabilityService = new Mock<IAvailabilityService>();
            availabilityService.Setup(e => e.Request(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AvailabilityResponseModel(HttpStatusCode.OK, 65, "{}"));
            
            var availabilityProcessor = new AvailabilityProcessor(_availabilityRecordRepository, 
                availabilityService.Object);

            await availabilityProcessor.ProcessAvailabilityRecord(availabilityRecord.Id, CancellationToken.None);

            availabilityRecord = await _availabilityRecordRepository.GetById(availabilityRecord.Id, CancellationToken.None);

            Assert.Single(availabilityRecord.AvailabilityLogs);
            Assert.Equal(200, availabilityRecord.AvailabilityLogs.First().StatusCode);
            Assert.Equal("{}", availabilityRecord.AvailabilityLogs.First().Body);
            Assert.Equal(65, availabilityRecord.AvailabilityLogs.First().ResponseTime);
        }
    }
}