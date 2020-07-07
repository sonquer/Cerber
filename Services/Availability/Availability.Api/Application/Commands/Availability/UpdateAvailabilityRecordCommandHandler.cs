using System;
using System.Threading;
using System.Threading.Tasks;
using Availability.Api.Application.Claims;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class UpdateAvailabilityRecordCommandHandler : INotificationHandler<UpdateAvailabilityRecordCommand>
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;

        private readonly IClaimConverter _claimConverter;

        public UpdateAvailabilityRecordCommandHandler(IAvailabilityRecordRepository availabilityRecordRepository,
            IClaimConverter claimConverter)
        {
            _availabilityRecordRepository = availabilityRecordRepository;
            _claimConverter = claimConverter;
        }

        public async Task Handle(UpdateAvailabilityRecordCommand notification, CancellationToken cancellationToken)
        {
            var accountId = _claimConverter.GetAccountId(notification.ClaimsPrincipal);

            var availabilityRecord = await _availabilityRecordRepository.GetById(notification.Id, cancellationToken)
                .ConfigureAwait(false);

            if (availabilityRecord.AccountId != accountId)
            {
                throw new InvalidOperationException("Invalid account id");
            }

            availabilityRecord.UpdateName(notification.Name);
            availabilityRecord.UpdateUrl(notification.Url);
            availabilityRecord.UpdateExpectedResponse(notification.ExpectedResponse);
            availabilityRecord.UpdateExpectedStatusCode(notification.ExpectedStatusCode);
            availabilityRecord.UpdateLogLifetimeThresholdInHours(notification.LogLifetimeThresholdInHours);
        }
    }
}
