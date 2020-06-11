using Accounts.Domain.AggregateModels.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Accounts.Infrastructure.EntityTypeConfigurations
{
    public class AccountsEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToContainer("Accounts");
            
            builder.HasNoDiscriminator();
            
            var converter = new GuidToStringConverter();
            
            builder.HasPartitionKey(e => e.PartitionKey);

            builder.Property(e => e.PartitionKey)
                .HasConversion(converter)
                .ValueGeneratedNever();
            
            builder.Property(e => e.Id)
                .HasConversion(converter)
                .ValueGeneratedNever();

            builder.HasIndex(e => e.Email)
                .IsUnique();
        }
    }
}