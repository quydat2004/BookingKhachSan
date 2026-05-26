using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Bookings;

namespace BookingHotel.Application.Features.Bookings.Queries.GetBookingHistory;

public record GetBookingHistoryQuery : IRequest<Result<IEnumerable<BookingSummaryDto>>>
{
    public int UserId { get; init; }
}

public class GetBookingHistoryQueryHandler : IRequestHandler<GetBookingHistoryQuery, Result<IEnumerable<BookingSummaryDto>>>
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IMapper _mapper;

    public GetBookingHistoryQueryHandler(IBookingRepository bookingRepo, IMapper mapper)
    {
        _bookingRepo = bookingRepo;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<BookingSummaryDto>>> Handle(GetBookingHistoryQuery request, CancellationToken ct)
    {
        var bookings = await _bookingRepo.GetByUserIdAsync(request.UserId);
        return Result<IEnumerable<BookingSummaryDto>>.Success(_mapper.Map<IEnumerable<BookingSummaryDto>>(bookings));
    }
}
