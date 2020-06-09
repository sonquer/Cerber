using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Commands.Accounts;
using Accounts.Api.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("Create")]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createAccountDto, CancellationToken cancellationToken)
        {
            var accountId = await _mediator.Send(new CreateAccountCommand(createAccountDto.Email, createAccountDto.Password),
                cancellationToken).ConfigureAwait(false);

            return Ok(accountId);
        }
    }
}
