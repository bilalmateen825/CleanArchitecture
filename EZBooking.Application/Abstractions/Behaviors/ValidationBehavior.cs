using EZBooking.Application.Abstractions.Messaging;
using EZBooking.Application.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application.Abstractions.Behaviors
{
    /// <summary>
    /// TRequest : IBaseCommand because we only want to run validation for our commands
    /// </summary>
    /// <typeparam name="TRequest"> Command</typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly IEnumerable<IValidator<TRequest>> m_validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            m_validators = validators;
        }

        public async Task<TResponse> Handle
            (TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            //checking if we dont defined any validators for this type we immidiately invoke the command and return from the pipeline
            if (!m_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationErrors = m_validators
                .Select(validator => validator.Validate(context))
                .Where(validationResult => validationResult.Errors.Any())
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new ValidationError(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage))
                .ToList();

            if (validationErrors.Any())
            {
                throw new Exceptions.ValidationException(validationErrors);
            }

            return await next();
        }
    }
}
