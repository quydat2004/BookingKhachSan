using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingHotel.Domain.Entities.Views;

namespace BookingHotel.Infrastructure.Configurations;

public class AvailableRoomViewConfiguration : IEntityTypeConfiguration<AvailableRoomView>
{
    public void Configure(EntityTypeBuilder<AvailableRoomView> builder)
    {
        builder.ToView("vw_AvailableRooms");
        builder.HasNoKey();
    }
}

public class BookingSummaryViewConfiguration : IEntityTypeConfiguration<BookingSummaryView>
{
    public void Configure(EntityTypeBuilder<BookingSummaryView> builder)
    {
        builder.ToView("vw_BookingSummary");
        builder.HasNoKey();
    }
}

public class RevenueReportViewConfiguration : IEntityTypeConfiguration<RevenueReportView>
{
    public void Configure(EntityTypeBuilder<RevenueReportView> builder)
    {
        builder.ToView("vw_RevenueReport");
        builder.HasNoKey();
    }
}

public class HotelRatingViewConfiguration : IEntityTypeConfiguration<HotelRatingView>
{
    public void Configure(EntityTypeBuilder<HotelRatingView> builder)
    {
        builder.ToView("vw_HotelRatings");
        builder.HasNoKey();
    }
}
