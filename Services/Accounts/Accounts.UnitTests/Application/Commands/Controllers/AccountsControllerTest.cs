using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Commands.Accounts;
using Accounts.Api.Application.Dtos;
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
            var mediator = new Mock<IMediator>();
            mediator.Setup(e => e.Send(It.IsAny<CreateAccountCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var accountsController = new AccountsController(mediator.Object);

            var createAccountDto = new CreateAccountDto
            {
                Email = "test@hotmail.com",
                Password = "secret"
            };

            var createdAccountActionResult = await accountsController
                .CreateAccount(createAccountDto, CancellationToken.None)
                .ConfigureAwait(false);
            
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(createdAccountActionResult);
            
            Assert.Equal((long)1, objectResult.Value);
        }
    }
}