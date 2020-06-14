using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Infrastructure;
using Availability.Infrastructure.Repositories;
using Availability.Worker.Application.Commands;
using Availability.Worker.Application.Processors;
using Availability.Worker.Application.Services.Availability;
using Availability.Worker.Application.Services.Availability.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Availability.Worker.UnitTests.Application.Commands
{
    public class RequestServicesCommandHandlerTest
    {
        private readonly AvailabilityContext _availabilityContext;
        
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        public RequestServicesCommandHandlerTest()
        {
            var options = new DbContextOptionsBuilder<AvailabilityContext>()
                .UseInMemoryDatabase(nameof(RequestServicesCommandHandlerTest))
                .Options;
            
            _availabilityContext = new AvailabilityContext(options);
            _availabilityRecordRepository = new AvailabilityRecordRepository(_availabilityContext);
        }
        
        [Fact]
        public async Task RequestServicesCommandHandler_Handle_RecordsProcessed()
        {
            var availabilityService = new Mock<IAvailabilityService>();
            availabilityService.Setup(e => e.Request(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AvailabilityResponseModel(HttpStatusCode.OK, 22, "{}"));
            
            var availabilityProcessor = new AvailabilityProcessor(_availabilityRecordRepository,
                availabilityService.Object);

            var requestServicesCommandHandler = new RequestServicesCommandHandler(_availabilityContext, 
                availabilityProcessor);
            
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "http://google.com", 
                200, 
                null, 
                2);

            await _availabilityRecordRepository.AddAsync(availabilityRecord, CancellationToken.None);
            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);
            
            await requestServicesCommandHandler.Handle(new RequestServicesCommand(), CancellationToken.None);

            availabilityRecord = await _availabilityRecordRepository
                .GetById(availabilityRecord.Id, CancellationToken.None)
                .ConfigureAwait(false);

            Assert.Single(availabilityRecord.AvailabilityLogs);
            Assert.Equal(200, availabilityRecord.AvailabilityLogs.First().StatusCode);
            Assert.Equal(22, availabilityRecord.AvailabilityLogs.First().ResponseTime);
            Assert.Equal("{}", availabilityRecord.AvailabilityLogs.First().Body);
        }
    }
}