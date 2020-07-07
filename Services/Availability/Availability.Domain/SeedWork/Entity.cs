namespace Availability.Domain.SeedWork
{
    using System;

    public abstract class Entity
    {
        public Guid Id { get; protected set; }
    }
}
