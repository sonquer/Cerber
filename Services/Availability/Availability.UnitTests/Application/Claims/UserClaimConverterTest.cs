using System;
using System.Security.Claims;
using Availability.Api.Application.Claims;
using Xunit;

namespace Availability.UnitTests.Application.Claims
{
    public class UserClaimConverterTest
    {
        [Fact]
        public void UserClaimConverter_GetAccountId_AccountIdReturned()
        {
            var accountId = Guid.NewGuid();
            var userClaimConverter = new UserClaimConverter();

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountId.ToString()));
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsIdentity);

            var returnedAccountId = userClaimConverter.GetAccountId(claimsPrincipal);

            Assert.Equal(accountId, returnedAccountId);
        }
    }
}