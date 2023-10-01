using EZBooking.Domain.Abstractions;

namespace EZBooking.Domain.Bookings;

public sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;