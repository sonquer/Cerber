using System.Threading;
using System.Threading.Tasks;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Domain.SeedWork;
using Availability.Infrastructure.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Availability.Infrastructure
{
    public class AvailabilityContext : DbContext, IUnitOfWork
    {
        public static string AVAILABILITY_SCHEMA = "availability";

        public AvailabilityContext(DbContextOptions<AvailabilityContext> availabilityContext) 
            : base(availabilityContext)
        {
        }
        
        public DbSet<AvailabilityRecord> AvailabilityRecords { get; set; }

        public DbSet<AvailabilityLog> AvailabilityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new AvailabilityRecordEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AvailabilityLogEntityTypeConfiguration());
        }
        
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}