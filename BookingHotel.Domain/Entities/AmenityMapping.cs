namespace BookingHotel.Domain.Entities;

public class HotelAmenity
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int AmenityId { get; set; }

    public Hotel Hotel { get; set; } = null!;
    public Amenity Amenity { get; set; } = null!;
}
