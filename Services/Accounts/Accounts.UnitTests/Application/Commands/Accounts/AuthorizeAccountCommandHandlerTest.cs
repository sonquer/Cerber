using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Commands.Accounts;
using Accounts.Api.Application.Services;
using Accounts.Domain.AggregateModels.AccountAggregate;
using Accounts.Infrastructure;
using Accounts.Infrastructure.Repository;
using Accounts.UnitTests.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Accounts.UnitTests.Application.Commands.Accounts
{
    public class AuthorizeAccountCommandHandlerTest
    {
        private readonly IAccountRepository _accountRepository;
        
        private readonly IAuthorizationService _authorizationService;
        
        public AuthorizeAccountCommandHandlerTest()
        {
            var options = new DbContextOptionsBuilder<AccountsContext>()
                .UseInMemoryDatabase(nameof(JwtAuthorizationServiceTest))
                .Options;

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(e => e[It.IsAny<string>()])
                .Returns("SeCrEtKeYSeCrEtKeYSeCrEtKeYSeCrEtKeYSeCrEtKeY");
            
            var databaseContext = new AccountsContext(options);
            
            _accountRepository = new AccountRepository(databaseContext);
            
            _authorizationService = new JwtAuthorizationService(_accountRepository, configuration.Object);
        }

        [Fact]
        public async Task AuthorizeAccountCommandHandler_Handle_AccountAuthorized()
        {
            await _accountRepository.AddAsync(new Account("sonquer@o2.pl", "secret"), CancellationToken.None);
            await _accountRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);

            var authorizeAccountCommandHandler = new AuthorizeAccountCommandHandler(_authorizationService);
            var token = await authorizeAccountCommandHandler.Handle(
                new AuthorizeAccountCommand("sonquer@o2.pl", "secret"),
                CancellationToken.None);
            
            Assert.NotNull(token);
            Assert.True(token.Token.Length > 0);
            Assert.True(token.ExpiresAt > DateTime.UtcNow);
        }
        
        [Fact]
        public async Task AuthorizeAccountCommandHandler_Handle_InvalidCredentialsFail()
        {
            await _accountRepository.AddAsync(new Account("sonquer@o2.pl", "secret"), CancellationToken.None);
            await _accountRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var authorizeAccountCommandHandler = new AuthorizeAccountCommandHandler(_authorizationService);
                var unused = await authorizeAccountCommandHandler.Handle(
                    new AuthorizeAccountCommand("sonquer@o2.pl", "asdfg"),
                    CancellationToken.None);
            });
        }
    }
}