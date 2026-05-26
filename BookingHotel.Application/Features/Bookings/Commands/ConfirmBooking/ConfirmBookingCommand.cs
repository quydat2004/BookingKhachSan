using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;

namespace BookingHotel.Application.Features.Bookings.Commands.ConfirmBooking;

public record ConfirmBookingCommand : IRequest<Result<Unit>>
{
    public int BookingId { get; init; }
}

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, Result<Unit>>
{
    private readonly IBookingRepository _bookingRepo;

    public ConfirmBookingCommandHandler(IBookingRepository bookingRepo) => _bookingRepo = bookingRepo;

    public async Task<Result<Unit>> Handle(ConfirmBookingCommand request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.BookingId);
        if (booking is null)
            return Result<Unit>.Failure("Booking not found", "NOT_FOUND");

        if (booking.Status != "Pending")
            return Result<Unit>.Failure("Only pending bookings can be confirmed", "INVALID_STATUS");

        booking.Status = "Confirmed";
        _bookingRepo.Update(booking);
        return Result<Unit>.Success(Unit.Value);
    }
}
