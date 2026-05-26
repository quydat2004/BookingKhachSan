using BookingHotel.Domain.Entities;

namespace BookingHotel.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<User> CreateAsync(User user);
    void Update(User user);
}

public interface IHotelRepository
{
    Task<Hotel?> GetByIdAsync(int id);
    Task<IEnumerable<Hotel>> GetAllAsync();
    Task<IEnumerable<Hotel>> SearchAsync(string? city, string? keyword, byte? minRating, int page, int pageSize);
    Task<int> GetTotalCountAsync(string? city, string? keyword, byte? minRating);
    Task<IEnumerable<Hotel>> GetByManagerIdAsync(int managerId);
    Task<IEnumerable<Hotel>> GetPendingAsync();
    Task<Hotel> CreateAsync(Hotel hotel);
    void Update(Hotel hotel);
    void Delete(Hotel hotel);
}

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(int id);
    Task<Booking?> GetByCodeAsync(string code);
    Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Booking>> GetByHotelIdAsync(int hotelId);
    Task<IEnumerable<Booking>> GetByHotelIdsAsync(IEnumerable<int> hotelIds, string? status, DateTime? fromDate, DateTime? toDate);
    Task<Booking> CreateAsync(Booking booking);
    void Update(Booking booking);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
}

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(int id);
    Task<IEnumerable<Room>> GetByHotelIdAsync(int hotelId);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int? roomTypeId = null);
    Task<Room> CreateAsync(Room room);
    void Update(Room room);
}

public interface IRoomTypeRepository
{
    Task<RoomType?> GetByIdAsync(int id);
    Task<IEnumerable<RoomType>> GetByHotelIdAsync(int hotelId);
    Task<RoomType> CreateAsync(RoomType roomType);
    void Update(RoomType roomType);
}

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);
    Task<IEnumerable<Payment>> GetByBookingIdAsync(int bookingId);
    Task<Payment?> GetByTransactionIdAsync(string transactionId);
    Task<Payment> CreateAsync(Payment payment);
    void Update(Payment payment);
}

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int id);
    Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId, bool? approved = null);
    Task<IEnumerable<Review>> GetPendingAsync();
    Task<bool> ExistsForBookingAsync(int bookingId);
    Task<Review> CreateAsync(Review review);
    void Update(Review review);
}

public interface IVoucherRepository
{
    Task<Voucher?> GetByIdAsync(int id);
    Task<Voucher?> GetByCodeAsync(string code);
    Task<IEnumerable<Voucher>> GetValidVouchersAsync(decimal orderAmount);
    Task<Voucher> CreateAsync(Voucher voucher);
    void Update(Voucher voucher);
}
