using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Web.Controllers;

public class ReviewsController : Controller
{
    public IActionResult Create() => View();
}
