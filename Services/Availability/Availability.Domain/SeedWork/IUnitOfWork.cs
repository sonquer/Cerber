using System.Threading;
using System.Threading.Tasks;

namespace Availability.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}