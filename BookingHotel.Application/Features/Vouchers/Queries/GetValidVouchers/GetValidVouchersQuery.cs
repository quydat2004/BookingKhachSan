using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Vouchers;

namespace BookingHotel.Application.Features.Vouchers.Queries.GetValidVouchers;

public record GetValidVouchersQuery : IRequest<Result<IEnumerable<VoucherDto>>>
{
    public decimal OrderAmount { get; init; }
}

public class GetValidVouchersQueryHandler : IRequestHandler<GetValidVouchersQuery, Result<IEnumerable<VoucherDto>>>
{
    private readonly IVoucherRepository _voucherRepo;
    private readonly IMapper _mapper;

    public GetValidVouchersQueryHandler(IVoucherRepository voucherRepo, IMapper mapper)
    {
        _voucherRepo = voucherRepo;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<VoucherDto>>> Handle(GetValidVouchersQuery request, CancellationToken ct)
    {
        var vouchers = await _voucherRepo.GetValidVouchersAsync(request.OrderAmount);
        return Result<IEnumerable<VoucherDto>>.Success(_mapper.Map<IEnumerable<VoucherDto>>(vouchers));
    }
}
