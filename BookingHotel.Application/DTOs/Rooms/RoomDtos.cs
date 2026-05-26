namespace BookingHotel.Application.DTOs.Rooms;

public class RoomTypeDto
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public int BedCount { get; set; }
    public string? BedType { get; set; }
    public decimal? RoomSize { get; set; }
    public decimal Price { get; set; }
    public decimal? WeekendPrice { get; set; }
    public decimal? HolidayPrice { get; set; }
    public int MaxAdults { get; set; }
    public int MaxChildren { get; set; }
    public string[] Images { get; set; } = [];
    public List<string> Amenities { get; set; } = new();
}

public class RoomDto
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int RoomTypeId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int? FloorNumber { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class AvailableRoomDto
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int? FloorNumber { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public int Capacity { get; set; }
    public int BedCount { get; set; }
    public string? BedType { get; set; }
    public decimal Price { get; set; }
}

public class CreateRoomTypeRequest
{
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public int BedCount { get; set; } = 1;
    public string? BedType { get; set; }
    public decimal? RoomSize { get; set; }
    public decimal Price { get; set; }
    public decimal? WeekendPrice { get; set; }
    public decimal? HolidayPrice { get; set; }
    public int MaxAdults { get; set; } = 2;
    public int MaxChildren { get; set; } = 1;
}

public class AvailableRoomSearchRequest
{
    public int HotelId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int? RoomTypeId { get; set; }
    public int? GuestCount { get; set; }
}
