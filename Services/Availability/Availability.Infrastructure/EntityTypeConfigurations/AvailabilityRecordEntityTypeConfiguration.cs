using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Availability.Infrastructure.EntityTypeConfigurations
{
    public class AvailabilityRecordEntityTypeConfiguration : IEntityTypeConfiguration<AvailabilityRecord>
    {
        public void Configure(EntityTypeBuilder<AvailabilityRecord> builder)
        {
            builder.ToTable("availability_records", AvailabilityContext.AVAILABILITY_SCHEMA);

            builder.HasKey(e => e.Id)
                .HasName("pk_availability_records_id");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(e => e.AccountId)
                .HasColumnName("account_id");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.Url)
                .HasColumnName("url");

            builder.Property(e => e.ExpectedResponse)
                .HasColumnName("expected_response");

            builder.Property(e => e.ExpectedStatusCode)
                .HasColumnName("expected_status_code");

            builder.Property(e => e.LogLifetimeThresholdInHours)
                .HasColumnName("log_lifetime_threshold_in_hours");

            builder.HasIndex(e => e.AccountId)
                .HasName("ix_availiability_recrods_account_id")
                .IsUnique();

            builder.HasMany(e => e.AvailabilityLogs)
                .WithOne()
                .HasForeignKey("availability_record_id")
                .HasConstraintName("fk_availability_records_availability_logs");
        }
    }
}