using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Api.Application.Models;
using Accounts.Domain.AggregateModels.AccountAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Api.Application.Services
{
    public class JwtAuthorizationService : IAuthorizationService
    {
        private readonly IAccountRepository _accountRepository;
        
        private readonly IConfiguration _configuration;

        public JwtAuthorizationService(IAccountRepository accountRepository,
            IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;

            if (string.IsNullOrWhiteSpace(_configuration["Token:Key"]))
            {
                throw new ArgumentException("Configuration Token:Key is null or empty");
            }
        }
        
        public async Task<TokenModel> Authorize(string email, string password, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByEmail(email, cancellationToken)
                .ConfigureAwait(false);
            
            if (account is null)
            {
                throw new InvalidOperationException("Account not found");
            }

            if (account.PasswordIsValid(password) == false)
            {
                throw new InvalidOperationException("Invalid password");
            }

            return GenerateToken(account);
        }

        private TokenModel GenerateToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Token:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "cerber_identity",
                Issuer = "cerber_identity",
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()) 
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenModel
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };
        }
    }
}