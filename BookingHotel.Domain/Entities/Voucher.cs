namespace BookingHotel.Domain.Entities;

public class Voucher : Common.BaseEntity
{
    public string VoucherCode { get; set; } = string.Empty;
    public string DiscountType { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public decimal? MaxDiscount { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int? UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<BookingVoucher> BookingVouchers { get; set; } = new List<BookingVoucher>();
}
