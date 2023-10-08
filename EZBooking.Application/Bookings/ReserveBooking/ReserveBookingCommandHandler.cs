using EZBooking.Application.Abstractions.Clock;
using EZBooking.Application.Abstractions.Messaging;
using EZBooking.Domain.Abstractions;
using EZBooking.Domain.Apartments;
using EZBooking.Domain.Bookings;
using EZBooking.Domain.Bookings.ValueObject;
using EZBooking.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application.Bookings.ReserveBooking
{
    //ReserveBookingCommand return Guid Response
    //check document
    public sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IUserRepository m_userRepository;
        private readonly IApartmentRepository m_apartmentRepository;
        private readonly IBookingRepository m_bookingRepository;
        private readonly IUnitOfWork m_unitOfWork;
        private readonly PricingService m_pricingService;
        private readonly IDateTimeProvider m_dateTimeProvider;

        public ReserveBookingCommandHandler(
            IUserRepository userRepository,
            IApartmentRepository apartmentRepository,
            IBookingRepository bookingRepository,
            IUnitOfWork unitOfWork,
            PricingService pricingService,
            IDateTimeProvider dateTimeProvider)
        {
            m_userRepository = userRepository;
            m_apartmentRepository = apartmentRepository;
            m_bookingRepository = bookingRepository;
            m_unitOfWork = unitOfWork;
            m_pricingService = pricingService;
            m_dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await m_userRepository.GetByIdAsync(request.UserId, cancellationToken);

            //to reserve booking user should be valid
            if (user is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            var apartment = await m_apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);

            //Apartment should exist for booking
            if (apartment is null)
            {
                return Result.Failure<Guid>(ApartmentErrors.NotFound);
            }

            var duration = DateRange.Create(request.StartDate, request.EndDate);

            //checking if booking is overlaping with existing booking from database
            if (await m_bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }

            var booking = Booking.Reserve(
                apartment,
                user.Id,
                duration,
                m_dateTimeProvider.UtcNow,
                m_pricingService);

            m_bookingRepository.Add(booking);

            await m_unitOfWork.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }
    }
}
