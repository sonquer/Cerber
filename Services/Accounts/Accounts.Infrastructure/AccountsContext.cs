using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Domain.AggregateModels.AccountAggregate;
using Accounts.Domain.SeedWork;
using Accounts.Infrastructure.EntityTypeConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure
{
    public class AccountsContext : DbContext, IUnitOfWork
    {
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(AccountsContext)}::ctor");
        }

        public DbSet<Account> Accounts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new AccountsEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}