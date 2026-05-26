using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Bookings;

namespace BookingHotel.Application.Features.Bookings.Queries.GetBookingById;

public record GetBookingByIdQuery : IRequest<Result<BookingDto>>
{
    public int Id { get; init; }
}

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IMapper _mapper;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepo, IMapper mapper)
    {
        _bookingRepo = bookingRepo;
        _mapper = mapper;
    }

    public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.Id);
        if (booking is null)
            return Result<BookingDto>.Failure("Booking not found", "NOT_FOUND");

        return Result<BookingDto>.Success(_mapper.Map<BookingDto>(booking));
    }
}
