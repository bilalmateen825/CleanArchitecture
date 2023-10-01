using EZBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Bookings.ValueObject
{
    public record PricingDetails(
        Money PriceForPeriod,
        Money CleaningFee,
        Money AmenitiesUpCharge,
        Money TotalPrice);
}
