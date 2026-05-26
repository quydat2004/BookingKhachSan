using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Application.DTOs.Hotels;
using System.Collections.Generic;
using AutoMapper;

namespace BookingHotel.Application.Features.Hotels.Queries.GetPendingHotels;

public record GetPendingHotelsQuery : IRequest<Result<IEnumerable<HotelDto>>>;

public class GetPendingHotelsQueryHandler : IRequestHandler<GetPendingHotelsQuery, Result<IEnumerable<HotelDto>>>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IMapper _mapper;

    public GetPendingHotelsQueryHandler(IHotelRepository hotelRepo, IMapper mapper)
    {
        _hotelRepo = hotelRepo;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<HotelDto>>> Handle(GetPendingHotelsQuery request, CancellationToken ct)
    {
        var hotels = await _hotelRepo.GetPendingAsync();
        return Result<IEnumerable<HotelDto>>.Success(_mapper.Map<IEnumerable<HotelDto>>(hotels));
    }
}
