using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Availability.Infrastructure;
using Availability.Worker.Application.Processors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Availability.Worker.Application.Commands
{
    public class RequestServicesCommandHandler : INotificationHandler<RequestServicesCommand>
    {
        private readonly AvailabilityContext _availabilityContext;
        
        private readonly IAvailabilityProcessor _availabilityProcessor;

        public RequestServicesCommandHandler(AvailabilityContext availabilityContext,
            IAvailabilityProcessor availabilityProcessor)
        {
            _availabilityContext = availabilityContext;
            _availabilityProcessor = availabilityProcessor;
        }
        
        public async Task Handle(RequestServicesCommand notification, CancellationToken cancellationToken)
        {
            var availabilityRecordIds = await _availabilityContext.AvailabilityRecords
                .Select(e => e.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            Parallel.ForEach(availabilityRecordIds, new ParallelOptions
            {
                MaxDegreeOfParallelism = 8,
                CancellationToken = cancellationToken
            },availabilityRecordId =>
            {
                _availabilityProcessor.ProcessAvailabilityRecord(availabilityRecordId, cancellationToken)
                    .GetAwaiter()
                    .GetResult();
            });
        }
    }
}