using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Commands.Accounts;
using Accounts.Domain.AggregateModels.AccountAggregate;
using Accounts.Infrastructure;
using Accounts.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Accounts.UnitTests.Application.Commands.Accounts
{
    public class CreateAccountCommandHandlerTest
    {
        private readonly IAccountRepository _accountRepository;
        
        public CreateAccountCommandHandlerTest()
        {
            var options = new DbContextOptionsBuilder<AccountsContext>()
                .UseInMemoryDatabase(nameof(CreateAccountCommandHandlerTest))
                .Options;

            var accountContext = new AccountsContext(options);
            
            _accountRepository = new AccountRepository(accountContext);
        }
        
        [Fact]
        public async Task CreateAccountCommandHandler_Handle_AccountCreated_Success()
        {
            var createAccountCommandHandler = new CreateAccountCommandHandler(_accountRepository);

            var accountId = await createAccountCommandHandler
                .Handle(new CreateAccountCommand("test", "password"), CancellationToken.None)
                .ConfigureAwait(false);
            
            Assert.True(Guid.TryParse(accountId.ToString(), out var unused));
        }
    }
}