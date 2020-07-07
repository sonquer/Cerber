using System;
using Accounts.Domain.AggregateModels.AccountAggregate;
using Xunit;

namespace Accounts.UnitTests.Domain.AccountAggregate
{
    public class AccountAggregateTest
    {
        [Fact]
        public void Account_Constructor_Created_Success()
        {
            var account = new Account("test@hotmail.com", "password");

            Assert.True(IsValidGuid(account.Id));
            Assert.Equal("test@hotmail.com", account.Email);
            Assert.Equal(DateTime.UtcNow.Date, account.CreatedAt.Date);
            Assert.Equal(DateTime.UtcNow.Date, account.UpdatedAt.Date);
        }

        private static bool IsValidGuid(Guid guid) => Guid.TryParse(guid.ToString(), out var unused);
        
        [Fact]
        public void Account_UpdateModification_Updated_Success()
        {
            var account = new Account("test@hotmail.com", "password");

            account.UpdateModification();
            
            Assert.True(IsValidGuid(account.Id));
            Assert.Equal("test@hotmail.com", account.Email);
            Assert.Equal(DateTime.UtcNow.Date, account.CreatedAt.Date);
            Assert.Equal(DateTime.UtcNow.Date, account.UpdatedAt.Date);
        }
        
        [Fact]
        public void Account_Constructor_PasswordIsValid_Success()
        {
            var account = new Account("test", "password");

            Assert.True(account.PasswordIsValid("password"));
        }

        [Fact]
        public void Account_Constructor_PasswordIsInvalid_Success()
        {
            var account = new Account("test", "password");

            Assert.False(account.PasswordIsValid("password123"));
        }

        [Fact]
        public void Account_ChangePassword_PasswordIsChanged_Success()
        {
            var account = new Account("test", "password");

            account.ChangePassword("password123");

            Assert.True(account.PasswordIsValid("password123"));
        }
        
        [Fact]
        public void Account_ChangePassword_PasswordIsNotChanged_Fail()
        {
            var account = new Account("test", "password");

            account.ChangePassword("password123");

            Assert.False(account.PasswordIsValid("password"));
        }
    }
}
