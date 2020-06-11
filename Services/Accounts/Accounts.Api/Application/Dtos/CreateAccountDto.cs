using System.ComponentModel.DataAnnotations;

namespace Accounts.Api.Application.Dtos
{
    public class CreateAccountDto
    {
        [EmailAddress]
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}