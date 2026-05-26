using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;

namespace BookingHotel.Application.Features.Auth.Commands.ChangePassword;

public record ChangePasswordCommand : IRequest<Result<bool>>
{
    public int UserId { get; init; }
    public string CurrentPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly IUserRepository _userRepo;

    public ChangePasswordCommandHandler(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId);
        if (user is null)
            return Result<bool>.Failure("User not found", "USER_NOT_FOUND");

        // Verify current password
        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return Result<bool>.Failure("Current password is incorrect", "INVALID_CURRENT_PASSWORD");

        // Check if new password is same as current
        if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
            return Result<bool>.Failure("New password must be different from current password", "SAME_PASSWORD");

        // Update password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        _userRepo.Update(user);

        return Result<bool>.Success(true);
    }
}
