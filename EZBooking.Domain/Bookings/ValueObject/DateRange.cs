using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Bookings.ValueObject
{
    public record DateRange
    {
        public DateRange()
        {
            
        }

        public DateOnly Start { get; init; }
        public DateOnly End { get; set; }

        public int LengthInDays => End.DayNumber - Start.DayNumber;

        public static DateRange Create(DateOnly start, DateOnly end)
        {
            if(start > end)
            {
                throw new ApplicationException("End date precedes start date");
            }

            return new DateRange
            {
                Start = start,
                End = end,
            };
        }
    }
}
