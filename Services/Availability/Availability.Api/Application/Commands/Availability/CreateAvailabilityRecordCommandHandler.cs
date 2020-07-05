using System.Threading;
using System.Threading.Tasks;
using Availability.Api.Application.Claims;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class CreateAvailabilityRecordCommandHandler : INotificationHandler<CreateAvailabilityRecordCommand>
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        private readonly IClaimConverter _claimConverter;

        public CreateAvailabilityRecordCommandHandler(IAvailabilityRecordRepository availabilityRecordRepository,
            IClaimConverter claimConverter)
        {
            _availabilityRecordRepository = availabilityRecordRepository;
            _claimConverter = claimConverter;
        }
        
        public async Task Handle(CreateAvailabilityRecordCommand notification, CancellationToken cancellationToken)
        {
            var accountId = _claimConverter.GetAccountId(notification.ClaimsPrincipal);
            
            var availabilityRecord = new AvailabilityRecord(accountId, 
                notification.Name,
                notification.Url, 
                notification.ExpectedStatusCode, 
                notification.ExpectedResponse,
                notification.LogLifetimeThresholdInHours);

            await _availabilityRecordRepository.AddAsync(availabilityRecord, cancellationToken)
                .ConfigureAwait(false);

            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}