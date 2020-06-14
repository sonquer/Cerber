using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Availability.Api.Application.Claims;
using Availability.Api.Application.Commands.Availability;
using Availability.Api.Application.Mappings;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Infrastructure;
using Availability.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Availability.UnitTests.Application.Commands.Availability
{
    public class GetAvailabilityRecordsCommandHandlerTest
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;

        private readonly IClaimConverter _claimConverter;
        
        public GetAvailabilityRecordsCommandHandlerTest()
        {
            var options = new DbContextOptionsBuilder<AvailabilityContext>()
                .UseInMemoryDatabase(nameof(GetAvailabilityRecordsCommandHandlerTest))
                .Options;
            
            var availabilityContext = new AvailabilityContext(options);
            
            _availabilityRecordRepository = new AvailabilityRecordRepository(availabilityContext);
            
            _claimConverter = new UserClaimConverter();
        }
        
        [Fact]
        public async Task GetAvailabilityRecordsCommandHandler_Handle_AvailabilityRecordsReturned()
        {
            var accountId = Guid.NewGuid();

            var availabilityRecord = new AvailabilityRecord(accountId, 
                "http://google.com/", 
                200, 
                null, 
                2);
            
            availabilityRecord.AppendLog(200, "{}", 66);

            await _availabilityRecordRepository.AddAsync(availabilityRecord, CancellationToken.None)
                .ConfigureAwait(false);

            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);
            
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountId.ToString()));
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsIdentity);

            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<DefaultDomainMapping>();
            });
            
            var mapper = configuration.CreateMapper();

            var getAvailabilityRecordsCommandHandler = new GetAvailabilityRecordsCommandHandler(_availabilityRecordRepository, 
                _claimConverter, 
                mapper);

            var availabilityRecordDtos = await getAvailabilityRecordsCommandHandler
                .Handle(new GetAvailabilityRecordsCommand(claimsPrincipal), CancellationToken.None)
                .ConfigureAwait(false);
            
            Assert.Equal(availabilityRecord.Id, availabilityRecordDtos.First().Id);
            Assert.Equal("http://google.com/", availabilityRecordDtos.First().Url);
            Assert.Equal(200, availabilityRecordDtos.First().ExpectedStatusCode);
            Assert.Null(availabilityRecordDtos.First().ExpectedResponse);
            Assert.Equal(2, availabilityRecordDtos.First().LogLifetimeThresholdInHours);
            Assert.Equal("{}", availabilityRecordDtos.First().AvailabilityLogs.First().Body);
            Assert.Equal(66, availabilityRecordDtos.First().AvailabilityLogs.First().ResponseTime);
            Assert.Equal(200, availabilityRecordDtos.First().AvailabilityLogs.First().StatusCode);
        }
    }
}