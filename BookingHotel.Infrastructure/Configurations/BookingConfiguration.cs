using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingHotel.Domain.Entities;

namespace BookingHotel.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("UserId").ValueGeneratedOnAdd();
        builder.Property(u => u.FullName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.AvatarUrl).HasMaxLength(500);
        builder.Property(u => u.RefreshToken).HasMaxLength(500);
        builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
    }
}

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotels");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasColumnName("HotelId").ValueGeneratedOnAdd();
        builder.Property(h => h.HotelName).HasMaxLength(200).IsRequired();
        builder.Property(h => h.Address).HasMaxLength(500).IsRequired();
        builder.Property(h => h.City).HasMaxLength(100).IsRequired();
        builder.Property(h => h.District).HasMaxLength(100);
        builder.Property(h => h.Description).HasColumnType("nvarchar(max)");
        builder.Property(h => h.Phone).HasMaxLength(20);
        builder.Property(h => h.Email).HasMaxLength(256);
        builder.Property(h => h.Website).HasMaxLength(200);
        builder.Property(h => h.Status).HasMaxLength(50).HasDefaultValue("Pending");

        // Decimal properties with precision
        builder.Property(h => h.Latitude).HasColumnType("decimal(10,7)");
        builder.Property(h => h.Longitude).HasColumnType("decimal(10,7)");

        builder.HasOne(h => h.Manager).WithMany(u => u.ManagedHotels).HasForeignKey(h => h.ManagerId);
    }
}

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("BookingId").ValueGeneratedOnAdd();
        builder.Property(b => b.BookingCode).HasMaxLength(20).IsRequired();
        builder.HasIndex(b => b.BookingCode).IsUnique();
        builder.Property(b => b.SpecialRequests).HasMaxLength(1000);
        builder.Property(b => b.CancellationReason).HasMaxLength(500);
        builder.Property(b => b.Status).HasMaxLength(50).HasDefaultValue("Pending");

        // Decimal properties with precision
        builder.Property(b => b.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(b => b.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.RefundAmount).HasColumnType("decimal(18,2)");

        // Fix cascade paths - use NO ACTION for User relationship
        builder.HasOne(b => b.User).WithMany(u => u.Bookings).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(b => b.Hotel).WithMany(h => h.Bookings).HasForeignKey(b => b.HotelId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(b => b.BookingDetails).WithOne(d => d.Booking).HasForeignKey(d => d.BookingId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(b => b.Payments).WithOne(p => p.Booking).HasForeignKey(p => p.BookingId);
    }
}
