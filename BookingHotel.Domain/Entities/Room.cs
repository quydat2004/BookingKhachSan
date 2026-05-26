namespace BookingHotel.Domain.Entities;

public class Room : Common.BaseAuditableEntity
{
    public int HotelId { get; set; }
    public int RoomTypeId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int? FloorNumber { get; set; }
    public string Status { get; set; } = "Available";
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }

    public Hotel Hotel { get; set; } = null!;
    public RoomType RoomType { get; set; } = null!;
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}
