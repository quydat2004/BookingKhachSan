namespace BookingHotel.Domain.Entities.Views;

public class AvailableRoomView
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int? FloorNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal Price { get; set; }
    public int BedCount { get; set; }
    public string? BedType { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public byte StarRating { get; set; }
}

public class BookingSummaryView
{
    public int BookingId { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfNights { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? PaymentStatus { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
}

public class RevenueReportView
{
    public DateTime ReportDate { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal CancelledRevenue { get; set; }
    public decimal? AvgBookingValue { get; set; }
}

public class HotelRatingView
{
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public int TotalReviews { get; set; }
    public double AvgRating { get; set; }
    public double? AvgStaff { get; set; }
    public double? AvgCleanliness { get; set; }
    public double? AvgComfort { get; set; }
    public double? AvgLocation { get; set; }
    public double? AvgValue { get; set; }
}
