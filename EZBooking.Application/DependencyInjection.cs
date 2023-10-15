using EZBooking.Application.Abstractions.Behaviors;
using EZBooking.Domain.Bookings;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Application
{
    /// <summary>
    /// Responsible for registering the services that are specific to application layer
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(conf =>
            {
                //this is wiring up, the Command & Command handler,
                //Query & Query handler
                conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
                //typeof(DependencyInjection).Assembly is consequently the Application project

                conf.AddOpenBehavior(typeof(LoggingBehavior<,>));
                conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });


            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddTransient<PricingService>();

            return services;
        }
    }
}
