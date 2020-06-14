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
    public class CreateAvailabilityRecordCommandHandlerTest
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;

        private readonly IClaimConverter _claimConverter;
        
        public CreateAvailabilityRecordCommandHandlerTest()
        {
            var options = new DbContextOptionsBuilder<AvailabilityContext>()
                .UseInMemoryDatabase(nameof(CreateAvailabilityRecordCommandHandlerTest))
                .Options;
            
            var availabilityContext = new AvailabilityContext(options);
            
            _availabilityRecordRepository = new AvailabilityRecordRepository(availabilityContext);
            
            _claimConverter = new UserClaimConverter();
        }
        
        [Fact]
        public async Task CreateAvailabilityRecordCommandHandler_Handle_AvailabilityRecordCreated()
        {
            var accountId = Guid.NewGuid();
            
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountId.ToString()));
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsIdentity);
            
            var createAvailabilityRecordCommandHandler = new CreateAvailabilityRecordCommandHandler(_availabilityRecordRepository,
                _claimConverter);

            await createAvailabilityRecordCommandHandler.Handle(new CreateAvailabilityRecordCommand(claimsPrincipal,
                "test name",
                "http://google.com/",
                200,
                "{}",
                12), CancellationToken.None).ConfigureAwait(false);

            var records = await _availabilityRecordRepository.GetByAccountId(accountId, CancellationToken.None)
                .ConfigureAwait(false);

            Assert.Single(records);
            Assert.Equal(accountId, records.First().AccountId);
            Assert.Equal("test name", records.First().Name);
            Assert.Equal("http://google.com/", records.First().Url);
            Assert.Equal(200, records.First().ExpectedStatusCode);
            Assert.Equal("{}", records.First().ExpectedResponse);
            Assert.Equal(12, records.First().LogLifetimeThresholdInHours);
        }
    }
}