using EZBooking.Domain.Abstractions;
using EZBooking.Domain.Bookings.ValueObject;
using EZBooking.Domain.Users.Events;
using EZBooking.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZBooking.Domain.Common;
using EZBooking.Domain.Apartments;

namespace EZBooking.Domain.Bookings
{
    public sealed class Booking : Entity
    {
        private Booking(
            Guid id,
            Guid apartmentId,
            Guid userId,
            DateRange duration,
            Money priceForPeriod,
            Money cleaningFee,
            Money amenitiesUpCharge,
            Money totalPrice,
            BookingStatus bookingStatus,
            DateTime createdOnUtc) : base(id)
        {
            ApartmentId = apartmentId;
            UserId = userId;
            Duration = duration;
            PriceForPeriod = priceForPeriod;
            CleaningFee = cleaningFee;
            AmenitiesUpCharge = amenitiesUpCharge;
            TotalPrice = totalPrice;
            Status = bookingStatus;
            CreatedOnUtc = createdOnUtc;
        }

        public Guid ApartmentId { get; set; }
        public Guid UserId { get; set; }
        public DateRange Duration { get; set; }
        public Money PriceForPeriod { get; set; }
        public Money CleaningFee { get; set; }
        public Money AmenitiesUpCharge { get; set; }
        public Money TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ConfirmedOnUtc { get; set; }
        public DateTime? RejectedOnUtc { get; set; }
        public DateTime? CompletedOnUtc { get; set; }
        public DateTime? CancelledOnUtc { get; set; }

        public static Booking Reserve(
            Apartment apartment,
            Guid userId,
            DateRange duration,
            DateTime UtcNow,
            PricingService pricingService)
        {
            //Pricing is required for booking.
            //The calculations that naturally doesnot fit the responsibilities of your entities
            //To calculate pricing we make a domain service
            PricingDetails pricingDetails = pricingService.CalculatePrice(apartment, duration);
            
            Booking booking = new Booking(
                Guid.NewGuid(),
                apartment.Id, 
                userId, duration,
                pricingDetails.PriceForPeriod,
                pricingDetails.CleaningFee,
                pricingDetails.AmenitiesUpCharge,
                pricingDetails.TotalPrice,
                BookingStatus.Reserved,
                UtcNow);

            booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));

            apartment.LastBookedOnUtc = UtcNow;

            return booking;
        }
         
        public Result Confirm(DateTime utcNow)
        {
            //we Confirm booking if Status is Reserved
            if (Status != BookingStatus.Reserved)
                return Result.Failure(BookingErrors.NotReserved);

            Status = BookingStatus.Confirmed;
            ConfirmedOnUtc=utcNow;
            RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));
            return Result.Success();
        }

        public Result Reject(DateTime utcNow)
        {
            //we Reject booking if Status is Reserved
            if (Status != BookingStatus.Reserved)
                return Result.Failure(BookingErrors.NotReserved);

            Status = BookingStatus.Rejected;
            RejectedOnUtc = utcNow;
            RaiseDomainEvent(new BookingRejectedDomainEvent(Id));
            return Result.Success();
        }

        public Result Complete(DateTime utcNow)
        {
            //we Complete booking if Status is Confirmed
            if (Status != BookingStatus.Confirmed)
                return Result.Failure(BookingErrors.NotConfirmed);

            Status = BookingStatus.Completed;
            CompletedOnUtc = utcNow;
            RaiseDomainEvent(new BookingCompletedDomainEvent(Id));
            return Result.Success();
        }

        public Result Cancel(DateTime utcNow)
        {
            //we Cancel booking if Status is Confirmed
            if (Status != BookingStatus.Confirmed)
                return Result.Failure(BookingErrors.NotConfirmed);

            var currentDate = DateOnly.FromDateTime(utcNow);

            if(currentDate > Duration.Start)
                return Result.Failure(BookingErrors.AlreadyStarted);

            Status = BookingStatus.Cancelled;
            CancelledOnUtc = utcNow;
            RaiseDomainEvent(new BookingCancelledDomainEvent(Id));
            return Result.Success();
        }
    }
}
