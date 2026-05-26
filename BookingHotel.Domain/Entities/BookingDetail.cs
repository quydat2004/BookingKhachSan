namespace BookingHotel.Domain.Entities;

public class BookingDetail
{
    public int DetailId { get; set; }
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public int RoomTypeId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal SubTotal { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Booking Booking { get; set; } = null!;
    public Room Room { get; set; } = null!;
    public RoomType RoomType { get; set; } = null!;
}
