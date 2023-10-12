using EZBooking.Application.Abstractions.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        public Task SendEmailAsync(Domain.Users.Email recipient, string subject, string body)
        {
            return Task.CompletedTask;  
        }
    }
}
