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
    /// we added IBaseCommand interface that can help us to apply generic constraints in our pipeline behaviors for cross cutting concerns.
    /// </summary>
    public interface IBaseCommand
    {
    }

    /// <summary>
    /// we enforce that all of our commands have to return a result
    /// </summary>
    public interface ICommand : IRequest<Result>, IBaseCommand
    {
    }

    /// <summary>
    /// we enforce that all of our commands have to return a result and also add flexibility for the command to return some sort of value
    /// which could be useful in some use cases.
    /// </summary>
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {
    }
}
