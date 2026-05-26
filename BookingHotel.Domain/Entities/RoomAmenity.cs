namespace BookingHotel.Domain.Entities;

public class RoomAmenity
{
    public int Id { get; set; }
    public int RoomTypeId { get; set; }
    public int AmenityId { get; set; }

    public RoomType RoomType { get; set; } = null!;
    public Amenity Amenity { get; set; } = null!;
}
