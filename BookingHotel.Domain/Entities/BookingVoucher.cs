namespace BookingHotel.Domain.Entities;

public class BookingVoucher
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int VoucherId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public Booking Booking { get; set; } = null!;
    public Voucher Voucher { get; set; } = null!;
}
