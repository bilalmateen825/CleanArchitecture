using EZBooking.Domain.Apartments;
using EZBooking.Domain.Users;
using EZBooking.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Infrastructure.Repositories
{
    internal class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
