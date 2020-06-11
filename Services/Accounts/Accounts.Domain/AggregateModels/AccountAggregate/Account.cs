using System;
using Accounts.Domain.Helpers;
using Accounts.Domain.SeedWork;

namespace Accounts.Domain.AggregateModels.AccountAggregate
{
    public class Account : Entity, IAggregateRoot
    {
        public string Email { get; protected set; }

        public string HashedPassword { get; protected set; }

        public DateTime CreatedAt { get; protected set; }

        public DateTime UpdatedAt { get; protected set; }

        public Account(string email, string password)
        {
            PartitionKey = Guid.NewGuid();
            Id = Guid.NewGuid();
            Email = email;
            HashedPassword = SecurePasswordHasherHelper.Hash(password);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        protected Account()
        {
        }

        public void ChangePassword(string password)
        {
            HashedPassword = SecurePasswordHasherHelper.Hash(password);
        }

        public void UpdateModification()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public bool PasswordIsValid(string password)
        {
            return SecurePasswordHasherHelper.Verify(password, HashedPassword);
        }
    }
}