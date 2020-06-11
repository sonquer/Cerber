using System;

namespace Accounts.Api.Application.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        
        public DateTime ExpiresAt { get; set; }
    }
}