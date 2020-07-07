using System;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Availability.Infrastructure.EntityTypeConfigurations
{
    public class AvailabilityLogEntityTypeConfiguration : IEntityTypeConfiguration<AvailabilityLog>
    {
        public void Configure(EntityTypeBuilder<AvailabilityLog> builder)
        {
            builder.ToTable("availability_logs", AvailabilityContext.AVAILABILITY_SCHEMA);

            builder.HasKey(e => e.Id)
                .HasName("pk_availability_logs_id");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property<Guid>("availability_record_id")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("availability_record_id")
                .IsRequired();

            builder.Property(e => e.Body)
                .HasColumnName("body");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(e => e.ResponseTime)
                .HasColumnName("response_time");

            builder.Property(e => e.StatusCode)
                .HasColumnName("status_code");
        }
    }
}
