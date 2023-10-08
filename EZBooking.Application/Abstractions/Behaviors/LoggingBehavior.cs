using EZBooking.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EZBooking.Application.Abstractions.Behaviors
{
    /// <summary>
    /// We passed TRequest & TResponse to IPipelineBehavior Interface
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly ILogger<TRequest> m_logger;

        public LoggingBehavior(ILogger<TRequest> logger)
        {
            m_logger = logger;
        }

        /// <summary>
        /// RequestHandlerDelegate is our command Handler
        /// </summary>
        /// <param name="request">It is our command</param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;

            try
            {
                m_logger.LogInformation("Executing command {Command}", name);

                var result = await next();

                m_logger.LogInformation("Command {Command} processed sucessfully", name);

                return result;
            }
            catch (Exception ex)
            {
                m_logger.LogError(ex, "Command {Command} processing failed", name);
                throw;
            }
        }
    }
}
