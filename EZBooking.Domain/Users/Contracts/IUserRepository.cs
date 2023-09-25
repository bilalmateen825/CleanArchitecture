using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Domain.Users.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid id, CancellationToken cancellationToken = default);
        void Add(User user);
    }
}
