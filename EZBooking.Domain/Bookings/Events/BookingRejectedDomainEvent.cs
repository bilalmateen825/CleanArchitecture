using EZBooking.Domain.Abstractions;

namespace EZBooking.Domain.Bookings;

public sealed record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;