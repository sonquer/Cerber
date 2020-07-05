using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Services;
using Accounts.Domain.AggregateModels.AccountAggregate;
using Accounts.Infrastructure;
using Accounts.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Accounts.UnitTests.Application.Services
{
    public class JwtAuthorizationServiceTest
    {
        private readonly AccountsContext _databaseContext;

        private readonly IConfiguration _configuration;
        
        public JwtAuthorizationServiceTest()
        {
            var options = new DbContextOptionsBuilder<AccountsContext>()
                .UseInMemoryDatabase(nameof(JwtAuthorizationServiceTest))
                .Options;
            
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(e => e[It.IsAny<string>()])
                .Returns("SeCrEtKeYSeCrEtKeYSeCrEtKeYSeCrEtKeYSeCrEtKeY");

            _configuration = configuration.Object;
            
            _databaseContext = new AccountsContext(options);
        }
        
        [Fact]
        public async Task JwtAuthorizationService_Constructor_ConfigurationIsMissingFail()
        {
            var repository = new AccountRepository(_databaseContext);
            
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(e => e[It.IsAny<string>()])
                .Returns(string.Empty);

            Assert.Throws<ArgumentException>(() =>
            {
                var unused = new JwtAuthorizationService(repository, configuration.Object);
            });
        }
        
        [Fact]
        public async Task JwtAuthorizationService_Authorize_AccountAuthorized()
        {
            var repository = new AccountRepository(_databaseContext);
            await repository.AddAsync(new Account("sonquer@o2.pl", "secret"), CancellationToken.None);
            await repository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);
            var jwtAuthorizationService = new JwtAuthorizationService(repository, _configuration);

            var token = await jwtAuthorizationService.Authorize("sonquer@o2.pl", "secret", CancellationToken.None);
            
            Assert.NotNull(token);
            Assert.True(token.Token.Length > 0);
            Assert.True(token.ExpiresAt > DateTime.UtcNow);
        }

        [Fact]
        public async Task JwtAuthorizationService_Authorize_AccountNotExistsAuthorizationFailed()
        {
            var repository = new AccountRepository(_databaseContext);
            await repository.AddAsync(new Account("sonquer@o2.pl", "secret"), CancellationToken.None);
            await repository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);
            var jwtAuthorizationService = new JwtAuthorizationService(repository, _configuration);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var unused = await jwtAuthorizationService.Authorize("test@wp.pl", "asdfg", CancellationToken.None);
            });
        }
        
        [Fact]
        public async Task JwtAuthorizationService_Authorize_InvalidPasswordFail()
        {
            var repository = new AccountRepository(_databaseContext);
            await repository.AddAsync(new Account("sonquer@o2.pl", "secret"), CancellationToken.None);
            await repository.UnitOfWork.SaveEntitiesAsync(CancellationToken.None);
            var jwtAuthorizationService = new JwtAuthorizationService(repository, _configuration);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var unused = await jwtAuthorizationService.Authorize("sonquer@o2.pl", "asdfg", CancellationToken.None);
            });
        }
    }
}