using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.DTOs.Vouchers;

namespace BookingHotel.Application.Features.Vouchers.Commands.ApplyVoucher;

public record ApplyVoucherCommand : IRequest<Result<ApplyVoucherResult>>
{
    public string VoucherCode { get; init; } = string.Empty;
    public decimal OrderAmount { get; init; }
}

public class ApplyVoucherCommandHandler : IRequestHandler<ApplyVoucherCommand, Result<ApplyVoucherResult>>
{
    private readonly IVoucherRepository _voucherRepo;

    public ApplyVoucherCommandHandler(IVoucherRepository voucherRepo) => _voucherRepo = voucherRepo;

    public async Task<Result<ApplyVoucherResult>> Handle(ApplyVoucherCommand request, CancellationToken ct)
    {
        var voucher = await _voucherRepo.GetByCodeAsync(request.VoucherCode);
        if (voucher is null)
            return Result<ApplyVoucherResult>.Success(new ApplyVoucherResult
            {
                IsValid = false,
                ErrorMessage = "Voucher not found"
            });

        if (!voucher.IsActive || voucher.ValidFrom > DateTime.UtcNow || voucher.ValidTo < DateTime.UtcNow)
            return Result<ApplyVoucherResult>.Success(new ApplyVoucherResult
            {
                IsValid = false,
                ErrorMessage = "Voucher is expired or inactive"
            });

        if (voucher.UsageLimit is not null && voucher.UsedCount >= voucher.UsageLimit)
            return Result<ApplyVoucherResult>.Success(new ApplyVoucherResult
            {
                IsValid = false,
                ErrorMessage = "Voucher usage limit reached"
            });

        if (voucher.MinOrderAmount is not null && request.OrderAmount < voucher.MinOrderAmount)
            return Result<ApplyVoucherResult>.Success(new ApplyVoucherResult
            {
                IsValid = false,
                ErrorMessage = $"Minimum order amount is {voucher.MinOrderAmount:N0}"
            });

        var discountAmount = voucher.DiscountType == "Percentage"
            ? Math.Min(request.OrderAmount * voucher.DiscountValue / 100, voucher.MaxDiscount ?? request.OrderAmount)
            : Math.Min(voucher.DiscountValue, voucher.MaxDiscount ?? voucher.DiscountValue);

        return Result<ApplyVoucherResult>.Success(new ApplyVoucherResult
        {
            IsValid = true,
            DiscountAmount = discountAmount,
            FinalAmount = request.OrderAmount - discountAmount,
            Voucher = new VoucherDto
            {
                Id = voucher.Id,
                VoucherCode = voucher.VoucherCode,
                DiscountType = voucher.DiscountType,
                DiscountValue = voucher.DiscountValue,
                MinOrderAmount = voucher.MinOrderAmount,
                MaxDiscount = voucher.MaxDiscount
            }
        });
    }
}
