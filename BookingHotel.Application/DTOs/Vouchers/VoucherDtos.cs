namespace BookingHotel.Application.DTOs.Vouchers;

public class VoucherDto
{
    public int Id { get; set; }
    public string VoucherCode { get; set; } = string.Empty;
    public string DiscountType { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public decimal? MaxDiscount { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int? UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public bool IsActive { get; set; }
}

public class ApplyVoucherRequest
{
    public string VoucherCode { get; set; } = string.Empty;
    public decimal OrderAmount { get; set; }
}

public class ApplyVoucherResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public VoucherDto? Voucher { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
}
