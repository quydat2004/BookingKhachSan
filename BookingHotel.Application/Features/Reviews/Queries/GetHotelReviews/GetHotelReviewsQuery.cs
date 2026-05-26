using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Reviews;

namespace BookingHotel.Application.Features.Reviews.Queries.GetHotelReviews;

public record GetHotelReviewsQuery : IRequest<Result<IEnumerable<ReviewDto>>>
{
    public int HotelId { get; init; }
    public bool? ApprovedOnly { get; init; } = true;
}

public class GetHotelReviewsQueryHandler : IRequestHandler<GetHotelReviewsQuery, Result<IEnumerable<ReviewDto>>>
{
    private readonly IReviewRepository _reviewRepo;
    private readonly IMapper _mapper;

    public GetHotelReviewsQueryHandler(IReviewRepository reviewRepo, IMapper mapper)
    {
        _reviewRepo = reviewRepo;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ReviewDto>>> Handle(GetHotelReviewsQuery request, CancellationToken ct)
    {
        var reviews = await _reviewRepo.GetByHotelIdAsync(request.HotelId, request.ApprovedOnly);
        return Result<IEnumerable<ReviewDto>>.Success(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
    }
}
