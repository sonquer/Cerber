using System.Threading;
using System.Threading.Tasks;
using Accounts.Domain.AggregateModels.AccountAggregate;
using MediatR;

namespace Accounts.Api.Application.Commands.Accounts
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, long>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        
        public async Task<long> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account(request.Email, request.Password);

            await _accountRepository.AddAsync(account, cancellationToken)
                .ConfigureAwait(false);

            await _accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken)
                .ConfigureAwait(false);

            return account.Id;
        }
    }
}