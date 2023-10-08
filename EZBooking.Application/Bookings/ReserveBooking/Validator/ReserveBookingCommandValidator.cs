using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application.Bookings.ReserveBooking.Validator
{
    public class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            //they are going to be trigger in my validation behavior when my pipeline is executed
            RuleFor(c=> c.UserId).NotEmpty();
            RuleFor(c=> c.ApartmentId).NotEmpty();
            RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
        }
    }
}
