using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Commands.Accounts;
using Accounts.Infrastructure;
using Accounts.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Accounts.UnitTests.Application.Commands.Accounts
{
    public class CreateAccountCommandHandlerTest
    {
        [Fact]
        public async Task CreateAccountCommandHandler_Handle_AccountCreated()
        {
            var options = new DbContextOptionsBuilder<AccountsContext>()
                .UseInMemoryDatabase(nameof(CreateAccountCommandHandler_Handle_AccountCreated))
                .Options;

            var accountContext = new AccountsContext(options);
            
            var accountRepository = new AccountRepository(accountContext);
            
            var createAccountCommandHandler = new CreateAccountCommandHandler(accountRepository);

            var accountId = await createAccountCommandHandler
                .Handle(new CreateAccountCommand("test", "password"), CancellationToken.None)
                .ConfigureAwait(false);
            
            Assert.True(Guid.TryParse(accountId.ToString(), out var unused));
        }
        
        [Fact]
        public async Task CreateAccountCommandHandler_Handle_AccountAlreadyExistsFail()
        {
            var options = new DbContextOptionsBuilder<AccountsContext>()
                .UseInMemoryDatabase(nameof(CreateAccountCommandHandler_Handle_AccountAlreadyExistsFail))
                .Options;

            var accountContext = new AccountsContext(options);
            
            var accountRepository = new AccountRepository(accountContext);
            
            var createAccountCommandHandler = new CreateAccountCommandHandler(accountRepository);

            await createAccountCommandHandler
                .Handle(new CreateAccountCommand("test", "password"), CancellationToken.None)
                .ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await createAccountCommandHandler
                    .Handle(new CreateAccountCommand("test", "password"), CancellationToken.None)
                    .ConfigureAwait(false);
            });
        }
    }
}