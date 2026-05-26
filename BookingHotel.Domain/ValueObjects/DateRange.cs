namespace BookingHotel.Domain.ValueObjects;

public class DateRange : Common.ValueObject
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public int Nights => (int)(EndDate - StartDate).TotalDays;

    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException("EndDate must be after StartDate");
        StartDate = startDate;
        EndDate = endDate;
    }

    public bool Overlaps(DateRange other) =>
        StartDate < other.EndDate && EndDate > other.StartDate;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
