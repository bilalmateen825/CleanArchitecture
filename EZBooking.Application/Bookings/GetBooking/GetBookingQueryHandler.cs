using Dapper;
using EZBooking.Application.Abstractions.Data;
using EZBooking.Application.Abstractions.Messaging;
using EZBooking.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application.Bookings.GetBooking
{
    internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
    {
        private readonly ISqlConnectionFactory m_sqlConnectionFactory;

        public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            m_sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            using var connection = m_sqlConnectionFactory.CreateConnection();

            const string sql = """
                                Select
                                    id AS Id,
                                    apartment_id AS ApartmentId,
                                    user_id AS UserId,
                                    status AS Status,
                                    price_for_period_amount AS PriceAmount,
                                    price_for_period_currency AS PriceCurrency,
                                    cleaning_fee_amount AS CleaningFeeAmount,
                                    cleaning_fee_currency AS CleaningFeeCurrency,
                                    amenities_up_charge_amount AS AmenitiesUpChargeAmount,
                                    amenities_up_charge_currency AS AmenitiesUpChargeCurrency,
                                    total_price_amount AS TotalPriceAmount,
                                    total_price_currency AS TotalPriceCurrency,
                                    duration_start AS DurationStart,
                                    duration_end AS DurationEnd,
                                    created_on_utc AS CreatedOnUtc
                                From bookings
                                Where id = @BookingId
                                """;

            BookingResponse booking = await connection.QueryFirstOrDefaultAsync<BookingResponse>
            (
               sql, // sql query
               new // parameter for this query
               {
                   request.BookingId
               }
            );

            return booking;
        }
    }
}
