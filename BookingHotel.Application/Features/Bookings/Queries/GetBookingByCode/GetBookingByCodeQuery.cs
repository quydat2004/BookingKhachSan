using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Bookings;

namespace BookingHotel.Application.Features.Bookings.Queries.GetBookingByCode;

public record GetBookingByCodeQuery : IRequest<Result<BookingDto>>
{
    public string BookingCode { get; init; } = string.Empty;
}

public class GetBookingByCodeQueryHandler : IRequestHandler<GetBookingByCodeQuery, Result<BookingDto>>
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IMapper _mapper;

    public GetBookingByCodeQueryHandler(IBookingRepository bookingRepo, IMapper mapper)
    {
        _bookingRepo = bookingRepo;
        _mapper = mapper;
    }

    public async Task<Result<BookingDto>> Handle(GetBookingByCodeQuery request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByCodeAsync(request.BookingCode);
        if (booking is null)
            return Result<BookingDto>.Failure("Booking not found", "NOT_FOUND");

        return Result<BookingDto>.Success(_mapper.Map<BookingDto>(booking));
    }
}
