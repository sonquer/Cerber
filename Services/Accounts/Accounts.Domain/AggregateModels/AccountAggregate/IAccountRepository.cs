using System.Threading;
using System.Threading.Tasks;
using Accounts.Domain.SeedWork;

namespace Accounts.Domain.AggregateModels.AccountAggregate
{
    public interface IAccountRepository : IRepository
    {
        Task<Account> AddAsync(Account account, CancellationToken cancellationToken);

        Task<Account> GetById(long id, CancellationToken cancellationToken);

        Task<Account> GetByEmail(string email, CancellationToken cancellationToken);
    }
}