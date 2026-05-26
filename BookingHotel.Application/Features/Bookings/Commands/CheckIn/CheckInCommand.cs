using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Enums;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;

namespace BookingHotel.Application.Features.Bookings.Commands.CheckIn;

public record CheckInCommand : IRequest<Result<Unit>>
{
    public int BookingId { get; init; }
}

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, Result<Unit>>
{
    private readonly IBookingRepository _bookingRepo;

    public CheckInCommandHandler(IBookingRepository bookingRepo) => _bookingRepo = bookingRepo;

    public async Task<Result<Unit>> Handle(CheckInCommand request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.BookingId);
        if (booking is null)
            return Result<Unit>.Failure("Booking not found", "NOT_FOUND");

        if (booking.Status != "Confirmed")
            return Result<Unit>.Failure("Booking must be confirmed before check-in", "INVALID_STATUS");

        booking.Status = "CheckedIn";
        _bookingRepo.Update(booking);
        return Result<Unit>.Success(Unit.Value);
    }
}
