namespace BookingHotel.Domain.Exceptions;

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message = "Authentication is required.")
        : base(message) { }
}
