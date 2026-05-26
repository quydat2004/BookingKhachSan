namespace BookingHotel.Domain.Entities;

public class Payment : Common.BaseAuditableEntity
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = "Pending";
    public string? TransactionId { get; set; }
    public string? PaymentGateway { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentDetails { get; set; }

    public Booking Booking { get; set; } = null!;
    public User User { get; set; } = null!;
}
