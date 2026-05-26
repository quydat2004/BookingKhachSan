using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Bookings;

namespace BookingHotel.Application.Features.Bookings.Queries.GetManagerBookings;

public record GetManagerBookingsQuery : IRequest<Result<IEnumerable<ManagerBookingDto>>>
{
    public int ManagerId { get; init; }
    public string? Status { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}

public class GetManagerBookingsQueryHandler : IRequestHandler<GetManagerBookingsQuery, Result<IEnumerable<ManagerBookingDto>>>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly IMapper _mapper;

    public GetManagerBookingsQueryHandler(IHotelRepository hotelRepo, IBookingRepository bookingRepo, IMapper mapper)
    {
        _hotelRepo = hotelRepo;
        _bookingRepo = bookingRepo;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ManagerBookingDto>>> Handle(GetManagerBookingsQuery request, CancellationToken ct)
    {
        var hotels = await _hotelRepo.GetByManagerIdAsync(request.ManagerId);
        var hotelIds = hotels.Select(h => h.Id).ToList();
        if (hotelIds.Count == 0)
            return Result<IEnumerable<ManagerBookingDto>>.Success(Array.Empty<ManagerBookingDto>());

        var bookings = await _bookingRepo.GetByHotelIdsAsync(hotelIds, request.Status, request.FromDate, request.ToDate);
        return Result<IEnumerable<ManagerBookingDto>>.Success(_mapper.Map<IEnumerable<ManagerBookingDto>>(bookings));
    }
}
