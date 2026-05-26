using MediatR;
using BookingHotel.Domain.Common;
using BookingHotel.Domain.Exceptions;
using BookingHotel.Application.Interfaces.Repositories;
using AutoMapper;
using BookingHotel.Application.DTOs.Hotels;

namespace BookingHotel.Application.Features.Hotels.Queries.SearchHotels;

public record SearchHotelsQuery : IRequest<Result<IEnumerable<HotelDto>>>
{
    public string? City { get; init; }
    public string? Keyword { get; init; }
    public byte? MinRating { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class SearchHotelsQueryHandler : IRequestHandler<SearchHotelsQuery, Result<IEnumerable<HotelDto>>>
{
    private readonly IHotelRepository _hotelRepo;
    private readonly IMapper _mapper;

    public SearchHotelsQueryHandler(IHotelRepository hotelRepo, IMapper mapper)
    {
        _hotelRepo = hotelRepo;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<HotelDto>>> Handle(SearchHotelsQuery request, CancellationToken ct)
    {
        var hotels = await _hotelRepo.SearchAsync(request.City, request.Keyword, request.MinRating, request.Page, request.PageSize);
        return Result<IEnumerable<HotelDto>>.Success(_mapper.Map<IEnumerable<HotelDto>>(hotels));
    }
}
