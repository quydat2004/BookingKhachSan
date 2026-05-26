using MediatR;
using BookingHotel.Domain.Common;
using AutoMapper;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.DTOs.Auth;

namespace BookingHotel.Application.Features.Auth.Queries.GetProfile;

public record GetProfileQuery : IRequest<Result<ProfileDto>>
{
    public int UserId { get; init; }
}

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileDto>>
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public GetProfileQueryHandler(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<Result<ProfileDto>> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId);
        if (user is null)
            return Result<ProfileDto>.Failure("User not found", "NOT_FOUND");

        return Result<ProfileDto>.Success(_mapper.Map<ProfileDto>(user));
    }
}
