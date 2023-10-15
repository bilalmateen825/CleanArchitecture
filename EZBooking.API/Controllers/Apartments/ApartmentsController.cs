using EZBooking.Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EZBooking.API.Controllers.Apartments
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly ISender m_sender;

        public ApartmentsController(ISender sender)
        {
            m_sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> SearchApartments(
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken)
        {
            var query = new SearchApartmentsQuery(startDate, endDate);

            var result = await m_sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }
    }
}
