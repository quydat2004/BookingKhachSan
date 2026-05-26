namespace BookingHotel.Application.DTOs.Reports;

public class RevenueReportDto
{
    public DateTime ReportDate { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int? HotelId { get; set; }
    public string? HotelName { get; set; }
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal CancelledRevenue { get; set; }
    public decimal? AvgBookingValue { get; set; }
}

public class OccupancyReportDto
{
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public double OccupancyRate { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class TopHotelDto
{
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public double AvgRating { get; set; }
    public int TotalReviews { get; set; }
    public string City { get; set; } = string.Empty;
}
