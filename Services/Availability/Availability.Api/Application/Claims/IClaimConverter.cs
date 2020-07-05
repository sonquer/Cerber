using System;
using System.Security.Claims;

namespace Availability.Api.Application.Claims
{
    public interface IClaimConverter
    {
        Guid GetAccountId(ClaimsPrincipal claimsPrincipal);
    }
}