using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;

namespace BookingHotel.Application.Features.Bookings.Commands.CheckOut;

public record CheckOutCommand : IRequest<Result<Unit>>
{
    public int BookingId { get; init; }
    public decimal? AdditionalCharges { get; init; }
}

public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, Result<Unit>>
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IRoomRepository _roomRepo;

    public CheckOutCommandHandler(IBookingRepository bookingRepo, IRoomRepository roomRepo)
    {
        _bookingRepo = bookingRepo;
        _roomRepo = roomRepo;
    }

    public async Task<Result<Unit>> Handle(CheckOutCommand request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.BookingId);
        if (booking is null)
            return Result<Unit>.Failure("Booking not found", "NOT_FOUND");

        if (booking.Status != "CheckedIn")
            return Result<Unit>.Failure("Booking must be checked in before check-out", "INVALID_STATUS");

        if (request.AdditionalCharges.HasValue && request.AdditionalCharges.Value > 0)
            booking.TotalAmount += request.AdditionalCharges.Value;

        booking.Status = "CheckedOut";
        foreach (var detail in booking.BookingDetails)
        {
            if (detail.Room != null)
            {
                detail.Room.Status = "Cleaning";
                _roomRepo.Update(detail.Room);
            }
        }

        _bookingRepo.Update(booking);
        return Result<Unit>.Success(Unit.Value);
    }
}
