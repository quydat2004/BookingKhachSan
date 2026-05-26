namespace BookingHotel.Domain.ValueObjects;

public class GeoLocation : Common.ValueObject
{
    public decimal Latitude { get; }
    public decimal Longitude { get; }

    public GeoLocation(decimal latitude, decimal longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
