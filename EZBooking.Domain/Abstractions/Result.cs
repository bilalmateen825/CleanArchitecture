using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Abstractions
{
    public class Result
    {
        protected internal Result(bool bSuccess, Error error)
        {
            if (bSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!bSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = bSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure
        {
            get { return !IsSuccess; }
        }

        public Error Error { get; }
        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
        public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? value;

        protected internal Result(TValue? value, bool bSuccess, Error error)
            : base(bSuccess, error)
        {
            this.value = value;
        }

        [NotNull]
        public TValue Value => IsSuccess ? value! : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        //implicit conversion operator allow  to convert a nullable TValue  into a Result<TValue>
        //implementation of the conversion is handled by the Create method.
        public static implicit operator Result<TValue>(TValue? value) => Create(value);
    }
}
