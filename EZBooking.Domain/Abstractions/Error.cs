using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Abstractions
{
    public record Error(string stCode, string stName)
    {
        public static Error None = new(string.Empty, string.Empty);
        public static Error NullValue = new("Error.NullValue", "Null value was provided");
    }
}
