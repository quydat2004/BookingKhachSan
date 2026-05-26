using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;

namespace BookingHotel.Application.Features.Hotels.Commands.UpdateHotel;

public record UpdateHotelCommand : IRequest<Result<Unit>>
{
    public int Id { get; init; }
    public string? HotelName { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? District { get; init; }
    public string? Description { get; init; }
    public byte? StarRating { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Website { get; init; }
}

public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Result<Unit>>
{
    private readonly IHotelRepository _hotelRepo;

    public UpdateHotelCommandHandler(IHotelRepository hotelRepo) => _hotelRepo = hotelRepo;

    public async Task<Result<Unit>> Handle(UpdateHotelCommand request, CancellationToken ct)
    {
        var hotel = await _hotelRepo.GetByIdAsync(request.Id);
        if (hotel is null)
            return Result<Unit>.Failure("Hotel not found", "NOT_FOUND");

        if (request.HotelName is not null) hotel.HotelName = request.HotelName;
        if (request.Address is not null) hotel.Address = request.Address;
        if (request.City is not null) hotel.City = request.City;
        if (request.District is not null) hotel.District = request.District;
        if (request.Description is not null) hotel.Description = request.Description;
        if (request.StarRating.HasValue) hotel.StarRating = request.StarRating.Value;
        if (request.Phone is not null) hotel.Phone = request.Phone;
        if (request.Email is not null) hotel.Email = request.Email;
        if (request.Website is not null) hotel.Website = request.Website;

        _hotelRepo.Update(hotel);
        return Result<Unit>.Success(Unit.Value);
    }
}
