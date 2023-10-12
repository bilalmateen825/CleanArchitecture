using EZBooking.Domain.Apartments;
using EZBooking.Domain.Bookings;
using EZBooking.Domain.Bookings.ValueObject;
using EZBooking.Domain.Users;
using EZBooking.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Infrastructure.Repositories
{
    internal class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private static readonly BookingStatus[] ActiveBookingStatuses =
        {
            BookingStatus.Reserved,
            BookingStatus.Confirmed,
            BookingStatus.Completed,
        };

        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<bool> IsOverlappingAsync(
            Apartment apartment,
            DateRange duration, 
            CancellationToken cancellationToken = default)
        {
            return await DBContext
                .Set<Booking>()
                .AnyAsync(
                    booking =>
                        booking.ApartmentId == apartment.Id &&
                        booking.Duration.Start <= duration.End &&
                        booking.Duration.End >= duration.Start &&
                        ActiveBookingStatuses.Contains(booking.Status),
                    cancellationToken);
        }
    }
}
