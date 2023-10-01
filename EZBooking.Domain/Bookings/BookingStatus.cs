using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Bookings
{
    public enum BookingStatus
    {
        None=0,
        Confirmed,
        Reserved,
        Rejected,
        Cancelled,
        Completed,
    }
}
