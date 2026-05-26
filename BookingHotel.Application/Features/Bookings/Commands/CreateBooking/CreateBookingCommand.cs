using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Enums;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.Interfaces.Services;
using AutoMapper;
using BookingHotel.Application.DTOs.Bookings;

namespace BookingHotel.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand : IRequest<Result<BookingDto>>
{
    public int UserId { get; init; }
    public int HotelId { get; init; }
    public List<int> RoomIds { get; init; } = new();
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public int NumberOfGuests { get; init; }
    public string? SpecialRequests { get; init; }
    public string? VoucherCode { get; init; }
}

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<BookingDto>>
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IRoomRepository _roomRepo;
    private readonly IHotelRepository _hotelRepo;
    private readonly IVoucherRepository _voucherRepo;
    private readonly IMapper _mapper;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepo,
        IRoomRepository roomRepo,
        IHotelRepository hotelRepo,
        IVoucherRepository voucherRepo,
        IMapper mapper)
    {
        _bookingRepo = bookingRepo;
        _roomRepo = roomRepo;
        _hotelRepo = hotelRepo;
        _voucherRepo = voucherRepo;
        _mapper = mapper;
    }

    public async Task<Result<BookingDto>> Handle(CreateBookingCommand request, CancellationToken ct)
    {
        var hotel = await _hotelRepo.GetByIdAsync(request.HotelId);
        if (hotel is null || hotel.Status != "Approved")
            return Result<BookingDto>.Failure("Hotel not available", "HOTEL_UNAVAILABLE");

        decimal subTotal = 0;
        var details = new List<BookingDetail>();

        foreach (var roomId in request.RoomIds)
        {
            var available = await _bookingRepo.IsRoomAvailableAsync(roomId, request.CheckInDate, request.CheckOutDate);
            if (!available)
                return Result<BookingDto>.Failure($"Room {roomId} is not available", "ROOM_UNAVAILABLE");

            var room = await _roomRepo.GetByIdAsync(roomId);
            if (room is null)
                return Result<BookingDto>.Failure($"Room {roomId} not found", "NOT_FOUND");

            var nights = (int)(request.CheckOutDate - request.CheckInDate).TotalDays;
            
            decimal roomSubTotal = 0;
            for (int i = 0; i < nights; i++)
            {
                var date = request.CheckInDate.AddDays(i);
                decimal price = room.RoomType.Price;
                
                // Dynamic Pricing: Weekends (Friday and Saturday nights)
                if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    price = room.RoomType.WeekendPrice ?? price;
                }
                
                // TODO: Add Holiday checking logic if needed
                
                roomSubTotal += price;
            }

            details.Add(new BookingDetail
            {
                RoomId = roomId,
                RoomTypeId = room.RoomTypeId,
                UnitPrice = room.RoomType.Price, // Keeping base price as unit price for summary
                Quantity = nights,
                SubTotal = roomSubTotal
            });
            subTotal += roomSubTotal;
        }

        decimal discountAmount = 0;
        Voucher? appliedVoucher = null;
        if (!string.IsNullOrEmpty(request.VoucherCode))
        {
            var voucher = await _voucherRepo.GetByCodeAsync(request.VoucherCode);
            if (voucher is not null && voucher.IsActive && voucher.ValidFrom <= DateTime.UtcNow && voucher.ValidTo >= DateTime.UtcNow
                && (voucher.UsageLimit is null || voucher.UsedCount < voucher.UsageLimit)
                && (voucher.MinOrderAmount is null || subTotal >= voucher.MinOrderAmount))
            {
                appliedVoucher = voucher;
                discountAmount = voucher.DiscountType == "Percentage"
                    ? Math.Min(subTotal * voucher.DiscountValue / 100, voucher.MaxDiscount ?? subTotal)
                    : Math.Min(voucher.DiscountValue, voucher.MaxDiscount ?? voucher.DiscountValue);
            }
        }

        var taxAmount = (subTotal - discountAmount) * 0.1m;
        var totalAmount = subTotal - discountAmount + taxAmount;

        var booking = new Booking
        {
            BookingCode = $"BK-{request.HotelId:D3}{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1, 9999):D4}",
            UserId = request.UserId,
            HotelId = request.HotelId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            NumberOfGuests = request.NumberOfGuests,
            SpecialRequests = request.SpecialRequests,
            SubTotal = subTotal,
            DiscountAmount = discountAmount,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            Status = "Pending"
        };

        foreach (var d in details)
            booking.BookingDetails.Add(d);

        // Record Voucher usage
        if (appliedVoucher != null)
        {
            booking.BookingVouchers.Add(new BookingVoucher
            {
                VoucherId = appliedVoucher.Id,
                DiscountAmount = discountAmount
            });
            
            appliedVoucher.UsedCount++;
            _voucherRepo.Update(appliedVoucher);
        }

        await _bookingRepo.CreateAsync(booking);
        return Result<BookingDto>.Success(_mapper.Map<BookingDto>(booking));
    }
}
