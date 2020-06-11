using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Commands.Accounts;
using Accounts.Api.Application.Dtos;
using Accounts.Api.Application.Models;
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
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDto accountDto, CancellationToken cancellationToken)
        {
            var accountId = await _mediator.Send(new CreateAccountCommand(accountDto.Email, accountDto.Password),
                cancellationToken).ConfigureAwait(false);

            return Ok(accountId);
        }
        
        [HttpPost("Authorize")]
        [ProducesResponseType(typeof(TokenModel), 200)]
        public async Task<IActionResult> AuthorizeAccount([FromBody] AccountDto accountDto, CancellationToken cancellationToken)
        {
            var accountId = await _mediator.Send(new AuthorizeAccountCommand(accountDto.Email, accountDto.Password),
                cancellationToken).ConfigureAwait(false);

            return Ok(accountId);
        }
    }
}
