using Microsoft.EntityFrameworkCore;
using BookingHotel.Domain.Entities;
using BookingHotel.Domain.Entities.Views;
using BookingHotel.Domain.Common;

namespace BookingHotel.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<HotelAmenity> HotelAmenities => Set<HotelAmenity>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<RoomType> RoomTypes => Set<RoomType>();
    public DbSet<RoomAmenity> RoomAmenities => Set<RoomAmenity>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingDetail> BookingDetails => Set<BookingDetail>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<BookingVoucher> BookingVouchers => Set<BookingVoucher>();
    public DbSet<Review> Reviews => Set<Review>();

    // Views
    public DbSet<AvailableRoomView> AvailableRoomViews => Set<AvailableRoomView>();
    public DbSet<BookingSummaryView> BookingSummaryViews => Set<BookingSummaryView>();
    public DbSet<RevenueReportView> RevenueReportViews => Set<RevenueReportView>();
    public DbSet<HotelRatingView> HotelRatingViews => Set<HotelRatingView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        return await base.SaveChangesAsync(ct);
    }
}
