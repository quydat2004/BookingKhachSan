namespace BookingHotel.Domain.Entities;

public class Hotel : Common.BaseAuditableEntity
{
    public int ManagerId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? District { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Description { get; set; }
    public byte StarRating { get; set; } = 3;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public TimeSpan CheckInTime { get; set; } = new(14, 0, 0);
    public TimeSpan CheckOutTime { get; set; } = new(12, 0, 0);
    public string Status { get; set; } = "Pending";
    public bool IsActive { get; set; } = true;

    public User Manager { get; set; } = null!;
    public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
