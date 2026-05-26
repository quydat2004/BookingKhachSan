namespace BookingHotel.Application.DTOs.Payments;

public class PaymentDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string? PaymentGateway { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentUrl { get; set; }
}

public class ProcessPaymentRequest
{
    public int BookingId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; }
}

public class PaymentCallbackRequest
{
    public string? TransactionId { get; set; }
    public string? OrderInfo { get; set; }
    public string? ResponseCode { get; set; }
    public string? PaymentGateway { get; set; }
    public string? RawResponse { get; set; }
}
