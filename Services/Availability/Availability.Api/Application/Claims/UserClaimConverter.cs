using System;
using System.Linq;
using System.Security.Claims;

namespace Availability.Api.Application.Claims
{
    public class UserClaimConverter : IClaimConverter
    {
        public Guid GetAccountId(ClaimsPrincipal claimsPrincipal)
        {
            var claimAccountId = claimsPrincipal.Claims
                .FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            return Guid.TryParse(claimAccountId, out var accountId) == false ? Guid.Empty : accountId;
        }
    }
}