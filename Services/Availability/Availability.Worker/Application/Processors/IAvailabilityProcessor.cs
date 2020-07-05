using System;
using System.Threading;
using System.Threading.Tasks;

namespace Availability.Worker.Application.Processors
{
    public interface IAvailabilityProcessor
    {
        Task ProcessAvailabilityRecord(Guid recordId, CancellationToken cancellationToken);
    }
}