using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Admin.Controllers;

public class BookingsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
