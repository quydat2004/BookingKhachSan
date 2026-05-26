using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.Interfaces.Services;
using AutoMapper;
using BookingHotel.Application.DTOs.Hotels;

namespace BookingHotel.Application.Features.Hotels.Commands.CreateHotel;

public record CreateHotelCommand : IRequest<Result<HotelDto>>
{
    public int ManagerId { get; init; }
    public string HotelName { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string? District { get; init; }
    public string? Description { get; init; }
    public byte StarRating { get; init; } = 3;
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Website { get; init; }
}

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<HotelDto>>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IMapper _mapper;

    public CreateHotelCommandHandler(IHotelRepository hotelRepo, IMapper mapper)
    {
        _hotelRepo = hotelRepo;
        _mapper = mapper;
    }

    public async Task<Result<HotelDto>> Handle(CreateHotelCommand request, CancellationToken ct)
    {
        var hotel = new Hotel
        {
            ManagerId = request.ManagerId,
            HotelName = request.HotelName,
            Address = request.Address,
            City = request.City,
            District = request.District,
            Description = request.Description,
            StarRating = request.StarRating,
            Phone = request.Phone,
            Email = request.Email,
            Website = request.Website,
            Status = "Pending"
        };

        await _hotelRepo.CreateAsync(hotel);
        return Result<HotelDto>.Success(_mapper.Map<HotelDto>(hotel));
    }
}
