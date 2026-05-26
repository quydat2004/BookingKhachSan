using Microsoft.EntityFrameworkCore;
using BookingHotel.Domain.Entities;
using BookingHotel.Application.Interfaces.Repositories;
using BookingHotel.Infrastructure.Data;

namespace BookingHotel.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _db;

    public RoomRepository(ApplicationDbContext db) => _db = db;

    public async Task<Room?> GetByIdAsync(int id) =>
        await _db.Rooms.Include(r => r.RoomType).Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Room>> GetByHotelIdAsync(int hotelId) =>
        await _db.Rooms.Where(r => r.HotelId == hotelId && r.IsActive).ToListAsync();

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int? roomTypeId = null)
    {
        var query = _db.Rooms.Include(r => r.RoomType)
            .Where(r => r.HotelId == hotelId && r.IsActive && r.Status == "Available");
        if (roomTypeId.HasValue)
            query = query.Where(r => r.RoomTypeId == roomTypeId.Value);
        return await query.ToListAsync();
    }

    public async Task<Room> CreateAsync(Room room)
    {
        await _db.Rooms.AddAsync(room);
        await _db.SaveChangesAsync();
        return room;
    }

    public void Update(Room room) => _db.Rooms.Update(room);
}

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly ApplicationDbContext _db;

    public RoomTypeRepository(ApplicationDbContext db) => _db = db;

    public async Task<RoomType?> GetByIdAsync(int id) =>
        await _db.RoomTypes.Include(rt => rt.Rooms).FirstOrDefaultAsync(rt => rt.Id == id);

    public async Task<IEnumerable<RoomType>> GetByHotelIdAsync(int hotelId) =>
        await _db.RoomTypes.Where(rt => rt.HotelId == hotelId && rt.IsActive).ToListAsync();

    public async Task<RoomType> CreateAsync(RoomType roomType)
    {
        await _db.RoomTypes.AddAsync(roomType);
        await _db.SaveChangesAsync();
        return roomType;
    }

    public void Update(RoomType roomType) => _db.RoomTypes.Update(roomType);
}

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _db;

    public PaymentRepository(ApplicationDbContext db) => _db = db;

    public async Task<Payment?> GetByIdAsync(int id) =>
        await _db.Payments.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Payment>> GetByBookingIdAsync(int bookingId) =>
        await _db.Payments.Where(p => p.BookingId == bookingId).ToListAsync();

    public async Task<Payment?> GetByTransactionIdAsync(string transactionId) =>
        await _db.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);

    public async Task<Payment> CreateAsync(Payment payment)
    {
        await _db.Payments.AddAsync(payment);
        await _db.SaveChangesAsync();
        return payment;
    }

    public void Update(Payment payment) => _db.Payments.Update(payment);
}

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _db;

    public ReviewRepository(ApplicationDbContext db) => _db = db;

    public async Task<Review?> GetByIdAsync(int id) =>
        await _db.Reviews.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId, bool? approved = null)
    {
        var query = _db.Reviews.Include(r => r.User).Where(r => r.HotelId == hotelId).AsQueryable();
        if (approved.HasValue)
            query = query.Where(r => r.IsApproved == approved.Value);
        return await query.OrderByDescending(r => r.ReviewDate).ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetPendingAsync() =>
        await _db.Reviews.Include(r => r.User).Where(r => !r.IsApproved).ToListAsync();

    public async Task<bool> ExistsForBookingAsync(int bookingId) =>
        await _db.Reviews.AnyAsync(r => r.BookingId == bookingId);

    public async Task<Review> CreateAsync(Review review)
    {
        await _db.Reviews.AddAsync(review);
        await _db.SaveChangesAsync();
        return review;
    }

    public void Update(Review review) => _db.Reviews.Update(review);
}

public class VoucherRepository : IVoucherRepository
{
    private readonly ApplicationDbContext _db;

    public VoucherRepository(ApplicationDbContext db) => _db = db;

    public async Task<Voucher?> GetByIdAsync(int id) =>
        await _db.Vouchers.FirstOrDefaultAsync(v => v.Id == id);

    public async Task<Voucher?> GetByCodeAsync(string code) =>
        await _db.Vouchers.FirstOrDefaultAsync(v => v.VoucherCode == code);

    public async Task<IEnumerable<Voucher>> GetValidVouchersAsync(decimal orderAmount) =>
        await _db.Vouchers.Where(v => v.IsActive && v.ValidFrom <= DateTime.UtcNow && v.ValidTo >= DateTime.UtcNow
            && (v.UsageLimit == null || v.UsedCount < v.UsageLimit)
            && (v.MinOrderAmount == null || orderAmount >= v.MinOrderAmount))
            .ToListAsync();

    public async Task<Voucher> CreateAsync(Voucher voucher)
    {
        await _db.Vouchers.AddAsync(voucher);
        await _db.SaveChangesAsync();
        return voucher;
    }

    public void Update(Voucher voucher) => _db.Vouchers.Update(voucher);
}
