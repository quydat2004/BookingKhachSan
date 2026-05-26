using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Enums;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.Interfaces.Services;
using BookingHotel.Application.DTOs.Auth;

namespace BookingHotel.Application.Features.Auth.Commands.Register;

public record RegisterCommand : IRequest<Result<TokenDto>>
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? Phone { get; init; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<TokenDto>>
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<Result<TokenDto>> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await _userRepo.EmailExistsAsync(request.Email))
            return Result<TokenDto>.Failure("Email already exists", "EMAIL_EXISTS");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Phone = request.Phone,
            RoleId = (int)UserRole.Customer,
            IsActive = true
        };

        await _userRepo.CreateAsync(user);

        var jwtToken = _jwtService.GenerateToken(user.Id, user.Email, "Customer");
        return Result<TokenDto>.Success(new TokenDto
        {
            AccessToken = jwtToken.AccessToken,
            RefreshToken = jwtToken.RefreshToken,
            ExpiresAt = jwtToken.ExpiresAt,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = "Customer"
        });
    }
}
