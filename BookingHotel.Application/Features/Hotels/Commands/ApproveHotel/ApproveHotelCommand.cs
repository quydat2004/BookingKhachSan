using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;

namespace BookingHotel.Application.Features.Hotels.Commands.ApproveHotel;

public record ApproveHotelCommand(int Id) : IRequest<Result<bool>>;

public class ApproveHotelCommandHandler : IRequestHandler<ApproveHotelCommand, Result<bool>>
{
    private readonly IHotelRepository _hotelRepo;

    public ApproveHotelCommandHandler(IHotelRepository hotelRepo) => _hotelRepo = hotelRepo;

    public async Task<Result<bool>> Handle(ApproveHotelCommand request, CancellationToken ct)
    {
        var hotel = await _hotelRepo.GetByIdAsync(request.Id);
        if (hotel == null) return Result<bool>.Failure("Hotel not found");

        hotel.Status = "Approved";
        hotel.IsActive = true;
        _hotelRepo.Update(hotel);

        return Result<bool>.Success(true);
    }
}
