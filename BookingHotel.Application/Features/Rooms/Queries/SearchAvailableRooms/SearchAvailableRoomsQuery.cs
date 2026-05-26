using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Rooms;

namespace BookingHotel.Application.Features.Rooms.Queries.SearchAvailableRooms;

public record SearchAvailableRoomsQuery : IRequest<Result<IEnumerable<AvailableRoomDto>>>
{
    public int HotelId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public int? RoomTypeId { get; init; }
}

public class SearchAvailableRoomsQueryHandler : IRequestHandler<SearchAvailableRoomsQuery, Result<IEnumerable<AvailableRoomDto>>>
{
    private readonly IRoomRepository _roomRepo;
    private readonly IMapper _mapper;

    public SearchAvailableRoomsQueryHandler(IRoomRepository roomRepo, IMapper mapper)
    {
        _roomRepo = roomRepo;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<AvailableRoomDto>>> Handle(SearchAvailableRoomsQuery request, CancellationToken ct)
    {
        var rooms = await _roomRepo.GetAvailableRoomsAsync(request.HotelId, request.CheckInDate, request.CheckOutDate, request.RoomTypeId);
        var dtos = rooms.Select(r => new AvailableRoomDto
        {
            RoomId = r.Id,
            RoomNumber = r.RoomNumber,
            FloorNumber = r.FloorNumber,
            RoomTypeId = r.RoomTypeId,
            RoomTypeName = r.RoomType.TypeName,
            Capacity = r.RoomType.Capacity,
            BedCount = r.RoomType.BedCount,
            BedType = r.RoomType.BedType,
            Price = r.RoomType.Price
        });
        return Result<IEnumerable<AvailableRoomDto>>.Success(dtos);
    }
}
