using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Availability.Infrastructure.Repositories
{
    public class AvailabilityRecordRepository : IAvailabilityRecordRepository
    {
        private readonly AvailabilityContext _availabilityContext;

        public AvailabilityRecordRepository(AvailabilityContext availabilityContext)
        {
            _availabilityContext = availabilityContext;
        }

        public IUnitOfWork UnitOfWork => _availabilityContext;
        
        public async Task<AvailabilityRecord> AddAsync(AvailabilityRecord availabilityRecord, CancellationToken cancellationToken)
        {
            await _availabilityContext.Database.EnsureCreatedAsync(cancellationToken);
            
            await _availabilityContext.AvailabilityRecords.AddAsync(availabilityRecord, cancellationToken)
                .ConfigureAwait(false);

            return availabilityRecord;
        }

        public void Remove(AvailabilityRecord availabilityRecord)
        {
            _availabilityContext.AvailabilityRecords.Remove(availabilityRecord);
        }

        public void Update(AvailabilityRecord availabilityRecord)
        {
            _availabilityContext.AvailabilityRecords.Update(availabilityRecord);
        }

        public async Task<AvailabilityRecord> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _availabilityContext.AvailabilityRecords
                .Include(e => e.AvailabilityLogs)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<AvailabilityRecord>> GetByAccountId(Guid accountId, CancellationToken cancellationToken)
        {
            return await _availabilityContext.AvailabilityRecords
                .Include(e => e.AvailabilityLogs)
                .Where(e => e.AccountId == accountId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}