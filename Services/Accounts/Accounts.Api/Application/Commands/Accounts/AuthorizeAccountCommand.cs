using Accounts.Api.Application.Models;
using MediatR;

namespace Accounts.Api.Application.Commands.Accounts
{
    public class AuthorizeAccountCommand : IRequest<TokenModel>
    {
        public string Email { get; set; }
        
        public string Password { get; set; }

        public AuthorizeAccountCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}