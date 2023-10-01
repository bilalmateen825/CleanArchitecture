using EZBooking.Domain.Abstractions;

namespace EZBooking.Domain.Bookings;

public sealed record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvent;