namespace BookingHotel.Domain.Entities;

public class Amenity : Common.BaseEntity
{
    public string AmenityName { get; set; } = string.Empty;
    public string? IconClass { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
    public ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
}
