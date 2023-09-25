using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Apartments
{
    public record Address(
        string Country,
        string State,
        string ZipCode,
        string City,
        string Street);
}
