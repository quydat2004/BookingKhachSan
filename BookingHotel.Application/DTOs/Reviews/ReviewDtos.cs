namespace BookingHotel.Application.DTOs.Reviews;

public class ReviewDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int HotelId { get; set; }
    public byte Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public byte? StaffRating { get; set; }
    public byte? CleanlinessRating { get; set; }
    public byte? ComfortRating { get; set; }
    public byte? LocationRating { get; set; }
    public byte? ValueRating { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTime ReviewDate { get; set; }
}

public class CreateReviewRequest
{
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
    public bool IsAnonymous { get; set; }
}
