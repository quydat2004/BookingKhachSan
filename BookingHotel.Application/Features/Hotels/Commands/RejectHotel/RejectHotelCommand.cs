using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;

namespace BookingHotel.Application.Features.Hotels.Commands.RejectHotel;

public record RejectHotelCommand(int Id, string Reason) : IRequest<Result<bool>>;

public class RejectHotelCommandHandler : IRequestHandler<RejectHotelCommand, Result<bool>>
{
    private readonly IHotelRepository _hotelRepo;

    public RejectHotelCommandHandler(IHotelRepository hotelRepo) => _hotelRepo = hotelRepo;

    public async Task<Result<bool>> Handle(RejectHotelCommand request, CancellationToken ct)
    {
        var hotel = await _hotelRepo.GetByIdAsync(request.Id);
        if (hotel == null) return Result<bool>.Failure("Hotel not found");

        hotel.Status = "Rejected";
        // Optionally save reason to extra field if it exists
        _hotelRepo.Update(hotel);

        return Result<bool>.Success(true);
    }
}
