namespace Accounts.Domain.SeedWork
{
    using System;

    public abstract class Entity
    {
        public Guid PartitionKey { get; protected set; }
        
        public Guid Id { get; protected set; }
    }
}
