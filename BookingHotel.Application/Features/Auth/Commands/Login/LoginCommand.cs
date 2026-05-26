using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.Interfaces.Services;
using BookingHotel.Application.DTOs.Auth;

namespace BookingHotel.Application.Features.Auth.Commands.Login;

public record LoginCommand : IRequest<Result<TokenDto>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenDto>>
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userRepo.GetByEmailAsync(request.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<TokenDto>.Failure("Invalid email or password", "INVALID_CREDENTIALS");

        if (!user.IsActive)
            return Result<TokenDto>.Failure("Account is disabled", "ACCOUNT_DISABLED");

        var jwtToken = _jwtService.GenerateToken(user.Id, user.Email, user.Role.RoleName);
        return Result<TokenDto>.Success(new TokenDto
        {
            AccessToken = jwtToken.AccessToken,
            RefreshToken = jwtToken.RefreshToken,
            ExpiresAt = jwtToken.ExpiresAt,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.RoleName
        });
    }
}
