using EZBooking.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application.Apartments.SearchApartments
{
    /// <summary>
    /// Reture list of apartment with the provided date range
    /// </summary>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    public sealed record SearchApartmentsQuery(
        DateOnly StartDate,
        DateOnly EndDate): IQuery<IReadOnlyList<ApartmentResponse>>;
}
