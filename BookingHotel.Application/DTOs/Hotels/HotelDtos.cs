namespace BookingHotel.Application.DTOs.Hotels;

public class HotelDto
{
    public int Id { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public byte StarRating { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public decimal? MinPrice { get; set; }
    public double? AvgRating { get; set; }
    public int TotalReviews { get; set; }

    // Added for compatibility with HotelApproval/Index view
    public string Name => HotelName;
    public string? MainImageUrl => ThumbnailUrl;
    public string? Description { get; set; }
    public string? ManagerName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public List<HotelAmenityDto> Amenities { get; set; } = new();
}

public class HotelDetailDto
{
    public int Id { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? District { get; set; }
    public string City { get; set; } = string.Empty;
    public string? Description { get; set; }
    public byte StarRating { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public TimeSpan CheckInTime { get; set; }
    public TimeSpan CheckOutTime { get; set; }
    public string? Status { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public List<string> Images { get; set; } = new();
    public List<HotelAmenityDto> Amenities { get; set; } = new();
    public List<RoomTypeSummaryDto> RoomTypes { get; set; } = new();
    public double? AvgRating { get; set; }
    public int TotalReviews { get; set; }
}

public class HotelAmenityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? IconClass { get; set; }
}

public class RoomTypeSummaryDto
{
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int BedCount { get; set; }
    public decimal Price { get; set; }
    public int AvailableRooms { get; set; }
}

public class CreateHotelRequest
{
    public string HotelName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? District { get; set; }
    public string? Description { get; set; }
    public byte StarRating { get; set; } = 3;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
}

public class UpdateHotelRequest
{
    public string? HotelName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Description { get; set; }
    public byte? StarRating { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
}

public class HotelSearchRequest
{
    public string? City { get; set; }
    public string? Keyword { get; set; }
    public byte? MinRating { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
