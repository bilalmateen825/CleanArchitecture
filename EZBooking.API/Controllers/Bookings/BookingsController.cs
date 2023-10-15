using EZBooking.Application.Bookings.GetBooking;
using EZBooking.Application.Bookings.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EZBooking.API.Controllers.Bookings
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ISender m_sender;

        public BookingsController(ISender sender)
        {
            m_sender = sender;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetBookingQuery(id);

            var result = await m_sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ReserveBooking(
            ReserveBookingRequest request,
            CancellationToken cancellationToken
            )
        {
            var command = new ReserveBookingCommand(
                request.ApartmentId,
                request.UserId,
                request.StartDate,
                request.EndDate);

            var result = await m_sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
        }
    }
}
