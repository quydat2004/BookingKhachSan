namespace BookingHotel.Domain.ValueObjects;

public class ContactInfo : Common.ValueObject
{
    public string? Phone { get; }
    public string? Email { get; }
    public string? Website { get; }

    public ContactInfo(string? phone, string? email, string? website)
    {
        Phone = phone;
        Email = email;
        Website = website;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Phone ?? string.Empty;
        yield return Email ?? string.Empty;
        yield return Website ?? string.Empty;
    }
}
