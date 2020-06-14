using System;
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
    public class DeleteAvailabilityRecordCommandHandlerTest
    {
        private readonly AvailabilityContext _availabilityContext;
        
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        private readonly IClaimConverter _claimConverter;

        public DeleteAvailabilityRecordCommandHandlerTest()
        {
            var options = new DbContextOptionsBuilder<AvailabilityContext>()
                .UseInMemoryDatabase(nameof(DeleteAvailabilityRecordCommandHandlerTest))
                .Options;
            
            _availabilityContext = new AvailabilityContext(options);
            
            _availabilityRecordRepository = new AvailabilityRecordRepository(_availabilityContext);
            
            _claimConverter = new UserClaimConverter();
        }
        
        [Fact]
        public async Task DeleteAvailabilityRecordCommandHandler_Handle_AvailabilityRecordDeleted()
        {
            var accountId = Guid.NewGuid();
            var availabilityRecord = new AvailabilityRecord(accountId, 
                "test",
                "http://google.com/",
                200,
                "{}",
                2);

            await _availabilityRecordRepository.AddAsync(availabilityRecord, CancellationToken.None);
            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);
            
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountId.ToString()));
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsIdentity);

            var deleteAvailabilityRecordCommandHandler = new DeleteAvailabilityRecordCommandHandler(_availabilityRecordRepository, 
                _claimConverter);

            await deleteAvailabilityRecordCommandHandler.Handle(new DeleteAvailabilityRecordCommand(availabilityRecord.Id, claimsPrincipal),
                CancellationToken.None).ConfigureAwait(false);
            
            Assert.Empty(await _availabilityContext.AvailabilityRecords.ToListAsync(CancellationToken.None));
        }
        
        [Fact]
        public async Task DeleteAvailabilityRecordCommandHandler_Handle_AvailabilityRecordDeleteIsNotAllowedFail()
        {
            var accountId = Guid.NewGuid();
            var availabilityRecord = new AvailabilityRecord(accountId, 
                "test",
                "http://google.com/",
                200,
                "{}",
                2);

            await _availabilityRecordRepository.AddAsync(availabilityRecord, CancellationToken.None);
            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);
            
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()));
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsIdentity);

            var deleteAvailabilityRecordCommandHandler = new DeleteAvailabilityRecordCommandHandler(_availabilityRecordRepository, 
                _claimConverter);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await deleteAvailabilityRecordCommandHandler
                    .Handle(new DeleteAvailabilityRecordCommand(availabilityRecord.Id, claimsPrincipal),
                    CancellationToken.None).ConfigureAwait(false);
            });
        }
    }
}