using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using BookingHotel.Application.Features.Bookings.Commands.CreateBooking;
using BookingHotel.Application.Features.Bookings.Commands.CancelBooking;
using BookingHotel.Application.Features.Bookings.Commands.CheckIn;
using BookingHotel.Application.Features.Bookings.Commands.CheckOut;
using BookingHotel.Application.Features.Bookings.Commands.ConfirmBooking;
using BookingHotel.Application.Features.Bookings.Queries.GetBookingById;
using BookingHotel.Application.Features.Bookings.Queries.GetBookingHistory;

namespace BookingHotel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingCommand command)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        command = command with { UserId = userId };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.Error });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetBookingByIdQuery { Id = id });
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { error = result.Error });
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new GetBookingHistoryQuery { UserId = userId });
        return Ok(result.Data);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id, [FromBody] string? reason)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new CancelBookingCommand { BookingId = id, UserId = userId, Reason = reason });
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }

    [HttpPost("{id}/confirm")]
    [Authorize(Roles = "HotelManager,Receptionist")]
    public async Task<IActionResult> Confirm(int id)
    {
        var result = await _mediator.Send(new ConfirmBookingCommand { BookingId = id });
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }

    [HttpPost("{id}/checkin")]
    [Authorize(Roles = "HotelManager,Receptionist")]
    public async Task<IActionResult> CheckIn(int id)
    {
        var result = await _mediator.Send(new CheckInCommand { BookingId = id });
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }

    [HttpPost("{id}/checkout")]
    [Authorize(Roles = "HotelManager,Receptionist")]
    public async Task<IActionResult> CheckOut(int id, [FromBody] decimal? additionalCharges)
    {
        var result = await _mediator.Send(new CheckOutCommand { BookingId = id, AdditionalCharges = additionalCharges });
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }
}
