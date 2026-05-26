using Microsoft.EntityFrameworkCore;
using BookingHotel.Domain.Entities;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Infrastructure.Data;

namespace BookingHotel.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(int id) =>
        await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email) =>
        await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> EmailExistsAsync(string email) =>
        await _db.Users.AnyAsync(u => u.Email == email);

    public async Task<User> CreateAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public void Update(User user) => _db.Users.Update(user);
}

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _db;

    public BookingRepository(ApplicationDbContext db) => _db = db;

    public async Task<Booking?> GetByIdAsync(int id) =>
        await IncludeBookingDetails(_db.Bookings).FirstOrDefaultAsync(b => b.Id == id);

    public async Task<Booking?> GetByCodeAsync(string code) =>
        await IncludeBookingDetails(_db.Bookings).FirstOrDefaultAsync(b => b.BookingCode == code);

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId) =>
        await IncludeBookingDetails(_db.Bookings).Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt).ToListAsync();

    public async Task<IEnumerable<Booking>> GetByHotelIdAsync(int hotelId) =>
        await IncludeBookingDetails(_db.Bookings).Where(b => b.HotelId == hotelId).ToListAsync();

    public async Task<IEnumerable<Booking>> GetByHotelIdsAsync(IEnumerable<int> hotelIds, string? status, DateTime? fromDate, DateTime? toDate)
    {
        var ids = hotelIds.ToList();
        var query = IncludeBookingDetails(_db.Bookings).Where(b => ids.Contains(b.HotelId));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(b => b.Status == status);

        if (fromDate.HasValue)
            query = query.Where(b => b.CheckInDate >= fromDate.Value.Date);

        if (toDate.HasValue)
            query = query.Where(b => b.CheckOutDate <= toDate.Value.Date.AddDays(1).AddTicks(-1));

        return await query.OrderByDescending(b => b.CreatedAt).ToListAsync();
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        await _db.Bookings.AddAsync(booking);
        await _db.SaveChangesAsync();
        return booking;
    }

    public void Update(Booking booking) => _db.Bookings.Update(booking);

    private static IQueryable<Booking> IncludeBookingDetails(IQueryable<Booking> query) =>
        query.Include(b => b.User)
            .Include(b => b.Hotel)
            .Include(b => b.Payments)
            .Include(b => b.BookingDetails).ThenInclude(d => d.Room).ThenInclude(r => r.RoomType)
            .Include(b => b.Reviews);

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut) =>
        !await _db.BookingDetails.AnyAsync(bd =>
            bd.RoomId == roomId &&
            (bd.Booking.Status == "Confirmed" || bd.Booking.Status == "CheckedIn") &&
            bd.Booking.CheckInDate < checkOut &&
            bd.Booking.CheckOutDate > checkIn);
}
