using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingHotel.Domain.Entities;

namespace BookingHotel.Infrastructure.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("RoomId").ValueGeneratedOnAdd();
        builder.Property(r => r.RoomNumber).HasMaxLength(10).IsRequired();
        builder.Property(r => r.Status).HasMaxLength(50).HasDefaultValue("Available");
        builder.Property(r => r.Notes).HasMaxLength(500);

        // Fix cascade paths - use NO ACTION for RoomType relationship
        builder.HasOne(r => r.Hotel).WithMany(h => h.Rooms).HasForeignKey(r => r.HotelId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.RoomType).WithMany(rt => rt.Rooms).HasForeignKey(r => r.RoomTypeId).OnDelete(DeleteBehavior.NoAction);
        builder.HasIndex(r => new { r.HotelId, r.RoomNumber }).IsUnique();
    }
}

