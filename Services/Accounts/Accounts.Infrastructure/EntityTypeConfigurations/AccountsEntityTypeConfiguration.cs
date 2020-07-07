using Accounts.Domain.AggregateModels.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.EntityTypeConfigurations
{
    public class AccountsEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts", AccountsContext.ACCOUNTS_SCHEMA);
            
            builder.HasKey(e => e.Id)
                .HasName("pk_accounts_id");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(e => e.Email)
                .HasColumnName("email");

            builder.Property(e => e.HashedPassword)
                .HasColumnName("hashed_password");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            builder.HasIndex(e => e.Email)
                .HasName("ix_acounts_email")
                .IsUnique();
        }
    }
}