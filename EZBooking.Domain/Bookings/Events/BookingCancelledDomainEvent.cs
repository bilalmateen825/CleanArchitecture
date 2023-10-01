using EZBooking.Domain.Abstractions;

namespace EZBooking.Domain.Bookings;

public sealed record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent;