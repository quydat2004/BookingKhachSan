using Microsoft.EntityFrameworkCore;
using BookingHotel.Domain.Entities;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Infrastructure.Data;

namespace BookingHotel.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly ApplicationDbContext _db;

    public HotelRepository(ApplicationDbContext db) => _db = db;

    public async Task<Hotel?> GetByIdAsync(int id) =>
        await _db.Hotels.Include(h => h.Manager).Include(h => h.RoomTypes).Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.Id == id);

    public async Task<IEnumerable<Hotel>> GetAllAsync() =>
        await _db.Hotels.Where(h => h.IsActive).OrderByDescending(h => h.CreatedAt).ToListAsync();

    public async Task<IEnumerable<Hotel>> SearchAsync(string? city, string? keyword, byte? minRating, int page, int pageSize)
    {
        var query = _db.Hotels.Where(h => h.IsActive && h.Status == "Approved").AsQueryable();
        if (!string.IsNullOrEmpty(city))
            query = query.Where(h => h.City.Contains(city));
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(h => h.HotelName.Contains(keyword) || h.Address.Contains(keyword));
        if (minRating.HasValue)
            query = query.Where(h => h.StarRating >= minRating.Value);
        return await query.OrderByDescending(h => h.StarRating).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? city, string? keyword, byte? minRating)
    {
        var query = _db.Hotels.Where(h => h.IsActive && h.Status == "Approved").AsQueryable();
        if (!string.IsNullOrEmpty(city))
            query = query.Where(h => h.City.Contains(city));
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(h => h.HotelName.Contains(keyword) || h.Address.Contains(keyword));
        if (minRating.HasValue)
            query = query.Where(h => h.StarRating >= minRating.Value);
        return await query.CountAsync();
    }

    public async Task<IEnumerable<Hotel>> GetByManagerIdAsync(int managerId) =>
        await _db.Hotels.Where(h => h.ManagerId == managerId).ToListAsync();

    public async Task<IEnumerable<Hotel>> GetPendingAsync() =>
        await _db.Hotels
            .Include(h => h.Manager)
            .Include(h => h.HotelAmenities)
                .ThenInclude(ha => ha.Amenity)
            .Where(h => h.Status == "Pending")
            .ToListAsync();

    public async Task<Hotel> CreateAsync(Hotel hotel)
    {
        await _db.Hotels.AddAsync(hotel);
        await _db.SaveChangesAsync();
        return hotel;
    }

    public void Update(Hotel hotel) => _db.Hotels.Update(hotel);
    public void Delete(Hotel hotel) => _db.Hotels.Remove(hotel);
}
