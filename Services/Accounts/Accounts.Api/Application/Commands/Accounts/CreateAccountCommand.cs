using System;
using MediatR;

namespace Accounts.Api.Application.Commands.Accounts
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        
        public string Password { get; set; }

        public CreateAccountCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}