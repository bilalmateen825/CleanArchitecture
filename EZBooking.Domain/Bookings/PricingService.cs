using EZBooking.Domain.Apartments;
using EZBooking.Domain.Bookings.ValueObject;
using EZBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Bookings
{
    public class PricingService
    {
        public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
        {
            var currency = apartment.Price.Currency;

            //apartment price * duration of stay
            var priceForPeriod = new Money(
                apartment.Price.Amount * period.LengthInDays,
                currency);

            decimal percentageUpCharge = 0;
            foreach (var amenity in apartment.Amenities)
            {
                percentageUpCharge += amenity switch
                {
                    Amenity.GardenView or Amenity.MountainView => 0.05m,//GardenView and MountainView incur 5% upcharge

                    Amenity.AirConditioning => 0.01m, //AirConditioning incur 1% upcharge

                    Amenity.Parking => 0.01m, //Parking incur 1% upcharge
                    _ => 0
                };
            }

            var amenitiesUpCharge = Money.Zero(currency);

            if (percentageUpCharge > 0)
            {
                //The amount we have for the period , we multiply it with the incur percentage of amenities
                amenitiesUpCharge = new Money(
                    priceForPeriod.Amount * percentageUpCharge,
                    currency);
            }

            var totalPrice = Money.Zero();

            totalPrice += priceForPeriod;

            //Adding if apartment has some cleaning fee
            if (!apartment.CleaningFee.IsZero())
            {
                totalPrice += apartment.CleaningFee;
            }

            totalPrice += amenitiesUpCharge;
            return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenitiesUpCharge, totalPrice);
        }
    }
}
