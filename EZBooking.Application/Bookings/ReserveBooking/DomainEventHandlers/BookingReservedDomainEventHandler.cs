using EZBooking.Application.Abstractions.Email;
using EZBooking.Domain.Bookings;
using EZBooking.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application.Bookings.ReserveBooking.DomainEventHandlers
{
    //Naming convention is easy. We specify what is the command,query or event and then append handler
    internal sealed class BookingReservedDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
    {
        private readonly IBookingRepository m_bookingRepository;
        private readonly IUserRepository m_userRepository;
        private readonly IEmailService m_emailService;

        public BookingReservedDomainEventHandler(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IEmailService emailService)
        {
            m_bookingRepository = bookingRepository;
            m_userRepository = userRepository;
            m_emailService = emailService;
        }

        public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
        {
            var booking = await m_bookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);

            if (booking is null)
                return;

            var user = await m_userRepository.GetByIdAsync(booking.UserId, cancellationToken);

            if (user is null)
                return;

            await m_emailService.SendEmailAsync(user.Email, "Booking Reserved!", "Please confirm booking in 10 minutes otherwise it will be cancelled.");
        }
    }
}
