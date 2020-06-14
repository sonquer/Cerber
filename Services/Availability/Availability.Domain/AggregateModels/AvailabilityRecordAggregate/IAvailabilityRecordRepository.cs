using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Availability.Domain.SeedWork;

namespace Availability.Domain.AggregateModels.AvailabilityRecordAggregate
{
    public interface IAvailabilityRecordRepository : IRepository
    {
        Task<AvailabilityRecord> AddAsync(AvailabilityRecord availabilityRecord, CancellationToken cancellationToken);

        void Remove(AvailabilityRecord availabilityRecord);

        void Update(AvailabilityRecord availabilityRecord);

        Task<AvailabilityRecord> GetById(Guid id, CancellationToken cancellationToken);

        Task<List<AvailabilityRecord>> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
    }
}