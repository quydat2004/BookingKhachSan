using Microsoft.EntityFrameworkCore;
using BookingHotel.Domain.Entities;

namespace BookingHotel.Infrastructure.Data;

public class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext db)
    {
        await EnsureRolesAsync(db);
        await EnsureUsersAsync(db);
        await EnsureAmenitiesAsync(db);
        await EnsureHotelsAsync(db);
        await EnsureRoomTypesAsync(db);
        await EnsureRoomsAsync(db);
        await EnsureVouchersAsync(db);
    }

    private static async Task EnsureRolesAsync(ApplicationDbContext db)
    {
        var roles = new[]
        {
            new Role { RoleName = "Admin", Description = "System administrator" },
            new Role { RoleName = "HotelManager", Description = "Hotel manager" },
            new Role { RoleName = "Customer", Description = "Customer" }
        };

        foreach (var role in roles)
        {
            if (!await db.Roles.AnyAsync(r => r.RoleName == role.RoleName))
            {
                await db.Roles.AddAsync(role);
            }
        }

        await db.SaveChangesAsync();
    }

    private static async Task EnsureUsersAsync(ApplicationDbContext db)
    {
        var adminRoleId = await db.Roles.Where(r => r.RoleName == "Admin").Select(r => r.Id).FirstAsync();
        var managerRoleId = await db.Roles.Where(r => r.RoleName == "HotelManager").Select(r => r.Id).FirstAsync();
        var customerRoleId = await db.Roles.Where(r => r.RoleName == "Customer").Select(r => r.Id).FirstAsync();
        var defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");

        var users = new[]
        {
            new User
            {
                FullName = "Admin System",
                Email = "admin@booking.com",
                PasswordHash = defaultPasswordHash,
                RoleId = adminRoleId,
                IsActive = true
            },
            new User
            {
                FullName = "Nguyen Van A",
                Email = "manager@booking.com",
                PasswordHash = defaultPasswordHash,
                RoleId = managerRoleId,
                IsActive = true
            },
            new User
            {
                FullName = "Le Van C",
                Email = "customer@gmail.com",
                PasswordHash = defaultPasswordHash,
                RoleId = customerRoleId,
                IsActive = true
            }
        };

        foreach (var user in users)
        {
            var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser is null)
            {
                await db.Users.AddAsync(user);
                continue;
            }

            existingUser.PasswordHash = defaultPasswordHash;
            existingUser.RoleId = user.RoleId;
            existingUser.IsActive = true;
        }

        await db.SaveChangesAsync();
    }

    private static async Task EnsureAmenitiesAsync(ApplicationDbContext db)
    {
        var amenities = new[]
        {
            new Amenity { AmenityName = "Free WiFi", IconClass = "fa-wifi", Category = "General" },
            new Amenity { AmenityName = "Swimming Pool", IconClass = "fa-swimmer", Category = "Recreation" },
            new Amenity { AmenityName = "Gym", IconClass = "fa-dumbbell", Category = "Recreation" },
            new Amenity { AmenityName = "Parking", IconClass = "fa-parking", Category = "General" },
            new Amenity { AmenityName = "Restaurant", IconClass = "fa-utensils", Category = "Dining" },
            new Amenity { AmenityName = "Air Conditioning", IconClass = "fa-snowflake", Category = "Room" },
            new Amenity { AmenityName = "Flat-screen TV", IconClass = "fa-tv", Category = "Room" },
            new Amenity { AmenityName = "Mini Bar", IconClass = "fa-glass-cheers", Category = "Room" },
            new Amenity { AmenityName = "Bathtub", IconClass = "fa-bath", Category = "Room" },
            new Amenity { AmenityName = "Airport Shuttle", IconClass = "fa-shuttle-van", Category = "Service" }
        };

        foreach (var amenity in amenities)
        {
            if (!await db.Amenities.AnyAsync(a => a.AmenityName == amenity.AmenityName))
            {
                await db.Amenities.AddAsync(amenity);
            }
        }

        await db.SaveChangesAsync();
    }

    private static async Task EnsureHotelsAsync(ApplicationDbContext db)
    {
        var managerId = await db.Users.Where(u => u.Email == "manager@booking.com").Select(u => u.Id).FirstAsync();
        var hotels = new[]
        {
            new Hotel
            {
                ManagerId = managerId,
                HotelName = "Khách sạn Hoàng Gia Huế",
                Address = "123 Đường Lê Lợi",
                City = "Hue",
                District = "Phú Hội",
                Description = "Khách sạn 5 sao sang trọng tại trung tâm thành phố Huế.",
                StarRating = 5,
                Phone = "0234123456",
                Email = "info@hoanggia.com",
                Status = "Approved",
                IsActive = true
            },
            new Hotel
            {
                ManagerId = managerId,
                HotelName = "Resort Biển Xanh Đà Nẵng",
                Address = "456 Võ Nguyên Giáp",
                City = "Da Nang",
                District = "Ngũ Hành Sơn",
                Description = "Khu nghỉ dưỡng cao cấp ven biển Đà Nẵng.",
                StarRating = 4,
                Phone = "0236567890",
                Email = "info@bienxanh.com",
                Status = "Approved",
                IsActive = true
            }
        };

        foreach (var hotel in hotels)
        {
            if (!await db.Hotels.AnyAsync(h => h.HotelName == hotel.HotelName))
            {
                await db.Hotels.AddAsync(hotel);
            }
        }

        await db.SaveChangesAsync();
    }

    private static async Task EnsureRoomTypesAsync(ApplicationDbContext db)
    {
        var hueHotelId = await db.Hotels.Where(h => h.HotelName == "Khách sạn Hoàng Gia Huế").Select(h => h.Id).FirstAsync();
        var daNangHotelId = await db.Hotels.Where(h => h.HotelName == "Resort Biển Xanh Đà Nẵng").Select(h => h.Id).FirstAsync();

        var roomTypes = new[]
        {
            new RoomType { HotelId = hueHotelId, TypeName = "Standard", Price = 800000, WeekendPrice = 960000, Capacity = 2, BedCount = 1, BedType = "Queen", Description = "Phòng tiêu chuẩn" },
            new RoomType { HotelId = hueHotelId, TypeName = "Deluxe", Price = 1200000, WeekendPrice = 1440000, Capacity = 2, BedCount = 1, BedType = "King", Description = "Phòng cao cấp" },
            new RoomType { HotelId = hueHotelId, TypeName = "Suite", Price = 2500000, WeekendPrice = 3000000, Capacity = 4, BedCount = 2, BedType = "King", Description = "Phòng hạng sang" },
            new RoomType { HotelId = daNangHotelId, TypeName = "Standard", Price = 600000, WeekendPrice = 720000, Capacity = 2, BedCount = 1, BedType = "Queen", Description = "Phòng hướng vườn" },
            new RoomType { HotelId = daNangHotelId, TypeName = "Family", Price = 1800000, WeekendPrice = 2160000, Capacity = 5, BedCount = 3, BedType = "Queen", Description = "Phòng gia đình" }
        };

        foreach (var roomType in roomTypes)
        {
            if (!await db.RoomTypes.AnyAsync(rt => rt.HotelId == roomType.HotelId && rt.TypeName == roomType.TypeName))
            {
                await db.RoomTypes.AddAsync(roomType);
            }
        }

        await db.SaveChangesAsync();
    }

    private static async Task EnsureRoomsAsync(ApplicationDbContext db)
    {
        var roomTypes = await db.RoomTypes.ToListAsync();
        foreach (var roomType in roomTypes)
        {
            for (var index = 1; index <= 3; index++)
            {
                var roomNumber = $"{roomType.Id}{index:00}";
                if (!await db.Rooms.AnyAsync(r => r.RoomTypeId == roomType.Id && r.RoomNumber == roomNumber))
                {
                    await db.Rooms.AddAsync(new Room
                    {
                        HotelId = roomType.HotelId,
                        RoomTypeId = roomType.Id,
                        RoomNumber = roomNumber,
                        FloorNumber = roomType.Id,
                        Status = "Available",
                        IsActive = true
                    });
                }
            }
        }

        await db.SaveChangesAsync();
    }

    private static async Task EnsureVouchersAsync(ApplicationDbContext db)
    {
        var vouchers = new[]
        {
            new Voucher { VoucherCode = "WELCOME10", DiscountType = "Percentage", DiscountValue = 10, MaxDiscount = 500000, ValidFrom = DateTime.UtcNow.AddDays(-1), ValidTo = DateTime.UtcNow.AddYears(1), UsageLimit = 100, IsActive = true },
            new Voucher { VoucherCode = "SAVE200", DiscountType = "FixedAmount", DiscountValue = 200000, MinOrderAmount = 1000000, ValidFrom = DateTime.UtcNow.AddDays(-1), ValidTo = DateTime.UtcNow.AddYears(1), UsageLimit = 50, IsActive = true }
        };

        foreach (var voucher in vouchers)
        {
            if (!await db.Vouchers.AnyAsync(v => v.VoucherCode == voucher.VoucherCode))
            {
                await db.Vouchers.AddAsync(voucher);
            }
        }

        await db.SaveChangesAsync();
    }
}
