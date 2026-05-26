using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Enums;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;

namespace BookingHotel.Application.Features.Bookings.Commands.CancelBooking;

public record CancelBookingCommand : IRequest<Result<Unit>>
{
    public int BookingId { get; init; }
    public int UserId { get; init; }
    public string? Reason { get; init; }
}

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result<Unit>>
{
    private readonly IBookingRepository _bookingRepo;

    public CancelBookingCommandHandler(IBookingRepository bookingRepo) => _bookingRepo = bookingRepo;

    public async Task<Result<Unit>> Handle(CancelBookingCommand request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.BookingId);
        if (booking is null)
            return Result<Unit>.Failure("Booking not found", "NOT_FOUND");

        if (booking.UserId != request.UserId)
            return Result<Unit>.Failure("You can only cancel your own bookings", "FORBIDDEN");

        if (booking.Status is "Cancelled" or "CheckedOut")
            return Result<Unit>.Failure("Booking cannot be cancelled", "INVALID_STATUS");

        booking.Status = "Cancelled";
        booking.CancelledAt = DateTime.UtcNow;
        booking.CancelledBy = request.UserId;
        booking.CancellationReason = request.Reason;

        _bookingRepo.Update(booking);
        return Result<Unit>.Success(Unit.Value);
    }
}
