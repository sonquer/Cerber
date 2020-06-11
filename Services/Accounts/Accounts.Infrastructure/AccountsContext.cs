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
        private readonly IMediator _mediator;
        
        public AccountsContext(DbContextOptions<AccountsContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
            await _mediator.DispatchDomainEventsAsync(this);
            
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}