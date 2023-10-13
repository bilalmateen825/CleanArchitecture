using Dapper;
using EZBooking.Application.Abstractions.Clock;
using EZBooking.Application.Abstractions.Data;
using EZBooking.Application.Abstractions.Email;
using EZBooking.Domain.Abstractions;
using EZBooking.Domain.Apartments;
using EZBooking.Domain.Bookings;
using EZBooking.Domain.Users;
using EZBooking.Infrastructure.Clock;
using EZBooking.Infrastructure.Data;
using EZBooking.Infrastructure.Database;
using EZBooking.Infrastructure.Email;
using EZBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EZBooking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //configuration (it wrap my connection string from the app settings)

            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            var connectionString = configuration.GetConnectionString("Database") ??
                throw new ArgumentException(nameof(configuration));

            //Registering EFCore
            //AddDbContext method is exposed by EntityFramework core
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //On DatabaseContextOptionsBuilder we mentioned that we are going to use specific provider for our database
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            //Registering the IUnitOfWork and Handover to service provider to resolve the database context and use it as Unit of Work Implementation.
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(options => new SqlConnectionFactory(connectionString));

            //adding DateOnly type handler for Dapper dependency to support DateOnly
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            return services;
        }
    }
}