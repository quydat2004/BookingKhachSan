using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Reports;

namespace BookingHotel.Application.Features.Reports.Queries.GetRevenueReport;

public record GetRevenueReportQuery : IRequest<Result<IEnumerable<RevenueReportDto>>>
{
    public int? Year { get; init; }
    public int? Month { get; init; }
    public int? HotelId { get; init; }
}

public class GetRevenueReportQueryHandler : IRequestHandler<GetRevenueReportQuery, Result<IEnumerable<RevenueReportDto>>>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IBookingRepository _bookingRepo;

    public GetRevenueReportQueryHandler(IHotelRepository hotelRepo, IBookingRepository bookingRepo)
    {
        _hotelRepo = hotelRepo;
        _bookingRepo = bookingRepo;
    }

    public async Task<Result<IEnumerable<RevenueReportDto>>> Handle(GetRevenueReportQuery request, CancellationToken ct)
    {
        var hotels = request.HotelId.HasValue
            ? new[] { await _hotelRepo.GetByIdAsync(request.HotelId.Value) }
            : (await _hotelRepo.GetAllAsync()).ToArray();

        var result = new List<RevenueReportDto>();
        foreach (var hotel in hotels.Where(h => h is not null))
        {
            var bookings = await _bookingRepo.GetByHotelIdAsync(hotel!.Id);
            var filtered = bookings.AsEnumerable();

            if (request.Year.HasValue)
                filtered = filtered.Where(b => b.CreatedAt.Year == request.Year.Value);
            if (request.Month.HasValue)
                filtered = filtered.Where(b => b.CreatedAt.Month == request.Month.Value);

            var list = filtered.ToList();
            result.Add(new RevenueReportDto
            {
                HotelId = hotel.Id,
                HotelName = hotel.HotelName,
                TotalBookings = list.Count,
                TotalRevenue = list.Sum(b => b.TotalAmount),
                CancelledRevenue = list.Where(b => b.Status == "Cancelled").Sum(b => b.TotalAmount),
                AvgBookingValue = list.Where(b => b.Status is not "Cancelled" and not "Pending")
                    .Select(b => (decimal?)b.TotalAmount).Average()
            });
        }

        return Result<IEnumerable<RevenueReportDto>>.Success(result);
    }
}
