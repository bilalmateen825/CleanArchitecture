using EZBooking.Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application.Abstractions.Messaging
{
    /// <summary>
    /// We enforce that all of our Queries in the application return an envelop response which is the Result type
    /// The queries can either succeed or faul and the result object should communicate this.
    /// </summary>
    /// <typeparam name="TResponse"> TResponse should specify what is the type returned by this query</typeparam>
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {

    }
}
