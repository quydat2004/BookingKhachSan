using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.Interfaces.Services;
using System.Linq;

namespace BookingHotel.Application.Features.Payments.Commands.UpdatePaymentStatus;

public record UpdatePaymentStatusCommand : IRequest<Result<bool>>
{
    public string TransactionId { get; init; } = string.Empty;
    public string BookingCode { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? PaymentGateway { get; init; }
}

public class UpdatePaymentStatusCommandHandler : IRequestHandler<UpdatePaymentStatusCommand, Result<bool>>
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly IUserRepository _userRepo;
    private readonly IEmailService _emailService;

    public UpdatePaymentStatusCommandHandler(
        IPaymentRepository paymentRepo, 
        IBookingRepository bookingRepo,
        IUserRepository userRepo,
        IEmailService emailService)
    {
        _paymentRepo = paymentRepo;
        _bookingRepo = bookingRepo;
        _userRepo = userRepo;
        _emailService = emailService;
    }

    public async Task<Result<bool>> Handle(UpdatePaymentStatusCommand request, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByCodeAsync(request.BookingCode);
        if (booking == null) return Result<bool>.Failure("Booking not found");

        var payments = await _paymentRepo.GetByBookingIdAsync(booking.Id);
        var payment = payments.OrderByDescending(p => p.CreatedAt).FirstOrDefault();

        if (payment == null) return Result<bool>.Failure("Payment record not found");

        payment.TransactionId = request.TransactionId;
        payment.PaymentStatus = request.Status;
        payment.PaymentGateway = request.PaymentGateway;
        payment.PaymentDate = DateTime.UtcNow;

        if (request.Status == "Success")
        {
            booking.Status = "Confirmed";
            
            // Send email notification
            var user = await _userRepo.GetByIdAsync(booking.UserId);
            if (user != null)
            {
                await _emailService.SendPaymentSuccessAsync(
                    user.Email, 
                    user.FullName, 
                    booking.BookingCode, 
                    booking.TotalAmount, 
                    request.TransactionId);
            }
        }
        else if (request.Status == "Failed")
        {
            // Keep pending or handle cancellation
        }

        _paymentRepo.Update(payment);
        _bookingRepo.Update(booking);

        return Result<bool>.Success(true);
    }
}
