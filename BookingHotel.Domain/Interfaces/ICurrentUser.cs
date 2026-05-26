namespace BookingHotel.Domain.Interfaces;

public interface ICurrentUser
{
    int? UserId { get; }
    string? Email { get; }
    string[] Roles { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}
