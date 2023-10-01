using EZBooking.Domain.Abstractions;

namespace EZBooking.Domain.Reviews.Events;

public sealed record ReviewCreatedDomainEvent(Guid ReviewId) : IDomainEvent;