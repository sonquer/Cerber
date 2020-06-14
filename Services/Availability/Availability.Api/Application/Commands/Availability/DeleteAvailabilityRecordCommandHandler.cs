using System;
using System.Threading;
using System.Threading.Tasks;
using Availability.Api.Application.Claims;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class DeleteAvailabilityRecordCommandHandler : INotificationHandler<DeleteAvailabilityRecordCommand>
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        private readonly IClaimConverter _claimConverter;

        public DeleteAvailabilityRecordCommandHandler(IAvailabilityRecordRepository availabilityRecordRepository,
            IClaimConverter claimConverter)
        {
            _availabilityRecordRepository = availabilityRecordRepository;
            _claimConverter = claimConverter;
        }
        
        public async Task Handle(DeleteAvailabilityRecordCommand notification, CancellationToken cancellationToken)
        {
            var accountId = _claimConverter.GetAccountId(notification.ClaimsPrincipal);
            
            var availabilityRecord = await _availabilityRecordRepository.GetById(notification.Id, cancellationToken)
                .ConfigureAwait(false);

            if (availabilityRecord.AccountId != accountId)
            {
                throw new InvalidOperationException("Operation not allowed");
            }
            
            _availabilityRecordRepository.Remove(availabilityRecord);
            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}