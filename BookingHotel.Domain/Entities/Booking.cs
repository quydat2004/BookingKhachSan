namespace BookingHotel.Domain.Entities;

public class Booking : Common.BaseAuditableEntity
{
    public string BookingCode { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int HotelId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string? SpecialRequests { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? CancelledAt { get; set; }
    public int? CancelledBy { get; set; }
    public string? CancellationReason { get; set; }
    public decimal? RefundAmount { get; set; }

    public User User { get; set; } = null!;
    public Hotel Hotel { get; set; } = null!;
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<BookingVoucher> BookingVouchers { get; set; } = new List<BookingVoucher>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
