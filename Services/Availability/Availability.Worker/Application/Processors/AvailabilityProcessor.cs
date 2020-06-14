using System;
using System.Threading;
using System.Threading.Tasks;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Worker.Application.Services.Availability;

namespace Availability.Worker.Application.Processors
{
    public class AvailabilityProcessor : IAvailabilityProcessor
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityProcessor(IAvailabilityRecordRepository availabilityRecordRepository,
            IAvailabilityService availabilityService)
        {
            _availabilityRecordRepository = availabilityRecordRepository;
            _availabilityService = availabilityService;
        }
        
        public async Task ProcessAvailabilityRecord(Guid recordId, CancellationToken cancellationToken)
        {
            var availabilityRecord = await _availabilityRecordRepository.GetById(recordId, cancellationToken)
            .ConfigureAwait(false);

            var responseModel = await _availabilityService.Request(availabilityRecord.Url, cancellationToken);

            availabilityRecord.AppendLog((int) responseModel.HttpStatusCode, responseModel.Body,
                responseModel.ResponseTime);
            
            availabilityRecord.ClearOutdatedLogs();
            
            _availabilityRecordRepository.Update(availabilityRecord);
            await _availabilityRecordRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}