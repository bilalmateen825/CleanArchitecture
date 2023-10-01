using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Common
{
    public record Money(decimal Amount, Currency Currency)
    {
        public static Money operator +(Money first, Money second)
        {
            //Currencies of the money objects must be equal
            if (first.Currency != second.Currency)
            {
                throw new InvalidOperationException("Currencies have to be equal");
            }

            return new Money(first.Amount + second.Amount, first.Currency);
        }

        public static Money Zero() => new Money(0, Currency.None);

        public static Money Zero(Currency currency)
        {
            return new Money(0, currency);
        }

        public bool IsZero()
        {
            return this == Zero(Currency);
        }
    }
}
