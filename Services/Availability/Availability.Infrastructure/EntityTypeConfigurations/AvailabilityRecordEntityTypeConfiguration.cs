using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Availability.Infrastructure.EntityTypeConfigurations
{
    public class AvailabilityRecordEntityTypeConfiguration : IEntityTypeConfiguration<AvailabilityRecord>
    {
        public void Configure(EntityTypeBuilder<AvailabilityRecord> builder)
        {
            builder.ToContainer("AvailabilityRecords");
            
            builder.HasNoDiscriminator();
            
            var converter = new GuidToStringConverter();
            
            builder.HasPartitionKey(e => e.PartitionKey);

            builder.Property(e => e.PartitionKey)
                .HasConversion(converter)
                .ValueGeneratedNever();
            
            builder.Property(e => e.Id)
                .HasConversion(converter)
                .ValueGeneratedNever();

            builder.OwnsMany(e => e.AvailabilityLogs);
        }
    }
}