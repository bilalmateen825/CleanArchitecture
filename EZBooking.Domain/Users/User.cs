using EZBooking.Domain.Abstractions;
using EZBooking.Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Users
{
    public sealed class User : Entity
    {
        private User(
            Guid id,
            FirstName firstName,
            LastName lastName,
            Email email
            ) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Email Email { get; private set; }

        /// <summary>
        /// Factory to generate User
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static User Create(FirstName firstName, LastName lastName, Email email)
        {
            User user = new User(Guid.NewGuid(), firstName, lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            return user;
        }
    }
}
