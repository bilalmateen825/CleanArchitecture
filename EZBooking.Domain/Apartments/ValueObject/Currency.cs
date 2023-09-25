using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Apartments
{
    public record Currency
    {
        internal static readonly Currency None = new Currency("");
        public static readonly Currency USD = new Currency("USD");
        public static readonly Currency CAD = new Currency("CAD");
        public static readonly Currency EUR = new ("EUR");
        public Currency(string stCode)
        {
            Code = stCode;
        }

        public string Code { get; init; }

        public static readonly IReadOnlyCollection<Currency> Currencies = new[]
        {
            USD,
            CAD,
            EUR
        };

        public static Currency GetCurrencyFromCode(string stCode)
        {
            return Currencies.FirstOrDefault(x => x.Code == stCode) ?? throw new ApplicationException("The currency code is invalid");
        }
    }
}
