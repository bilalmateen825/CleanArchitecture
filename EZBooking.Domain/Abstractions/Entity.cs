
using System;
using System.Xml.Linq;

namespace EZBooking.Domain.Abstractions
{
    public abstract class Entity : IEquatable<Entity>
    {
        #region Private Members

        private readonly List<IDomainEvent> m_domainEvents = new();
        #endregion

        #region Constructor
        protected Entity(Guid id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        public Guid Id { get; init; }

        #endregion

        #region Methods

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return m_domainEvents.ToList();
        }

        public void ClearDomainEvents()
        {
            m_domainEvents.Clear();
        }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            m_domainEvents.Add(domainEvent);
        }

        #endregion

        #region IEquitable Implementation

        public override bool Equals(object obj)
        {
            // Check if obj is null or not of the same type as this
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Entity other = (Entity)obj;

            return Id == other.Id;
        }

        public bool Equals(Entity? other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id;
        }

        #endregion

    }
}
