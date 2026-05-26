using BookingHotel.Application.DTOs.Auth;

namespace BookingHotel.Application.Interfaces.Services;

public interface IJwtService
{
    TokenDto GenerateToken(int userId, string email, string role);
    string GenerateRefreshToken();
    int? ValidateToken(string token);
}

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendBookingConfirmationAsync(string email, string customerName, string bookingCode, string hotelName, DateTime checkIn, DateTime checkOut, decimal amount);
    Task SendPaymentSuccessAsync(string email, string customerName, string bookingCode, decimal amount, string transactionId);
}

public interface IPaymentService
{
    Task<string> CreatePaymentUrlAsync(int bookingId, decimal amount, string returnUrl);
    Task<bool> ProcessPaymentCallbackAsync(string transactionId, string responseCode, string rawResponse);
    Task<decimal> RefundAsync(int paymentId);
}

public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}

public interface IFileService
{
    Task<string> SaveFileAsync(Microsoft.AspNetCore.Http.IFormFile file, string folderName);
    void DeleteFile(string fileName);
}

public interface IVNPayService
{
    string CreatePaymentUrl(decimal amount, string bookingCode, string returnUrl, string ipAddress);
}

public interface IMoMoService
{
    string CreatePaymentUrl(decimal amount, string bookingCode, string returnUrl);
}
