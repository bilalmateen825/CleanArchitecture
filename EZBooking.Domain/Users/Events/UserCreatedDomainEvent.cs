using EZBooking.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Users.Events
{
    public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
}
