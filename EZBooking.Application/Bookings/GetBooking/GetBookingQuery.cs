using EZBooking.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application.Bookings.GetBooking
{
    /// <summary>
    /// This query is going to return the booking response object
    /// </summary>
    /// <param name="BookingId"></param>
    public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;
}
