using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingHotel.Domain.Entities;

namespace BookingHotel.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("RoleId").ValueGeneratedOnAdd();
        builder.Property(r => r.RoleName).HasMaxLength(50).IsRequired();
        builder.HasIndex(r => r.RoleName).IsUnique();
        builder.Property(r => r.Description).HasMaxLength(200);
        builder.Ignore(r => r.UpdatedAt);
    }
}

public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.ToTable("Amenities");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("AmenityId").ValueGeneratedOnAdd();
        builder.Property(a => a.AmenityName).HasMaxLength(100).IsRequired();
        builder.Property(a => a.IconClass).HasMaxLength(50);
        builder.Property(a => a.Category).HasMaxLength(50);
        builder.Ignore(a => a.UpdatedAt);
    }
}

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("Images");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("ImageId").ValueGeneratedOnAdd();
        builder.Property(i => i.ImageUrl).HasMaxLength(500).IsRequired();
        builder.Property(i => i.AltText).HasMaxLength(200);
        builder.Property(i => i.EntityType).HasColumnType("tinyint");
        builder.Ignore(i => i.UpdatedAt);
    }
}

public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("RoomTypes");
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id).HasColumnName("RoomTypeId").ValueGeneratedOnAdd();
        builder.Property(rt => rt.TypeName).HasMaxLength(100).IsRequired();
        builder.Property(rt => rt.Description).HasColumnType("nvarchar(max)");
        builder.Property(rt => rt.BedType).HasMaxLength(50);
        builder.Property(rt => rt.RoomSize).HasColumnType("decimal(10,2)");
        builder.Property(rt => rt.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(rt => rt.WeekendPrice).HasColumnType("decimal(18,2)");
        builder.Property(rt => rt.HolidayPrice).HasColumnType("decimal(18,2)");
        builder.HasOne(rt => rt.Hotel).WithMany(h => h.RoomTypes).HasForeignKey(rt => rt.HotelId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(rt => rt.RoomAmenities).WithOne(ra => ra.RoomType).HasForeignKey(ra => ra.RoomTypeId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class HotelAmenityConfiguration : IEntityTypeConfiguration<HotelAmenity>
{
    public void Configure(EntityTypeBuilder<HotelAmenity> builder)
    {
        builder.ToTable("HotelAmenities");
        builder.HasKey(ha => ha.Id);
        builder.Property(ha => ha.Id).ValueGeneratedOnAdd();
        builder.HasOne(ha => ha.Hotel).WithMany(h => h.HotelAmenities).HasForeignKey(ha => ha.HotelId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ha => ha.Amenity).WithMany(a => a.HotelAmenities).HasForeignKey(ha => ha.AmenityId);
        builder.HasIndex(ha => new { ha.HotelId, ha.AmenityId }).IsUnique();
    }
}

public class RoomAmenityConfiguration : IEntityTypeConfiguration<RoomAmenity>
{
    public void Configure(EntityTypeBuilder<RoomAmenity> builder)
    {
        builder.ToTable("RoomAmenities");
        builder.HasKey(ra => ra.Id);
        builder.Property(ra => ra.Id).ValueGeneratedOnAdd();
        builder.HasOne(ra => ra.RoomType).WithMany(rt => rt.RoomAmenities).HasForeignKey(ra => ra.RoomTypeId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ra => ra.Amenity).WithMany(a => a.RoomAmenities).HasForeignKey(ra => ra.AmenityId);
        builder.HasIndex(ra => new { ra.RoomTypeId, ra.AmenityId }).IsUnique();
    }
}

public class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetail>
{
    public void Configure(EntityTypeBuilder<BookingDetail> builder)
    {
        builder.ToTable("BookingDetails");
        builder.HasKey(bd => bd.DetailId);
        builder.Property(bd => bd.DetailId).ValueGeneratedOnAdd();
        builder.Property(bd => bd.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(bd => bd.SubTotal).HasColumnType("decimal(18,2)").IsRequired();

        // Fix cascade paths - use NO ACTION for Room and RoomType relationships
        builder.HasOne(bd => bd.Booking).WithMany(b => b.BookingDetails).HasForeignKey(bd => bd.BookingId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(bd => bd.Room).WithMany(r => r.BookingDetails).HasForeignKey(bd => bd.RoomId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(bd => bd.RoomType).WithMany().HasForeignKey(bd => bd.RoomTypeId).OnDelete(DeleteBehavior.NoAction);
    }
}

public class BookingVoucherConfiguration : IEntityTypeConfiguration<BookingVoucher>
{
    public void Configure(EntityTypeBuilder<BookingVoucher> builder)
    {
        builder.ToTable("BookingVouchers");
        builder.HasKey(bv => bv.Id);
        builder.Property(bv => bv.Id).ValueGeneratedOnAdd();
        builder.Property(bv => bv.DiscountAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.HasOne(bv => bv.Booking).WithMany(b => b.BookingVouchers).HasForeignKey(bv => bv.BookingId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(bv => bv.Voucher).WithMany(v => v.BookingVouchers).HasForeignKey(bv => bv.VoucherId);
    }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("PaymentId").ValueGeneratedOnAdd();
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.PaymentMethod).HasMaxLength(50).IsRequired();
        builder.Property(p => p.PaymentStatus).HasMaxLength(50).IsRequired();
        builder.Property(p => p.TransactionId).HasMaxLength(100);

        // Fix cascade paths - use NO ACTION for User relationship
        builder.HasOne(p => p.Booking).WithMany(b => b.Payments).HasForeignKey(p => p.BookingId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(p => p.User).WithMany(u => u.Payments).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.NoAction);
    }
}

public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.ToTable("Vouchers");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("VoucherId").ValueGeneratedOnAdd();
        builder.Property(v => v.VoucherCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(v => v.VoucherCode).IsUnique();
        builder.Property(v => v.DiscountType).HasMaxLength(20).IsRequired();
        builder.Property(v => v.DiscountValue).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(v => v.MinOrderAmount).HasColumnType("decimal(18,2)");
        builder.Property(v => v.MaxDiscount).HasColumnType("decimal(18,2)");
        builder.Ignore(v => v.UpdatedAt);
    }
}

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("ReviewId").ValueGeneratedOnAdd();
        builder.Property(r => r.Comment).HasMaxLength(1000);

        // Fix cascade paths - use NO ACTION for all relationships
        builder.HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Hotel).WithMany(h => h.Reviews).HasForeignKey(r => r.HotelId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Booking).WithMany(b => b.Reviews).HasForeignKey(r => r.BookingId).OnDelete(DeleteBehavior.NoAction);
    }
}
