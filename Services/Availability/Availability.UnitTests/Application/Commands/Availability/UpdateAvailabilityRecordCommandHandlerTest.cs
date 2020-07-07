using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Availability.Api.Application.Claims;
using Availability.Api.Application.Commands.Availability;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Infrastructure;
using Availability.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Availability.UnitTests.Application.Commands.Availability
{
    public class UpdateAvailabilityRecordCommandHandlerTest
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;

        private readonly IClaimConverter _claimConverter;
        
        public UpdateAvailabilityRecordCommandHandlerTest()
        {
            var options = new DbContextOptionsBuilder<AvailabilityContext>()
                .UseInMemoryDatabase(nameof(UpdateAvailabilityRecordCommandHandlerTest))
                .Options;
            
            var availabilityContext = new AvailabilityContext(options);
            
            _availabilityRecordRepository = new AvailabilityRecordRepository(availabilityContext);
            
            _claimConverter = new UserClaimConverter();
        }
        
        [Fact]
        public async Task UpdateAvailabilityRecordCommandHandler_Handle_AvailabilityRecordUpdated()
        {
            var accountId = Guid.NewGuid();
            
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountId.ToString()));
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsIdentity);
            
            var createAvailabilityRecordCommandHandler = new UpdateAvailabilityRecordCommandHandler(_availabilityRecordRepository,
                _claimConverter);

            var record = new AvailabilityRecord(accountId,
                "name",
                "url",
                200,
                "{}",
                1);

            await _availabilityRecordRepository.AddAsync(record, 
                CancellationToken.None);

            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);

            await createAvailabilityRecordCommandHandler.Handle(new UpdateAvailabilityRecordCommand(claimsPrincipal,
                record.Id,
                "test name",
                "http://google.com/",
                200,
                "{}",
                12), CancellationToken.None).ConfigureAwait(false);
            
            Assert.Equal(accountId, record.AccountId);
            Assert.Equal("test name", record.Name);
            Assert.Equal("http://google.com/", record.Url);
            Assert.Equal(200, record.ExpectedStatusCode);
            Assert.Equal("{}", record.ExpectedResponse);
            Assert.Equal(12, record.LogLifetimeThresholdInHours);
        } 
        
        [Fact]
        public async Task UpdateAvailabilityRecordCommandHandler_Handle_DifferentAccountIdFail()
        {
            var accountId = Guid.NewGuid();
            
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountId.ToString()));
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsIdentity);
            
            var createAvailabilityRecordCommandHandler = new UpdateAvailabilityRecordCommandHandler(_availabilityRecordRepository,
                _claimConverter);

            var record = new AvailabilityRecord(Guid.NewGuid(),
                "name",
                "url",
                200,
                "{}",
                1);

            await _availabilityRecordRepository.AddAsync(record, 
                CancellationToken.None);

            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => { 
                await createAvailabilityRecordCommandHandler.Handle(new UpdateAvailabilityRecordCommand(claimsPrincipal,
                    record.Id,
                    "test name",
                    "http://google.com/",
                    200,
                    "{}",
                    12), CancellationToken.None).ConfigureAwait(false);
            });
        } 
    }
}