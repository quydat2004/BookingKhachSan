using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Admin.Controllers;

public class ReviewsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
