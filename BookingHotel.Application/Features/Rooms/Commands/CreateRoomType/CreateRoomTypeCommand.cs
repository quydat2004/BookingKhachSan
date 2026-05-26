using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Rooms;

namespace BookingHotel.Application.Features.Rooms.Commands.CreateRoomType;

public record CreateRoomTypeCommand : IRequest<Result<RoomTypeDto>>
{
    public int HotelId { get; init; }
    public string TypeName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int Capacity { get; init; }
    public int BedCount { get; init; } = 1;
    public string? BedType { get; init; }
    public decimal? RoomSize { get; init; }
    public decimal Price { get; init; }
    public decimal? WeekendPrice { get; init; }
    public decimal? HolidayPrice { get; init; }
    public int MaxAdults { get; init; } = 2;
    public int MaxChildren { get; init; } = 1;
}

public class CreateRoomTypeCommandHandler : IRequestHandler<CreateRoomTypeCommand, Result<RoomTypeDto>>
{
    private readonly IRoomTypeRepository _roomTypeRepo;
    private readonly IMapper _mapper;

    public CreateRoomTypeCommandHandler(IRoomTypeRepository roomTypeRepo, IMapper mapper)
    {
        _roomTypeRepo = roomTypeRepo;
        _mapper = mapper;
    }

    public async Task<Result<RoomTypeDto>> Handle(CreateRoomTypeCommand request, CancellationToken ct)
    {
        var roomType = new RoomType
        {
            HotelId = request.HotelId,
            TypeName = request.TypeName,
            Description = request.Description,
            Capacity = request.Capacity,
            BedCount = request.BedCount,
            BedType = request.BedType,
            RoomSize = request.RoomSize,
            Price = request.Price,
            WeekendPrice = request.WeekendPrice,
            HolidayPrice = request.HolidayPrice,
            MaxAdults = request.MaxAdults,
            MaxChildren = request.MaxChildren
        };

        await _roomTypeRepo.CreateAsync(roomType);
        return Result<RoomTypeDto>.Success(_mapper.Map<RoomTypeDto>(roomType));
    }
}
