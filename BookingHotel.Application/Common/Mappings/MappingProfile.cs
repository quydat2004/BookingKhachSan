using AutoMapper;

namespace BookingHotel.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Role, DTOs.Auth.RoleDto>();
        CreateMap<Domain.Entities.User, DTOs.Auth.UserDto>();
        CreateMap<Domain.Entities.User, DTOs.Auth.ProfileDto>();

        CreateMap<Domain.Entities.Hotel, DTOs.Hotels.HotelDto>()
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : ""))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.HotelAmenities.Select(ha => ha.Amenity)));
        CreateMap<Domain.Entities.Hotel, DTOs.Hotels.HotelDetailDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl)))
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.HotelAmenities.Select(ha => ha.Amenity)));
        CreateMap<Domain.Entities.Amenity, DTOs.Hotels.HotelAmenityDto>();
        CreateMap<Domain.Entities.RoomType, DTOs.Hotels.RoomTypeSummaryDto>()
            .ForMember(dest => dest.AvailableRooms, opt => opt.MapFrom(src => src.Rooms.Count(r => r.IsActive && r.Status == "Available")));

        CreateMap<Domain.Entities.RoomType, DTOs.Rooms.RoomTypeDto>();
        CreateMap<Domain.Entities.Room, DTOs.Rooms.RoomDto>();

        CreateMap<Domain.Entities.Booking, DTOs.Bookings.BookingDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.HotelName))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.BookingDetails))
            .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));
        CreateMap<Domain.Entities.Booking, DTOs.Bookings.BookingSummaryDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.HotelName))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Hotel.City));
        CreateMap<Domain.Entities.Booking, DTOs.Bookings.ManagerBookingDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.User.Phone))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.HotelName))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => string.Join(", ", src.BookingDetails.Select(d => d.Room.RoomNumber))))
            .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => string.Join(", ", src.BookingDetails.Select(d => d.Room.RoomType.TypeName).Distinct())));
        CreateMap<Domain.Entities.BookingDetail, DTOs.Bookings.BookingDetailDto>()
            .ForMember(dest => dest.DetailId, opt => opt.MapFrom(src => src.DetailId))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
            .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.Room.RoomType.TypeName));
        CreateMap<Domain.Entities.Payment, DTOs.Bookings.BookingPaymentDto>();

        CreateMap<Domain.Entities.Payment, DTOs.Payments.PaymentDto>();

        CreateMap<Domain.Entities.Review, DTOs.Reviews.ReviewDto>();

        CreateMap<Domain.Entities.Voucher, DTOs.Vouchers.VoucherDto>();
    }
}
