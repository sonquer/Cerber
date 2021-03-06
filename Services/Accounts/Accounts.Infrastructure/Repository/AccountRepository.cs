using System.Threading;
using System.Threading.Tasks;
using Accounts.Domain.AggregateModels.AccountAggregate;
using Accounts.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountsContext _accountsContext;

        public AccountRepository(AccountsContext accountsContext)
        {
            _accountsContext = accountsContext;
        }

        public IUnitOfWork UnitOfWork => _accountsContext;
        
        public async Task<Account> AddAsync(Account account, CancellationToken cancellationToken)
        {
            await _accountsContext.Database.EnsureCreatedAsync(cancellationToken);
            
            await _accountsContext.Accounts.AddAsync(account, cancellationToken)
                .ConfigureAwait(false);
            
            return account;
        }

        public async Task<Account> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var account = await _accountsContext.Accounts.FirstOrDefaultAsync(e => e.Email == email, cancellationToken)
                .ConfigureAwait(false);

            return account;
        }
    }
}