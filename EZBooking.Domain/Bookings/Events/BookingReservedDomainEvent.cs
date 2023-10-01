
using EZBooking.Domain.Abstractions;

namespace EZBooking.Domain.Bookings
{
    public sealed record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;
}
