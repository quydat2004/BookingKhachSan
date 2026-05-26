using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using BookingHotel.Application.Features.Payments.Commands.ProcessPayment;
using BookingHotel.Application.Features.Payments.Queries.GetPaymentStatus;

namespace BookingHotel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Process(ProcessPaymentCommand command)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        command = command with { UserId = userId };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.Error });
    }

    [HttpGet("booking/{bookingId}")]
    public async Task<IActionResult> GetStatus(int bookingId)
    {
        var result = await _mediator.Send(new GetPaymentStatusQuery { BookingId = bookingId });
        return Ok(result.Data);
    }
}
