using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Models;
using Accounts.Api.Application.Services;
using MediatR;

namespace Accounts.Api.Application.Commands.Accounts
{
    public class AuthorizeAccountCommandHandler : IRequestHandler<AuthorizeAccountCommand, TokenModel>
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizeAccountCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        
        public async Task<TokenModel> Handle(AuthorizeAccountCommand request, CancellationToken cancellationToken)
        {
            var token = await _authorizationService.Authorize(request.Email, request.Password, cancellationToken)
                .ConfigureAwait(false);

            return token;
        }
    }
}