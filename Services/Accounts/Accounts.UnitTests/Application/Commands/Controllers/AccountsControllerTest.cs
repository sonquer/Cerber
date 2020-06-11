using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Commands.Accounts;
using Accounts.Api.Application.Dtos;
using Accounts.Api.Application.Models;
using Accounts.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Accounts.UnitTests.Application.Commands.Controllers
{
    public class AccountsControllerTest
    {
        [Fact]
        public async Task AccountsController_CreateAccount_EventSend_Success()
        {
            var guid = Guid.NewGuid();
            
            var mediator = new Mock<IMediator>();
            mediator.Setup(e => e.Send(It.IsAny<CreateAccountCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(guid);

            var accountsController = new AccountsController(mediator.Object);

            var createAccountDto = new AccountDto
            {
                Email = "test@hotmail.com",
                Password = "secret"
            };

            var createdAccountActionResult = await accountsController
                .CreateAccount(createAccountDto, CancellationToken.None)
                .ConfigureAwait(false);
            
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(createdAccountActionResult);
            
            Assert.Equal(guid, objectResult.Value);
        }
        
        [Fact]
        public async Task AccountsController_AuthorizeAccount_EventSend_Success()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(e => e.Send(It.IsAny<AuthorizeAccountCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TokenModel
                {
                    Token = "token",
                    ExpiresAt = DateTime.UtcNow
                });

            var accountsController = new AccountsController(mediator.Object);

            var createAccountDto = new AccountDto
            {
                Email = "test@hotmail.com",
                Password = "secret"
            };

            var authorizeAccountActionResult = await accountsController
                .AuthorizeAccount(createAccountDto, CancellationToken.None)
                .ConfigureAwait(false);
            
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(authorizeAccountActionResult);
            var token = (TokenModel) objectResult.Value;
            
            Assert.Equal("token", token.Token);
        }
    }
}