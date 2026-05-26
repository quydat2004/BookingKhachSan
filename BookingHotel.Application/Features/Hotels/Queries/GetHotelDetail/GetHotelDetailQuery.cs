using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Hotels;

namespace BookingHotel.Application.Features.Hotels.Queries.GetHotelDetail;

public record GetHotelDetailQuery : IRequest<Result<HotelDetailDto>>
{
    public int Id { get; init; }
}

public class GetHotelDetailQueryHandler : IRequestHandler<GetHotelDetailQuery, Result<HotelDetailDto>>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IMapper _mapper;

    public GetHotelDetailQueryHandler(IHotelRepository hotelRepo, IMapper mapper)
    {
        _hotelRepo = hotelRepo;
        _mapper = mapper;
    }

    public async Task<Result<HotelDetailDto>> Handle(GetHotelDetailQuery request, CancellationToken ct)
    {
        var hotel = await _hotelRepo.GetByIdAsync(request.Id);
        if (hotel is null)
            return Result<HotelDetailDto>.Failure("Hotel not found", "NOT_FOUND");

        return Result<HotelDetailDto>.Success(_mapper.Map<HotelDetailDto>(hotel));
    }
}
