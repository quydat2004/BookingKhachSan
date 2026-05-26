using ErrorModel = BookingHotel.Application.Common.Models.Error;

namespace BookingHotel.Application.Common.Models;

public class Error
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public Error() { }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error Validation(string message) => new("VALIDATION_ERROR", message);
    public static Error NotFound(string message) => new("NOT_FOUND", message);
    public static Error Unauthorized(string message = "Unauthorized") => new("UNAUTHORIZED", message);
    public static Error Forbidden(string message = "Forbidden") => new("FORBIDDEN", message);
    public static Error Conflict(string message) => new("CONFLICT", message);
    public static Error Internal(string message = "Internal server error") => new("INTERNAL_ERROR", message);
}
