namespace BookingHotel.Domain.ValueObjects;

public class Address : Common.ValueObject
{
    public string Street { get; }
    public string District { get; }
    public string City { get; }

    public Address(string street, string district, string city)
    {
        Street = street;
        District = district;
        City = city;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return District;
        yield return City;
    }

    public override string ToString() => $"{Street}, {District}, {City}";
}
