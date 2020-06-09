using System;
using Accounts.Domain.Helpers;
using Xunit;

namespace Accounts.UnitTests.Domain.Helpers
{
    public class SecurePasswordHasherHelperTest
    {
        [Fact]
        public void SecurePasswordHasherHelper_Hash_PasswordHashed_Success()
        {
            const string password = "secret";

            var hash = SecurePasswordHasherHelper.Hash(password);
            
            Assert.NotEqual(password, hash);
        }
        
        [Fact]
        public void SecurePasswordHasherHelper_Verify_NotSupportedExceptionWasThrown_Fail()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                SecurePasswordHasherHelper.Verify("test", "test");
            });
        }
    }
}