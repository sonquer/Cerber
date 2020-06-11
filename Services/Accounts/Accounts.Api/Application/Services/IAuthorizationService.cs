using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Models;

namespace Accounts.Api.Application.Services
{
    public interface IAuthorizationService
    {
        Task<TokenModel> Authorize(string email, string password, CancellationToken cancellationToken);
    }
}