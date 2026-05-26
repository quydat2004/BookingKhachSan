namespace BookingHotel.Domain.Entities;

public class Review : Common.BaseAuditableEntity
{
    public int UserId { get; set; }
    public int HotelId { get; set; }
    public int BookingId { get; set; }
    public byte Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public byte? StaffRating { get; set; }
    public byte? CleanlinessRating { get; set; }
    public byte? ComfortRating { get; set; }
    public byte? LocationRating { get; set; }
    public byte? ValueRating { get; set; }
    public bool IsApproved { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Hotel Hotel { get; set; } = null!;
    public Booking Booking { get; set; } = null!;
}
