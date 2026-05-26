using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Payments;

namespace BookingHotel.Application.Features.Payments.Queries.GetPaymentStatus;

public record GetPaymentStatusQuery : IRequest<Result<IEnumerable<PaymentDto>>>
{
    public int BookingId { get; init; }
}

public class GetPaymentStatusQueryHandler : IRequestHandler<GetPaymentStatusQuery, Result<IEnumerable<PaymentDto>>>
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IMapper _mapper;

    public GetPaymentStatusQueryHandler(IPaymentRepository paymentRepo, IMapper mapper)
    {
        _paymentRepo = paymentRepo;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<PaymentDto>>> Handle(GetPaymentStatusQuery request, CancellationToken ct)
    {
        var payments = await _paymentRepo.GetByBookingIdAsync(request.BookingId);
        return Result<IEnumerable<PaymentDto>>.Success(_mapper.Map<IEnumerable<PaymentDto>>(payments));
    }
}
