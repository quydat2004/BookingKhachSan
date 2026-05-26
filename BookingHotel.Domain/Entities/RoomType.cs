namespace BookingHotel.Domain.Entities;

public class RoomType : Common.BaseAuditableEntity
{
    public int HotelId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public int BedCount { get; set; } = 1;
    public string? BedType { get; set; }
    public decimal? RoomSize { get; set; }
    public decimal Price { get; set; }
    public decimal? WeekendPrice { get; set; }
    public decimal? HolidayPrice { get; set; }
    public int MaxAdults { get; set; } = 2;
    public int MaxChildren { get; set; } = 1;
    public bool IsActive { get; set; } = true;

    public Hotel Hotel { get; set; } = null!;
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
}
