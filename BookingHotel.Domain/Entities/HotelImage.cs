namespace BookingHotel.Domain.Entities;

public class Image : Common.BaseEntity
{
    public byte EntityType { get; set; }
    public int EntityId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsThumbnail { get; set; }
    public int DisplayOrder { get; set; }
    public string? AltText { get; set; }
}
