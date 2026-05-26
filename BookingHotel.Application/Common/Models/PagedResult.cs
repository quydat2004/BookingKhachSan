using BookingHotel.Domain.Common;

namespace BookingHotel.Application.Common.Models;

public class PagedResult<T> : Domain.Common.PagedResult<T>
{
    public PagedResult() { }
    public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        : base(items, totalCount, page, pageSize) { }
}
