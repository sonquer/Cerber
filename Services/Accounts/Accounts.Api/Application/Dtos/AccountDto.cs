using System.ComponentModel.DataAnnotations;

namespace Accounts.Api.Application.Dtos
{
    public class AccountDto
    {
        [EmailAddress]
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}