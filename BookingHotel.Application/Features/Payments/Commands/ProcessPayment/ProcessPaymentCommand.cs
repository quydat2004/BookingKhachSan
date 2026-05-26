using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Enums;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.Interfaces.Services;
using AutoMapper;
using BookingHotel.Application.DTOs.Payments;

namespace BookingHotel.Application.Features.Payments.Commands.ProcessPayment;

public record ProcessPaymentCommand : IRequest<Result<PaymentDto>>
{
    public int BookingId { get; init; }
    public int UserId { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public string? ReturnUrl { get; init; }
    public string? IpAddress { get; init; }
}

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result<PaymentDto>>
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IPaymentRepository _paymentRepo;
    private readonly IVNPayService _vnpayService;
    private readonly IMoMoService _momoService;
    private readonly IMapper _mapper;

    public ProcessPaymentCommandHandler(
        IBookingRepository bookingRepo, 
        IPaymentRepository paymentRepo, 
        IVNPayService vnpayService,
        IMoMoService momoService,
        IMapper mapper)
    {
        _bookingRepo = bookingRepo;
        _paymentRepo = paymentRepo;
        _vnpayService = vnpayService;
        _momoService = momoService;
        _mapper = mapper;
    }

    public async Task<Result<PaymentDto>> Handle(ProcessPaymentCommand request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.BookingId);
        if (booking is null)
            return Result<PaymentDto>.Failure("Booking not found", "NOT_FOUND");

        if (booking.Status != "Pending")
            return Result<PaymentDto>.Failure("Booking is not pending payment", "INVALID_STATUS");

        var payment = new Payment
        {
            BookingId = request.BookingId,
            UserId = request.UserId,
            Amount = booking.TotalAmount,
            PaymentMethod = request.PaymentMethod,
            PaymentStatus = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepo.CreateAsync(payment);

        var paymentDto = _mapper.Map<PaymentDto>(payment);

        if (request.PaymentMethod == "VNPay")
        {
            paymentDto.PaymentUrl = _vnpayService.CreatePaymentUrl(
                booking.TotalAmount, 
                booking.BookingCode, 
                request.ReturnUrl ?? "", 
                request.IpAddress ?? "127.0.0.1");
        }
        else if (request.PaymentMethod == "MoMo")
        {
            paymentDto.PaymentUrl = _momoService.CreatePaymentUrl(
                booking.TotalAmount, 
                booking.BookingCode, 
                request.ReturnUrl ?? "");
        }

        return Result<PaymentDto>.Success(paymentDto);
    }
}
