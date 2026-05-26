using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Enums;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Reviews;

namespace BookingHotel.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand : IRequest<Result<ReviewDto>>
{
    public int UserId { get; init; }
    public int HotelId { get; init; }
    public int BookingId { get; init; }
    public byte Rating { get; init; }
    public string? Title { get; init; }
    public string? Comment { get; init; }
    public byte? StaffRating { get; init; }
    public byte? CleanlinessRating { get; init; }
    public byte? ComfortRating { get; init; }
    public byte? LocationRating { get; init; }
    public byte? ValueRating { get; init; }
    public bool IsAnonymous { get; init; }
}

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
{
    private readonly IReviewRepository _reviewRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IReviewRepository reviewRepo, IBookingRepository bookingRepo, IMapper mapper)
    {
        _reviewRepo = reviewRepo;
        _bookingRepo = bookingRepo;
        _mapper = mapper;
    }

    public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.BookingId);
        if (booking is null || booking.UserId != request.UserId)
            return Result<ReviewDto>.Failure("Invalid booking", "INVALID_BOOKING");

        if (booking.Status != "CheckedOut")
            return Result<ReviewDto>.Failure("Only checked-out bookings can be reviewed", "INVALID_STATUS");

        if (await _reviewRepo.ExistsForBookingAsync(request.BookingId))
            return Result<ReviewDto>.Failure("Booking has already been reviewed", "DUPLICATE_REVIEW");

        var review = new Review
        {
            UserId = request.UserId,
            HotelId = request.HotelId,
            BookingId = request.BookingId,
            Rating = request.Rating,
            Title = request.Title,
            Comment = request.Comment,
            StaffRating = request.StaffRating,
            CleanlinessRating = request.CleanlinessRating,
            ComfortRating = request.ComfortRating,
            LocationRating = request.LocationRating,
            ValueRating = request.ValueRating,
            IsAnonymous = request.IsAnonymous
        };

        await _reviewRepo.CreateAsync(review);
        return Result<ReviewDto>.Success(_mapper.Map<ReviewDto>(review));
    }
}
